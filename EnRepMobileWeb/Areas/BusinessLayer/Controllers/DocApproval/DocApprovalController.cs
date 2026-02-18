using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.DocApproval;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.DocApproval;
using Newtonsoft.Json.Linq;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.DocApproval
{ 
    public class DocApprovalController : Controller
    {
        string DocumentMenuId = "103140",title;
        DocApprovalModel _docApprovalModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DocApproval_ISERVICES _docApproval_ISERVICES;
        Common_IServices _Common_IServices;
        
        // GET: BusinessLayer/DocApproval

        public DocApprovalController(Common_IServices _Common_IServices,DocApproval_ISERVICES docApproval_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._docApproval_ISERVICES = docApproval_ISERVICES;
        }
       
        public ActionResult Documentlist(string DocName,string branch_id)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(DocName))
                {
                    GroupName = "";
                }
                else
                {
                    GroupName = DocName;
                }
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                list = _docApproval_ISERVICES.doc_list(GroupName, CompID, branch_id);
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(list.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DocumentlistDL(string DocName)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(DocName))
                {
                    GroupName = "";
                }
                else
                {
                    GroupName = DocName;
                }
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string branch_id="";
                list = _docApproval_ISERVICES.doc_list(GroupName, CompID,branch_id);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(list.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DocAppList(string Br_id,string Doc_id)
        {
            try
            {
                DocApprovalModel docApprovalModel = new DocApprovalModel();
                if (Doc_id == "" || Doc_id == null)
                {
                    Doc_id = "0";
                }
                if (Br_id == "" || Br_id == null)
                {
                    Br_id = "0";
                }
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }


                DataTable dt = _docApproval_ISERVICES.getDocAppListFilter(CompID,Br_id, Doc_id).Tables[0];
                //ViewBag.DocAppList = dt;
                docApprovalModel.DocAppList = dt;
                //Session["DASearch"] = "DA_Search";
                docApprovalModel.DASearch = "DA_Search";
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialApprovalList.cshtml", docApprovalModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDocAppDetails(DocApprovalModel docApprovalModel,string Command)
        {
            try
            {
                if (docApprovalModel.hdnAction == "Delete")
                {
                    Command = "Delete";
                }
                DocApprovalModel docApprovalM = new DocApprovalModel();
                switch (Command)
                {
                    case "Save":
                        if (docApprovalModel.TransType == null)
                        {
                            docApprovalModel.TransType = "Save";
                        }
                        string Message = InsertData(docApprovalModel, Command);
                        //Session["BtnName"] = "Save";
                        docApprovalModel.btnName = "Save";
                        TempData["Modeldata"] = docApprovalModel;                      
                        docApprovalM.EditCode = docApprovalModel.EditCode;
                        docApprovalM.Branch_id = docApprovalModel.Branch_id;
                        //var EditCode= docApprovalModel.EditCode;
                        return RedirectToAction("AddApprovalDetail", docApprovalM);
                    case "Edit":
                        docApprovalModel.hdnSavebtn = "";
                        docApprovalModel.TransType = "Update";
                        docApprovalModel.btnName = "Update";
                        string br_id = docApprovalModel.Branch_id;
                        br_id += "-" + docApprovalModel.Doc_id;
                        docApprovalModel.EditCode = br_id;
                        Message = checkEditClick(docApprovalModel, Command);
                        if (Message == "Used")
                        {
                            docApprovalModel.Message = Message;
                            docApprovalModel.btnName = "dblClick";
                            docApprovalModel.TransType = "Edit";
                        }
                        //docApprovalModel.Message = Message;
                        TempData["Modeldata"] = docApprovalModel;
                        docApprovalM.EditCode= br_id;
                        //docApprovalM.Branch_id = docApprovalModel.Branch_id;
                        //return RedirectToAction("AddApprovalDetail", _docApprovalModel);
                        return RedirectToAction("AddApprovalDetail", docApprovalM);
                    case "Delete":
                        br_id = docApprovalModel.Branch_id;
                        string doc_id = docApprovalModel.Doc_id;
                        string CompID = "";
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        //Session["DeleteCode"] = br_id;
                        docApprovalModel.DeleteCode = br_id;
                        //Session["EditCode"] = "";
                        docApprovalModel.EditCode = "";
                        Message = checkEditClick(docApprovalModel, Command);
                        if (Message == "Used")
                        {
                            docApprovalModel.Message = "UnableToDelete";
                            docApprovalModel.btnName = "dblClick";
                            docApprovalModel.TransType = "Edit";
                        }
                        else
                        {
                            Message = _docApproval_ISERVICES.DeleteDocAddDetails(br_id, doc_id, CompID);
                            docApprovalModel.Message = Message;
                        }
                        //Session["Message"] = Message;

                        //Session["BtnName"] = "Refresh";
                        docApprovalModel.hdnSavebtn = "";
                        docApprovalModel.btnName = "Refresh";
                        docApprovalModel.hdnAction = null;
                        TempData["Modeldata"] = docApprovalModel;
                        //return RedirectToAction("AddApprovalDetail", _docApprovalModel);
                        return RedirectToAction("AddApprovalDetail");
                    case "AddtoList":
                        //Session["BtnName"] = "AddNew";
                        docApprovalModel.hdnSavebtn = "";
                        docApprovalModel.btnName = "AddNew";
                        //Session["EditCode"] = "";
                        docApprovalModel.EditCode = "";
                        //TempData["Modeldata"] = docApprovalModel;
                        //return RedirectToAction("AddApprovalDetail", _docApprovalModel);
                        return RedirectToAction("AddApprovalDetail");
                    case "Refresh":
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "Refresh";
                        docApprovalModel.hdnSavebtn = "";
                        docApprovalModel.btnName = "Refresh";
                        //Session["EditCode"] = "";
                        docApprovalModel.EditCode = "";
                        docApprovalModel.TransType = "Edit";
                        TempData["Modeldata"] = docApprovalModel;
                        //return RedirectToAction("AddApprovalDetail", _docApprovalModel);
                        return RedirectToAction("AddApprovalDetail");
                    case "BacktoList":
                        //Session["EditCode"] = "";
                        docApprovalModel.EditCode = "";
                        return RedirectToAction("DocApproval", _docApprovalModel);
                }


                return View();
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public string checkEditClick(DocApprovalModel docApprovalModel, string Command)
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = docApprovalModel.Branch_id;
                string doc_id = docApprovalModel.Doc_id;
                String Message = _docApproval_ISERVICES.checkEditClick(CompID, br_id, doc_id);
                TempData["Modeldata"] = docApprovalModel;
                return Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
                //   throw ex;
            }
        }
        public string InsertData(DocApprovalModel docApprovalModel, string Command)
        {
            try
            {

                string br_id = docApprovalModel.Branch_id; //Session["branch_id"].ToString(); 
                string create_id = "";
                string CompID = "0";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["userid"] != null)
                {
                    create_id = Session["userid"].ToString();
                }
                string doc_id = docApprovalModel.Doc_id; //Session["doc_id"].ToString(); 
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                string mac_id = SystemDetail;

                DataTable DocAppHeader = new DataTable();
                DataTable DocAppUserList = new DataTable();

                DataTable dtDocAppHeader = new DataTable();
                dtDocAppHeader.Columns.Add("comp_id", typeof(int));
                dtDocAppHeader.Columns.Add("br_id", typeof(int));
                dtDocAppHeader.Columns.Add("create_id", typeof(int));
                dtDocAppHeader.Columns.Add("mac_id", typeof(string));
                dtDocAppHeader.Columns.Add("doc_id", typeof(string));
                dtDocAppHeader.Columns.Add("TransType", typeof(string));

                DataRow dtrowDocAppHeader = dtDocAppHeader.NewRow();
                dtrowDocAppHeader["comp_id"] = CompID;
                dtrowDocAppHeader["br_id"] = br_id;
                dtrowDocAppHeader["create_id"] = create_id;
                dtrowDocAppHeader["mac_id"] = mac_id;
                dtrowDocAppHeader["doc_id"] = doc_id;
                //dtrowDocAppHeader["TransType"] = Session["TransType"].ToString();
                dtrowDocAppHeader["TransType"] = docApprovalModel.TransType;
                dtDocAppHeader.Rows.Add(dtrowDocAppHeader);

                DocAppHeader = dtDocAppHeader;

                DataTable dtUserlist = new DataTable();

                dtUserlist.Columns.Add("level", typeof(int));
                dtUserlist.Columns.Add("userID", typeof(int));
                dtUserlist.Columns.Add("limit", typeof(float));

                JArray AddObject = JArray.Parse(docApprovalModel.UserDetailslist);
                for (int i = 0; i < AddObject.Count; i++)
                {
                    DataRow dtrowUserlist = dtUserlist.NewRow();


                    dtrowUserlist["level"] = AddObject[i]["level"].ToString();
                    dtrowUserlist["userID"] = AddObject[i]["userID"].ToString();
                    if (AddObject[i]["limit"].ToString() == "")
                    {
                        dtrowUserlist["limit"] = 0;
                    }
                    else
                    {
                        dtrowUserlist["limit"] = AddObject[i]["limit"].ToString();
                    }


                    dtUserlist.Rows.Add(dtrowUserlist);
                }
                DocAppUserList = dtUserlist;
                String SaveMessage = _docApproval_ISERVICES.InsertDocAppDetails(DocAppHeader, DocAppUserList);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                if (Message == "Update" || Message == "Save")
                {
                    //Session["EditCode"] = ItemCode;
                    docApprovalModel.EditCode = ItemCode;
                    //Session["Message"] = "Save";
                    docApprovalModel.Message = "Save";
                    //  Session["ItemCode"] = ItemCode;
                    // Session["TransType"] = "Update";
                }
                if (Message == "Duplicate")
                {
                    //Session["TransType"] = "Duplicate";
                    docApprovalModel.hdnSavebtn = "Duplicate";
                    docApprovalModel.TransType = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    docApprovalModel.Message = "Duplicate";
                    //  Session["ItemCode"] = ItemCode;
                }
                TempData["Modeldata"] = docApprovalModel;
                return  Message ;
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
             //   throw ex;
            }
        }
          private void CommonPageDetails()
        {
            try
            {
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string UserID = string.Empty;
                string language = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        public ActionResult Userlist( string Search,string branch_id,string DocName)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
              
                if (string.IsNullOrEmpty(Search))
                {
                    GroupName = "";
                }
                else
                {
                    GroupName = Search;
                }
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
               // branch_id = "2";
              //  branch_id= docApprovalModel.hdnBranch_id;
                list = _docApproval_ISERVICES.user_list(GroupName, CompID, branch_id, DocName);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(list.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DocApproval(DocApprovalModel docApprovalModel)
        {
            try
            {
                string CompID = string.Empty;
                string  branch_id = string.Empty;
             
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branch_id = Session["BranchId"].ToString();
                }
              
                DataTable dt = _docApproval_ISERVICES.getDocAppList(CompID, branch_id).Tables[0];
                //ViewBag.DocAppList = dt;
                docApprovalModel.DocAppList = dt;
                //Session["EditCode"] = "";
                docApprovalModel.EditCode = "";
                //Session["BtnName"] = "AddNew";
                docApprovalModel.btnName = "AddNew";
                //Session["Message"] = "New";
                docApprovalModel.Message = "New";
                //Session["TransType"] = "";
                docApprovalModel.TransType = "";
                DataSet DsBranch = getAppDocDetails();
                List<Branch_list> branch_Lists = new List<Branch_list>();
                DataRow DRow = DsBranch.Tables[0].NewRow();
                DRow["Comp_id"] = "0";
                DRow["comp_nm"] = "---All---";
                DsBranch.Tables[0].Rows.InsertAt(DRow, 0);
                foreach (DataRow dr in DsBranch.Tables[0].Rows)
                {
                    Branch_list _brlist = new Branch_list();
                    _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                    _brlist.BranchName = dr["comp_nm"].ToString();
                    branch_Lists.Add(_brlist);
                }
                docApprovalModel._branchList = branch_Lists;
                List<Doc_list> _doclist = new List<Doc_list>();
                
                foreach (DataRow dr in DsBranch.Tables[1].Rows)
                {
                    Doc_list _doc_list = new Doc_list();
                    _doc_list.DocID = dr["doc_id"].ToString();
                    _doc_list.Document = dr["doc_name_eng"].ToString();
                    _doclist.Add(_doc_list);
                }
                docApprovalModel._docList = _doclist;

                docApprovalModel.Branch_id = branch_id;
                ViewBag.MenuPageName = getDocumentName();
                docApprovalModel.Title = title;
                //Session["DASearch"] = "0";
                docApprovalModel.DASearch = "0";
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/DocApproval/ApprovalList.cshtml", docApprovalModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path,ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult DocAppSave(DocApprovalModel docApprovalModel, string Command)
        {
            try
            {

                ViewBag.MenuPageName = getDocumentName();
                docApprovalModel.Title = title;
                return View("~/Areas/BusinessLayer/Views/DocApproval/ApprovalList.cshtml", docApprovalModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private string getDocumentName()
        {
            try
            {
                string CompID = string.Empty;
                string language = string.Empty;
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
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;//Json("ErrorPage");
            }
           
        }
        private DataSet getAppDocDetails()
        {
            try
            {
                string CompID = string.Empty;
                string language = string.Empty;
                string create_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    create_id = Session["userid"].ToString();
                }
                string flag = "ddlbranch";
                DataSet DocumentName = _docApproval_ISERVICES.getAppDocDetails(flag, CompID, create_id);
                return DocumentName;
            }
          catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult AddApprovalDetail(string EditCode, DocApprovalModel docApprovalM,string br_id,string doc_id)
        {
            try
            {
                var docApprovalModel = TempData["Modeldata"] as DocApprovalModel;
                if (docApprovalModel != null)
                {
                    //Session["TransType"] = "Save";
                    if (docApprovalModel.TransType == null)
                    {
                        docApprovalModel.TransType = "Save";
                    }
                    //Session["Command"] = "Add";
                    docApprovalModel.Command = "Add";
                    //Session["AppStatus"] = 'D';
                    docApprovalModel.AppStatus = "D";

                    //  Session["BtnName"] = "BtnAddNew";
                    //Session["edit"] = "";
                    string Doc_ID = "";
                    string Br_ID = "";
                    if (br_id == null)
                    {
                        //br_id = Session["EditCode"].ToString();
                        br_id = docApprovalModel.EditCode;
                        //br_id = docApprovalM.EditCode;
                    }
                    if (br_id != null && br_id != "")
                    {

                        String str = br_id;
                        Doc_ID = str.Substring(str.IndexOf('-') + 1);

                        Br_ID = str.Substring(0, str.IndexOf("-"));
                        if (docApprovalModel.EditCode != "")
                        //if (Session["EditCode"].ToString() != "")
                        {
                            if (docApprovalModel.btnName == "Save")
                            //if (Session["BtnName"].ToString() == "Save")
                            {
                                //Session["BtnName"] = "Save";
                                docApprovalModel.btnName = "Save";
                            }
                            else
                            {
                                //Session["BtnName"] = "Update";
                                docApprovalModel.btnName = "Update";
                            }
                            //Session["TransType"] = "Update";
                                docApprovalModel.TransType = "Update";
                        }
                        else
                        {
                            //Session["BtnName"] = "dblClick";
                            docApprovalModel.btnName = "dblClick";
                            //Session["TransType"] = "Edit";
                            docApprovalModel.TransType = "Edit";
                        }

                    }
                    if (docApprovalModel.Message != null)
                    //if (Session["Message"] != null)
                    {
                        //string message = Session["Message"].ToString();
                        string message = docApprovalModel.Message;
                        if (docApprovalModel.Message == "Save")
                        //if (Session["Message"].ToString() == "Save")
                        {
                            //ViewBag.Message = "Save";
                            docApprovalModel.Message = "Save";
                            //Session["TransType"] = "Edit";
                            docApprovalModel.TransType = "Edit";
                        }
                        if (docApprovalModel.Message == "Deleted")
                        //if (Session["Message"].ToString() == "Deleted")
                        {
                            //ViewBag.Message = "Deleted";
                            docApprovalModel.Message = "Deleted";
                            //Session["TransType"] = "Edit";
                            docApprovalModel.TransType = "Edit";
                        }
                        if(docApprovalModel.Message == "Used")
                        {
                            docApprovalModel.TransType = "Edit";
                            docApprovalModel.btnName = "dblClick";
                            docApprovalModel.Command = "Add";
                        }
                        if (docApprovalModel.Message == "UnableToDelete")
                        {
                            docApprovalModel.TransType = "Edit";
                            docApprovalModel.btnName = "dblClick";
                            docApprovalModel.Command = "Add";
                            Br_ID = docApprovalModel.Branch_id;
                            Doc_ID = docApprovalModel.Doc_id;
                        }
                    }
                    string CompID = "";
                    if (Session["CompID"] != null)
                    {
                        CompID = Session["CompID"].ToString();
                    }
                    //Session["Message"] = "New";
                    if (docApprovalModel.Message == null)
                    {
                        docApprovalModel.Message = "New";
                    }
                    if(docApprovalModel.btnName == "Refresh")
                    {
                        docApprovalModel.Branch_id = null;
                        docApprovalModel.Doc_id = null;
                    }

                    DataSet EditData = _docApproval_ISERVICES.getDocAppEditDetails(Br_ID, Doc_ID, CompID);
                    if (EditData.Tables[1].Rows.Count > 0)
                    {
                        //ViewBag.Data = EditData.Tables[1];
                        docApprovalModel.Data = EditData.Tables[1];

                    }
                    if (EditData.Tables[0].Rows.Count > 0)
                    {
                        //ViewBag.createdt = EditData.Tables[0].Rows[0]["create_dt"].ToString();
                        //ViewBag.moddt = EditData.Tables[0].Rows[0]["mod_dt"].ToString();
                        //ViewBag.createid = EditData.Tables[0].Rows[0]["create_id"].ToString();
                        //ViewBag.modid = EditData.Tables[0].Rows[0]["mod_id"].ToString();
                        docApprovalModel.createdt = EditData.Tables[0].Rows[0]["create_dt"].ToString();
                        docApprovalModel.moddt = EditData.Tables[0].Rows[0]["mod_dt"].ToString();
                        docApprovalModel.createid = EditData.Tables[0].Rows[0]["create_id"].ToString();
                        docApprovalModel.modid = EditData.Tables[0].Rows[0]["mod_id"].ToString();
                        docApprovalModel.Branch_id = EditData.Tables[0].Rows[0]["br_id"].ToString();
                        docApprovalModel.Doc_id = EditData.Tables[0].Rows[0]["doc_id"].ToString();
                    }

                    DataSet DsBranch = getAppDocDetails();
                    List<Branch_list> branch_Lists = new List<Branch_list>();
                    foreach (DataRow dr in DsBranch.Tables[0].Rows)
                    {
                        Branch_list _brlist = new Branch_list();
                        _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                        _brlist.BranchName = dr["comp_nm"].ToString();
                        branch_Lists.Add(_brlist);
                    }
                    docApprovalModel._branchList = branch_Lists;
                    List<Doc_list> _doclist = new List<Doc_list>();
                    foreach (DataRow dr in DsBranch.Tables[1].Rows)
                    {
                        Doc_list _doc_list = new Doc_list();
                        _doc_list.DocID = dr["doc_id"].ToString();
                        _doc_list.Document = dr["doc_name_eng"].ToString();
                        _doclist.Add(_doc_list);
                    }
                    docApprovalModel._docList = _doclist;


                    // doclist(docApprovalModel);
                    CommonPageDetails();
                    ViewBag.MenuPageName = getDocumentName();
                    docApprovalModel.Title = title;
                    return View("~/Areas/BusinessLayer/Views/DocApproval/ApprovalDetail.cshtml", docApprovalModel);
                }
                else
                {
                    DocApprovalModel docApprovalModel1 = new DocApprovalModel();
                    //Session["TransType"] = "Save";
                    docApprovalModel1.TransType = "Save";

                    //Session["Command"] = "Add";
                    docApprovalModel1.Command = "Add";
                    //Session["AppStatus"] = 'D';
                    docApprovalModel1.AppStatus = "D";
                    if (docApprovalModel1.btnName == null)
                    {
                        docApprovalModel1.btnName = "AddNew";
                    }
                    //  Session["BtnName"] = "BtnAddNew";
                    //Session["edit"] = "";
                    string Doc_ID = "";
                    string Br_ID = "";
                    if (docApprovalM.EditCode == null)
                    {
                        docApprovalModel1.EditCode = docApprovalM.EditCode;
                    }
                    if (docApprovalModel1.EditCode == null)
                    {
                        docApprovalModel1.EditCode = "";
                    }

                    if (br_id == null)
                    {
                        //br_id = Session["EditCode"].ToString();
                        br_id = docApprovalM.EditCode;
                        //br_id = docApprovalModel1.EditCode.ToString();
                    }

                    if (br_id != null && br_id != "")
                    {

                        String str = br_id;
                        Doc_ID = str.Substring(str.IndexOf('-') + 1);

                        Br_ID = str.Substring(0, str.IndexOf("-"));
                        if (docApprovalModel1.EditCode != "")
                        //if (Session["EditCode"].ToString() != "")
                        {
                            if (docApprovalModel1.btnName == "Save")
                            //if (Session["BtnName"].ToString() == "Save")
                            {
                                //Session["BtnName"] = "Save";
                                docApprovalModel1.btnName = "Save";
                            }
                            else
                            {
                                //Session["BtnName"] = "Update";
                                docApprovalModel1.btnName = "Update";
                            }
                            //Session["TransType"] = "Update";
                            docApprovalModel1.TransType = "Update";

                        }
                        else
                        {
                            //Session["BtnName"] = "dblClick";
                            docApprovalModel1.btnName = "dblClick";
                            //Session["TransType"] = "Edit";
                            docApprovalModel1.TransType = "Edit";
                        }

                    }
                    if (docApprovalModel1.Message == null)
                    {
                        docApprovalModel1.Message = "New";
                        //docApprovalModel1.btnName= "AddNew";
                    }
                    if (docApprovalModel1.Message != null)
                    //if (Session["Message"] != null)
                    {
                        //string message = Session["Message"].ToString();
                        string message = docApprovalModel1.Message;
                        if (docApprovalModel1.Message == "Save")
                        //if (Session["Message"].ToString() == "Save")
                        {
                            //ViewBag.Message = "Save";
                            docApprovalModel1.Message = "Save";
                            //Session["TransType"] = "Edit";
                            docApprovalModel1.TransType = "Edit";
                        }
                        if (docApprovalModel1.Message == "Deleted")
                        //if (Session["Message"].ToString() == "Deleted")
                        {
                            //ViewBag.Message = "Deleted";
                            docApprovalModel1.Message = "Deleted";
                            //Session["TransType"] = "Edit";
                            docApprovalModel1.TransType = "Edit";
                        }

                    }
                    string CompID = "";
                    if (Session["CompID"] != null)
                    {
                        CompID = Session["CompID"].ToString();
                    }
                    //Session["Message"] = "New";
                    docApprovalModel1.Message = "New";

                    DataSet EditData = _docApproval_ISERVICES.getDocAppEditDetails(Br_ID, Doc_ID, CompID);
                    if (EditData.Tables[1].Rows.Count > 0)
                    {
                        //ViewBag.Data = EditData.Tables[1];
                        docApprovalModel1.Data = EditData.Tables[1];
                    }
                    if (EditData.Tables[0].Rows.Count > 0)
                    {
                        //ViewBag.createdt = EditData.Tables[0].Rows[0]["create_dt"].ToString();
                        //ViewBag.moddt = EditData.Tables[0].Rows[0]["mod_dt"].ToString();
                        //ViewBag.createid = EditData.Tables[0].Rows[0]["create_id"].ToString();
                        //ViewBag.modid = EditData.Tables[0].Rows[0]["mod_id"].ToString();
                        docApprovalModel1.createdt = EditData.Tables[0].Rows[0]["create_dt"].ToString();
                        docApprovalModel1.moddt = EditData.Tables[0].Rows[0]["mod_dt"].ToString();
                        docApprovalModel1.createid = EditData.Tables[0].Rows[0]["create_id"].ToString();
                        docApprovalModel1.modid = EditData.Tables[0].Rows[0]["mod_id"].ToString();
                        docApprovalModel1.Branch_id = EditData.Tables[0].Rows[0]["br_id"].ToString();
                        docApprovalModel1.Doc_id = EditData.Tables[0].Rows[0]["doc_id"].ToString();
                    }

                    DataSet DsBranch = getAppDocDetails();
                    List<Branch_list> branch_Lists = new List<Branch_list>();
                    foreach (DataRow dr in DsBranch.Tables[0].Rows)
                    {
                        Branch_list _brlist = new Branch_list();
                        _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                        _brlist.BranchName = dr["comp_nm"].ToString();
                        branch_Lists.Add(_brlist);
                    }
                    docApprovalModel1._branchList = branch_Lists;
                    List<Doc_list> _doclist = new List<Doc_list>();
                    foreach (DataRow dr in DsBranch.Tables[1].Rows)
                    {
                        Doc_list _doc_list = new Doc_list();
                        _doc_list.DocID = dr["doc_id"].ToString();
                        _doc_list.Document = dr["doc_name_eng"].ToString();
                        _doclist.Add(_doc_list);
                    }
                    docApprovalModel1._docList = _doclist;


                    // doclist(docApprovalModel);
                    CommonPageDetails();
                    ViewBag.MenuPageName = getDocumentName();
                    docApprovalModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/DocApproval/ApprovalDetail.cshtml", docApprovalModel1);
                }
                
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        
        public ActionResult ApprovalDetail()
        {
            try
            {
                if (Session["MenuPageName"] != null)
                {
                    ViewBag.MenuPageName = Session["MenuPageName"].ToString();
                }
                       
                return View("~/Areas/BusinessLayer/Views/DocApproval/ApprovalDetail.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }

}