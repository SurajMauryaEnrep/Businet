using System;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserSetup;
using EnRepMobileWeb.MODELS.SecurityLayer.UserSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using System.Configuration;
using System.Linq;

namespace EnRepMobileWeb.Areas.SecurityLayer.Controllers.UserSetup
{
    public class UserDetailController : Controller
    {
        string CompID, title, Br_ID, userid, language = String.Empty;
        string DocumentMenuId = "104101";
        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        //DataTable dt;
        DataTable UserRoleDetail = new DataTable();
        DataTable UserBranchAccDetail = new DataTable();
        UserDetail_ISERVICES _UserDetail_ISERVICES;
        // GET: SecurityLayer/UserDetail
        public UserDetailController(Common_IServices _Common_IServices, UserDetail_ISERVICES _UserDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._UserDetail_ISERVICES = _UserDetail_ISERVICES;
        }
        public ActionResult UserDetail(string UserId="0")
        {
            try
            {
               
                UserDetailModel userDetailModel = new UserDetailModel();
                DataSet lang = _UserDetail_ISERVICES.BindLAng();
                List<Lang_List> _LangList = new List<Lang_List>();
                foreach (DataRow dr in lang.Tables[0].Rows)
                {
                    Lang_List lang_List = new Lang_List();
                    lang_List.Lang_ID = dr["lang_id"].ToString();
                    lang_List.Lang_Name = dr["lang_name"].ToString();
                    _LangList.Add(lang_List);
                }
                userDetailModel.lang_Lists = _LangList;

                DataSet HOList = _UserDetail_ISERVICES.BindHeadOffice();
                List<HO_List> _HOList = new List<HO_List>();
                foreach (DataRow dr in HOList.Tables[0].Rows)
                {
                    HO_List hO_List = new HO_List();
                    hO_List.HO_ID = dr["Comp_Id"].ToString();
                    hO_List.HO_Name = dr["comp_nm"].ToString();
                    _HOList.Add(hO_List);
                }
                _HOList.Insert(0, new HO_List() { HO_ID = "0", HO_Name = "---Select---" });
                userDetailModel.hO_Lists = _HOList;



                DataSet BindReporting = _UserDetail_ISERVICES.BindReportingTo();
                List<ReportingTo_List> _ReportingTo_List = new List<ReportingTo_List>();
                foreach (DataRow dr in BindReporting.Tables[0].Rows)
                {
                    ReportingTo_List rpt_id = new ReportingTo_List();
                    rpt_id.ReportingTo_ID = dr["user_id"].ToString();
                    rpt_id.ReportingTo_Name = dr["user_nm"].ToString();
                    _ReportingTo_List.Add(rpt_id);
                }
                _ReportingTo_List.Insert(0, new ReportingTo_List() { ReportingTo_ID = "0", ReportingTo_Name = "---Select---" });
                userDetailModel.ReportingToLists = _ReportingTo_List;
                if (UserId != null && UserId != "0")
                {
                    Session["UserCode"] = UserId;
                    Session["TransType"] = "Update";
                    if(Session["Message"].ToString()== "PageLoad")
                    {
                        Session["BtnName"] = "BtnRefresh";
                        Session["Message"] = "PageLoad";
                    }
                    else
                    {
                        Session["BtnName"] = "BtnToDetailPage";
                        Session["Message"] = "New";
                    }
                   
                   
                    Session["Command"] = "";
                }
                if ((Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit" ) && Session["UserCode"] != null)
                {
                    string user_id = Session["UserCode"].ToString();
                    DataSet ds = _UserDetail_ISERVICES.getUserSetUpDt(user_id);
                   
                    userDetailModel.user_id = Convert.ToInt32(ds.Tables[0].Rows[0]["user_id"].ToString());
                    userDetailModel.ReportingToLists = userDetailModel.ReportingToLists.Where(x => x.ReportingTo_ID != user_id).ToList();
                    userDetailModel.gender = ds.Tables[0].Rows[0]["user_salute"].ToString();
                    userDetailModel.user_nm = ds.Tables[0].Rows[0]["user_nm"].ToString();
                    userDetailModel.nick_nm = ds.Tables[0].Rows[0]["nick_nm"].ToString();
                    userDetailModel.user_pwd = Decrypt(ds.Tables[0].Rows[0]["user_pwd"].ToString());
                    userDetailModel.def_lang = ds.Tables[0].Rows[0]["def_lang"].ToString();
                    userDetailModel.user_email = ds.Tables[0].Rows[0]["user_email"].ToString();
                    userDetailModel.user_cont = ds.Tables[0].Rows[0]["user_cont"].ToString();
                    userDetailModel.DOB = ds.Tables[0].Rows[0]["dob"].ToString();

                    userDetailModel.host_server = ds.Tables[0].Rows[0]["host_server"].ToString();
                    userDetailModel.LogInID = ds.Tables[0].Rows[0]["login_id"].ToString();
                    userDetailModel.Designation = ds.Tables[0].Rows[0]["designation"].ToString();
                    //userDetailModel.port = Convert.ToInt32(ds.Tables[0].Rows[0]["port"].ToString());
                    if (ds.Tables[0].Rows[0]["port"] != DBNull.Value && !string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["port"].ToString()))
                    {
                        userDetailModel.port = Convert.ToInt32(ds.Tables[0].Rows[0]["port"]);
                    }
                    else
                    {
                        userDetailModel.port = 0; // or any default value
                    }

                    userDetailModel.sender_email = ds.Tables[0].Rows[0]["sender_email"].ToString();
                    if (userDetailModel.user_id==1001)
                    {
                        userDetailModel.mail_pwd = ds.Tables[0].Rows[0]["mail_pwd"].ToString();
                    }
                    else
                    {
                        userDetailModel.mail_pwd = Decrypt(ds.Tables[0].Rows[0]["mail_pwd"].ToString());
                    }
                    userDetailModel.ssl_flag = ds.Tables[0].Rows[0]["ssl_flag"].ToString();
                    userDetailModel.ReportingTo = ds.Tables[0].Rows[0]["rpt_id"].ToString();

                    string act = ds.Tables[0].Rows[0]["act"].ToString();
                    if (act == "Y")
                    {
                        userDetailModel.act = true;
                    }
                    else
                    {
                        userDetailModel.act = false;
                    }
                    string crm = ds.Tables[0].Rows[0]["crm"].ToString();
                    if (crm == "Y")
                    {
                        userDetailModel.CRMUser = true;
                    }
                    else
                    {
                        userDetailModel.CRMUser = false;
                    }
                    if (ds.Tables[0].Rows[0]["user_avl"].ToString() == "Y") //Added this condtion and toggle By Nitesh 20-10-2023  for view data in page
                    {
                        userDetailModel.Available = true;
                    }
                    else
                    {
                        userDetailModel.Available = false;
                    }
                    string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                    string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                    if (Request.Url.Host == localIp)
                        serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                    else
                        serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();

                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["digi_sign"].ToString()))
                    {
                        userDetailModel.digi_sign = ds.Tables[0].Rows[0]["digi_sign"].ToString();
                        userDetailModel.attachmentdetails = null;
                    }
                    else
                    {
                       
                        userDetailModel.attachmentdetails = serverUrl + ds.Tables[0].Rows[0]["digi_sign"].ToString();
                        userDetailModel.hdnAttachment = ds.Tables[0].Rows[0]["digi_sign"].ToString();
                    } 
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["user_img"].ToString()))
                    {
                        userDetailModel.UserImage = ds.Tables[0].Rows[0]["user_img"].ToString();
                        userDetailModel.UserImage_attachmentdetails = null;
                    }
                    else
                    {               
                        userDetailModel.UserImage_attachmentdetails = serverUrl + ds.Tables[0].Rows[0]["user_img"].ToString();
                        userDetailModel.hdnAttachment_UserImg = ds.Tables[0].Rows[0]["user_img"].ToString();
                    }
                    userDetailModel.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                    userDetailModel.Createdon = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    userDetailModel.AmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                    userDetailModel.AmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    ViewBag.RoleDetails = ds.Tables[1];
                    ViewBag.BranchDetails = ds.Tables[2];


                }
                if (Session["Command"].ToString() == "Add" && Session["Message"].ToString()==""  )
                {
                    userDetailModel.Available = true;
                }
                ViewBag.MenuPageName = getDocumentName();
                userDetailModel.Title = title;
                CommonPageDetails();
                return View("~/Areas/SecurityLayer/Views/UserSetup/UserDetail.cshtml", userDetailModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult GetRoleName(string HO_ID)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet RoleName = _UserDetail_ISERVICES.GetRoleName(HO_ID);
                DataRows = Json(JsonConvert.SerializeObject(RoleName));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult GetBranchName(string HO_ID)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet BranchName = _UserDetail_ISERVICES.GetBranchName(HO_ID);
                DataRows = Json(JsonConvert.SerializeObject(BranchName));
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult UserSave(UserDetailModel _UserDetail, string command, HttpPostedFileBase digi_sign, HttpPostedFileBase UserImage)
        {
            try
            {
                //if (_UserDetail.hdnAction=="Delete")
                //{
                //    command = "Delete";
                //}
                switch (command)
                {
                    case "Edit":
                        Session["Message"] = "";
                        Session["Command"] = command;
                        Session["UserCode"] = _UserDetail.user_id;
                        Session["TransType"] = "Update";
                        Session["BtnName"] = "BtnEdit";
                        return RedirectToAction("UserDetail");

                    case "Add":
                        Session["Message"] = "";
                        Session["Command"] = command;
                        Session["UserCode"] = "";

                        _UserDetail = null;
                        Session["TransType"] = "Save";
                        Session["BtnName"] = "BtnAddNew";
                        return RedirectToAction("UserDetail");

                    case "Save":
                        Session["Command"] = command;
                        if (ModelState.IsValid)
                        {
                            if (digi_sign != null)
                            {
                                Guid guid = Guid.NewGuid();
                                string imagePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "DigitalSignature");

                                if (!Directory.Exists(imagePath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(imagePath);
                                }
                                string fileName = guid.ToString().Replace("-", "") + Path.GetExtension(digi_sign.FileName);
                                string finalPath = "/DigitalSignature/" + fileName;
                                _UserDetail.attachmentdetails = finalPath;
                                digi_sign.SaveAs(imagePath + "\\" + fileName);
                            }
                            else
                            {
                                _UserDetail.attachmentdetails = _UserDetail.hdnAttachment;
                            }
                            if (UserImage != null)
                            {
                                if (Session["CompId"] != null)
                                {
                                    CompID = Session["CompId"].ToString();
                                }
                                if (Session["BranchId"] != null)
                                {
                                    Br_ID = Session["BranchId"].ToString();
                                }
                                Guid guid = Guid.NewGuid();
                                string imagePath1 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "UserSetup");

                                if (!Directory.Exists(imagePath1))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(imagePath1);
                                }
                                Random rnd = new Random();
                                int num = rnd.Next(100000, 999999);                              
                                string fileName1 = _UserDetail.user_nm + CompID + Br_ID + num + Path.GetExtension(UserImage.FileName);

                                string finalPath1= "/UserSetup/" + fileName1;
                                _UserDetail.UserImage_attachmentdetails = finalPath1;
                                UserImage.SaveAs(imagePath1 + "\\" + fileName1);
                            }
                            else
                            {
                                _UserDetail.UserImage_attachmentdetails = _UserDetail.hdnAttachment_UserImg;
                            }
                            InsertUserSetupDetail(_UserDetail, command);
                            if (Session["Message"].ToString() == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            if (Session["Message"].ToString() == "Duplicate" || Session["Message"].ToString() == "DuplicateID")
                            {
                                Session["UserCode"] = "";
                                Session["TransType"] = "Save";
                                Session["BtnName"] = "BtnAddNew";
                                Session["Command"] = "Add";
                                ViewBag.Message = Session["Message"].ToString();
                                DataSet lang = _UserDetail_ISERVICES.BindLAng();
                                List<Lang_List> _LangList = new List<Lang_List>();
                                foreach (DataRow dr in lang.Tables[0].Rows)
                                {
                                    Lang_List lang_List = new Lang_List();
                                    lang_List.Lang_ID = dr["lang_id"].ToString();
                                    lang_List.Lang_Name = dr["lang_name"].ToString();
                                    _LangList.Add(lang_List);
                                }
                                _UserDetail.lang_Lists = _LangList;

                                DataSet HOList = _UserDetail_ISERVICES.BindHeadOffice();
                                List<HO_List> _HOList = new List<HO_List>();
                                foreach (DataRow dr in HOList.Tables[0].Rows)
                                {
                                    HO_List hO_List = new HO_List();
                                    hO_List.HO_ID = dr["Comp_Id"].ToString();
                                    hO_List.HO_Name = dr["comp_nm"].ToString();
                                    _HOList.Add(hO_List);
                                }
                                _HOList.Insert(0, new HO_List() { HO_ID = "0", HO_Name = "---Select---" });
                                _UserDetail.hO_Lists = _HOList;
                                DataSet BindReporting = _UserDetail_ISERVICES.BindReportingTo();
                                List<ReportingTo_List> _ReportingTo_List = new List<ReportingTo_List>();
                                foreach (DataRow dr in BindReporting.Tables[0].Rows)
                                {
                                    ReportingTo_List rpt_id = new ReportingTo_List();
                                    rpt_id.ReportingTo_ID = dr["user_id"].ToString();
                                    rpt_id.ReportingTo_Name = dr["user_nm"].ToString();
                                    _ReportingTo_List.Add(rpt_id);
                                }
                                _ReportingTo_List.Insert(0, new ReportingTo_List() { ReportingTo_ID = "0", ReportingTo_Name = "---Select---" });
                                _UserDetail.ReportingToLists = _ReportingTo_List;
                                ViewBag.RoleDetails = UserRoleDetail;//ds.Tables[1];
                                ViewBag.BranchDetails = UserBranchAccDetail;// ds.Tables[2];
                                CommonPageDetails();
                                _UserDetail.hdnsaveApprovebtn = null;
                                _UserDetail.UserImage_attachmentdetails = null;
                                _UserDetail.attachmentdetails = null;
                                _UserDetail.Title = title;
                                return View("~/Areas/SecurityLayer/Views/UserSetup/UserDetail.cshtml", _UserDetail);
                            }
                            else
                            {
                                UserRoleDetail = null;
                                UserBranchAccDetail = null;
                                Session["BtnName"] = "BtnToDetailPage";
                                return RedirectToAction("UserDetail");
                            }

                        }
                        else
                        {
                            _UserDetail = null;
                            CommonPageDetails();
                            return View("~/Areas/SecurityLayer/Views/UserSetup/UserDetail.cshtml", _UserDetail);
                        }
                    case "Update":
                        Session["Command"] = command;
                        if (digi_sign != null)
                        {
                            Guid guid = Guid.NewGuid();
                            string imagePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "DigitalSignature");
                          
                            if (!Directory.Exists(imagePath))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(imagePath);
                            }
                            string fileName = guid.ToString().Replace("-", "") + Path.GetExtension(digi_sign.FileName);
                            string finalPath = "/DigitalSignature/" + fileName;
                            _UserDetail.attachmentdetails = finalPath;
                            digi_sign.SaveAs(imagePath + "\\" + fileName);
                        }
                        else
                        {
                            _UserDetail.attachmentdetails = _UserDetail.hdnAttachment;
                        }
                        if (UserImage != null)
                        {
                            if (Session["CompId"] != null)
                            {
                                CompID = Session["CompId"].ToString();
                            }
                            if (Session["BranchId"] != null)
                            {
                                Br_ID = Session["BranchId"].ToString();
                            }
                            Guid guid = Guid.NewGuid();
                            string imagePath1 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "UserSetup");

                            if (!Directory.Exists(imagePath1))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(imagePath1);
                            }
                            Random rnd = new Random();
                            int num = rnd.Next(100000, 999999);
                            //string fileName = guid.ToString().Replace("-", "") + Path.GetExtension(UserImage.FileName);
                            string fileName1 =  _UserDetail.user_nm + CompID + Br_ID + num+ Path.GetExtension(UserImage.FileName);
                          
                            string finalPath1 = "/UserSetup/" + fileName1;
                            _UserDetail.UserImage_attachmentdetails = finalPath1;
                            UserImage.SaveAs(imagePath1 + "\\" + fileName1);
                        }
                        else
                        {
                            _UserDetail.UserImage_attachmentdetails = _UserDetail.hdnAttachment_UserImg;
                        }
                        InsertUserSetupDetail(_UserDetail, command);
                        if (Session["Message"].ToString() == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (Session["Message"].ToString() == "Duplicate" || Session["Message"].ToString() == "DuplicateID")
                        {
                            Session["UserCode"] = "";
                            ViewBag.Message = Session["Message"].ToString();
                            DataSet lang = _UserDetail_ISERVICES.BindLAng();
                            List<Lang_List> _LangList = new List<Lang_List>();
                            foreach (DataRow dr in lang.Tables[0].Rows)
                            {
                                Lang_List lang_List = new Lang_List();
                                lang_List.Lang_ID = dr["lang_id"].ToString();
                                lang_List.Lang_Name = dr["lang_name"].ToString();
                                _LangList.Add(lang_List);
                            }
                            _UserDetail.lang_Lists = _LangList;
                            DataSet HOList = _UserDetail_ISERVICES.BindHeadOffice();
                            List<HO_List> _HOList = new List<HO_List>();
                            foreach (DataRow dr in HOList.Tables[0].Rows)
                            {
                                HO_List hO_List = new HO_List();
                                hO_List.HO_ID = dr["Comp_Id"].ToString();
                                hO_List.HO_Name = dr["comp_nm"].ToString();
                                _HOList.Add(hO_List);
                            }
                            _HOList.Insert(0, new HO_List() { HO_ID = "0", HO_Name = "---Select---" });
                            _UserDetail.hO_Lists = _HOList;
                            DataSet BindReporting = _UserDetail_ISERVICES.BindReportingTo();
                            List<ReportingTo_List> _ReportingTo_List = new List<ReportingTo_List>();
                            foreach (DataRow dr in BindReporting.Tables[0].Rows)
                            {
                                ReportingTo_List rpt_id = new ReportingTo_List();
                                rpt_id.ReportingTo_ID = dr["user_id"].ToString();
                                rpt_id.ReportingTo_Name = dr["user_nm"].ToString();
                                _ReportingTo_List.Add(rpt_id);
                            }
                            _ReportingTo_List.Insert(0, new ReportingTo_List() { ReportingTo_ID = "0", ReportingTo_Name = "---Select---" });
                            _UserDetail.ReportingToLists = _ReportingTo_List;
                            ViewBag.RoleDetails = UserRoleDetail;//ds.Tables[1];
                            ViewBag.BranchDetails = UserBranchAccDetail;// ds.Tables[2];
                            CommonPageDetails();
                            _UserDetail.hdnAttachment_UserImg = _UserDetail.UserImage_attachmentdetails;
                            _UserDetail.Title = title;
                            return View("~/Areas/SecurityLayer/Views/UserSetup/UserDetail.cshtml", _UserDetail);
                        }
                        else
                        {
                            Session["BtnName"] = "BtnToDetailPage";
                            return RedirectToAction("UserDetail");
                        }
                    case "Refresh":
                        Session["BtnName"] = "BtnRefresh";
                        Session["Command"] = command;
                        Session["TransType"] = "Refresh";
                        Session["Message"] = "";
                        _UserDetail = null;
                        return RedirectToAction("UserDetail");

                    case "BacktoList":
                        Session.Remove("Message");
                        Session.Remove("TransType");
                        Session.Remove("Command");
                        Session.Remove("BtnName");
                        Session.Remove("DocumentStatus");
                        return RedirectToAction("UserList", "UserList");

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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, userid, DocumentMenuId, language);
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
        //public ActionResult UserDelete(UserDetailModel _userDetail)
        //{
        //    string Result = _UserDetail_ISERVICES.DeleteUserSetup(_userDetail.user_id.ToString());
        //    string User_id = Result.Substring(Result.IndexOf('-') + 1);
        //    string Message = Result.Substring(0, Result.IndexOf("-"));
        //    if (Message == "Deleted")
        //    {
        //        Session["Message"] = "Deleted";
        //    }
        //    if (User_id == "1001")
        //    {
        //        Session["Message"] = "Reserved";
        //    }
        //    return View();//RedirectToAction("UserDetail");
        //}
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212012";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            
            if (!string.IsNullOrEmpty(cipherText))
            {
                string EncryptionKey = "MAKV2SPBNI99212012";
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            
            return cipherText;
        }
        public ActionResult InsertUserSetupDetail(UserDetailModel _UserDetail, string command)
        {
            try
            {
                string userid = "";
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable Userdetail = new DataTable();
                DataTable UdtHeader = new DataTable();
                UdtHeader.Columns.Add("TransType", typeof(string));
                UdtHeader.Columns.Add("user_id", typeof(int));
                UdtHeader.Columns.Add("user_nm", typeof(string));
                UdtHeader.Columns.Add("nick_nm", typeof(string));
                UdtHeader.Columns.Add("user_pwd", typeof(string));
                UdtHeader.Columns.Add("def_lang", typeof(string));
                UdtHeader.Columns.Add("user_email", typeof(string));
                UdtHeader.Columns.Add("user_cont", typeof(string));
                UdtHeader.Columns.Add("act", typeof(string));
                UdtHeader.Columns.Add("dob", typeof(string));
                UdtHeader.Columns.Add("create_id", typeof(int));
                UdtHeader.Columns.Add("user_salute", typeof(string));
                UdtHeader.Columns.Add("Available", typeof(string));
                UdtHeader.Columns.Add("digi_sign", typeof(string));
                UdtHeader.Columns.Add("user_img", typeof(string));

                UdtHeader.Columns.Add("host_server", typeof(string));
                UdtHeader.Columns.Add("port", typeof(string));
                UdtHeader.Columns.Add("sender_email", typeof(string));
                UdtHeader.Columns.Add("mail_pwd", typeof(string));
                UdtHeader.Columns.Add("ssl_flag", typeof(string));
                UdtHeader.Columns.Add("use_deflt_cred", typeof(string));
                UdtHeader.Columns.Add("rpt_id", typeof(string));
                UdtHeader.Columns.Add("crm", typeof(string));
                UdtHeader.Columns.Add("login_id", typeof(string));
                UdtHeader.Columns.Add("designation", typeof(string));

                DataRow UdtRowHeader = UdtHeader.NewRow();
                UdtRowHeader["TransType"] = command;
                UdtRowHeader["user_id"] = _UserDetail.user_id;
                UdtRowHeader["user_nm"] = _UserDetail.user_nm;
                UdtRowHeader["nick_nm"] = _UserDetail.nick_nm;
                UdtRowHeader["user_pwd"] = Encrypt(_UserDetail.user_pwd);
                UdtRowHeader["def_lang"] = _UserDetail.def_lang;
                UdtRowHeader["user_email"] = _UserDetail.user_email;
                UdtRowHeader["user_cont"] = _UserDetail.user_cont;

                UdtRowHeader["host_server"] = _UserDetail.host_server;
                UdtRowHeader["port"] = _UserDetail.port;
                if(_UserDetail.mail_pwd != null)
                {
                    UdtRowHeader["mail_pwd"] = Encrypt(_UserDetail.mail_pwd);
                }
                UdtRowHeader["sender_email"] = _UserDetail.sender_email;
                if(_UserDetail.ssl_flag == "Y")
                {
                    UdtRowHeader["ssl_flag"] = "Y";
                }
                else
                {
                    UdtRowHeader["ssl_flag"] = "N";
                }
                if (_UserDetail.use_deflt_cred == "Y")
                {
                    UdtRowHeader["use_deflt_cred"] = "Y";
                }
                else
                {
                    UdtRowHeader["use_deflt_cred"] = "N";
                }

                if (_UserDetail.act)
                {
                    UdtRowHeader["act"] = "Y";
                }
                else
                {
                    UdtRowHeader["act"] = "N";
                }
                UdtRowHeader["dob"] = _UserDetail.DOB;
                UdtRowHeader["create_id"] = userid;
                UdtRowHeader["user_salute"] = _UserDetail.gender;
                if (_UserDetail.Available == true)
                {
                    UdtRowHeader["Available"] = "Y";
                }
                else
                {
                    UdtRowHeader["Available"] = "N";
                }
                UdtRowHeader["digi_sign"] = _UserDetail.attachmentdetails;
                UdtRowHeader["user_img"] = _UserDetail.UserImage_attachmentdetails;
                UdtRowHeader["rpt_id"] = _UserDetail.ReportingTo;
              
                if (_UserDetail.CRMUser == true)
                {
                    UdtRowHeader["crm"] = "Y";
                }
                else
                {
                    UdtRowHeader["crm"] = "N";
                }
                UdtRowHeader["login_id"] = _UserDetail.LogInID;
                UdtRowHeader["designation"] = _UserDetail.Designation;
                UdtHeader.Rows.Add(UdtRowHeader);
                Userdetail = UdtHeader;

                DataTable RoleDTHeader = new DataTable();

                RoleDTHeader.Columns.Add("comp_id", typeof(int));
                RoleDTHeader.Columns.Add("role_id", typeof(int));

                JArray jObject = JArray.Parse(_UserDetail.RoleDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow RoleRowDt = RoleDTHeader.NewRow();

                    RoleRowDt["comp_id"] = jObject[i]["HO_id"].ToString();
                    RoleRowDt["role_id"] = jObject[i]["Role_id"].ToString();

                    RoleDTHeader.Rows.Add(RoleRowDt);
                }
                UserRoleDetail = RoleDTHeader;

                DataTable BranchDTHeader = new DataTable();
                BranchDTHeader.Columns.Add("comp_id", typeof(int));
                BranchDTHeader.Columns.Add("br_id", typeof(int));
                BranchDTHeader.Columns.Add("def_br", typeof(string));

                JArray JBObject = JArray.Parse(_UserDetail.BranchDetail);
                for (int i = 0; i < JBObject.Count; i++)
                {
                    DataRow BranchDt = BranchDTHeader.NewRow();
                    BranchDt["comp_id"] = JBObject[i]["HO_id"].ToString();
                    BranchDt["br_id"] = JBObject[i]["BR_id"].ToString();
                    BranchDt["def_br"] = JBObject[i]["Status"].ToString();
                    BranchDTHeader.Rows.Add(BranchDt);

                }
                UserBranchAccDetail = BranchDTHeader;

                string Result = _UserDetail_ISERVICES.InsertUpdateUserSetup(Userdetail, UserRoleDetail, UserBranchAccDetail);
                string User_id = Result.Substring(Result.IndexOf('-') + 1);
                string Message = Result.Substring(0, Result.IndexOf("-"));
                if (Message == "DataNotFound")
                {
                    var a = User_id.Split('-');
                    var msg = "Data Not found" + a[0].Trim();
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    Session["Message"] = Message;
                    return RedirectToAction("UserDetail");
                }
                if (Message == "Save" || Message == "Update")
                {
                    Session["Message"] = "Save";
                    Session["UserCode"] = User_id;
                    Session["TransType"] = "Update";
                   // _UserDetail.hdnAttachment_UserImg = _UserDetail.UserImage_attachmentdetails.ToString();
                }
                if (Message == "Duplicate" || Message == "DuplicateID")
                {
                    //_UserDetail.hdnAttachment_UserImg = _UserDetail.UserImage_attachmentdetails.ToString();
                    UserRoleDetail.Rows.Clear();
                    RoleDTHeader.Columns.Add("comp_nm", typeof(string));
                    RoleDTHeader.Columns.Add("role_name", typeof(string));
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow RoleRowDt = RoleDTHeader.NewRow();
                        RoleRowDt["comp_id"] = jObject[i]["HO_id"].ToString();
                        RoleRowDt["role_id"] = jObject[i]["Role_id"].ToString();
                        RoleRowDt["comp_nm"] = jObject[i]["HO_nm"].ToString();
                        RoleRowDt["role_name"] = jObject[i]["Role_nm"].ToString();

                        RoleDTHeader.Rows.Add(RoleRowDt);
                    }
                    UserRoleDetail = RoleDTHeader;


                    BranchDTHeader.Columns.Add("branch_nm", typeof(string));
                    BranchDTHeader.Columns.Add("comp_nm", typeof(string));
                    UserBranchAccDetail.Rows.Clear();
                    for (int i = 0; i < JBObject.Count; i++)
                    {
                        DataRow BranchDt = BranchDTHeader.NewRow();
                        BranchDt["comp_id"] = JBObject[i]["HO_id"].ToString();
                        BranchDt["br_id"] = JBObject[i]["BR_id"].ToString();
                        BranchDt["def_br"] = JBObject[i]["Status"].ToString();
                        BranchDt["branch_nm"] = JBObject[i]["BR_nm"].ToString();
                        BranchDt["comp_nm"] = JBObject[i]["HO_nm"].ToString();
                        BranchDTHeader.Rows.Add(BranchDt);
                    }
                    UserBranchAccDetail = BranchDTHeader;


                    //Session["UserCode"] = User_id;
                    Session["Message"] = Message;
                }
                return RedirectToAction("UserDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public JsonResult GetUserDeatilUserstree(ItemMenuSearchModel ObjItemMenuSearchModel,string UserID)
        {
            try
            {
                if (Session["CompId"] != null)
                    ObjItemMenuSearchModel.Comp_ID = Session["CompId"].ToString();

                // Get all users in a DataTable
                DataSet dtUsers = _UserDetail_ISERVICES.GetUserTree(ObjItemMenuSearchModel);

                // Build hierarchy starting from root (rpt_id = 0)
                string parentId = string.IsNullOrEmpty(UserID) ? "0" : UserID;
                List<childrenNode> tree = BuildTree(dtUsers.Tables[0], parentId);

                // Return list (automatically converts to JSON array)
                return Json(tree, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw;
            }
        }

        private List<childrenNode> BuildTree(DataTable allUsers, string parentId)
        {
            List<childrenNode> nodes = new List<childrenNode>();
            DataRow[] childRows = allUsers.Select("rpt_id = '" + parentId + "'");

            foreach (DataRow child in childRows)
            {
                string status = child["user_avl"].ToString().Trim(); // Availability (Y/N)
                string activeStatus = child["act"].ToString().Trim(); // Active (Y/N)
                string color = "black"; // default color

                // 🟠 Available but not inactive
                if (status.Equals("N", StringComparison.OrdinalIgnoreCase))
                    color = "orange";

                // 🔴 Not active always overrides
                if (activeStatus.Equals("N", StringComparison.OrdinalIgnoreCase))
                    color = "red";

                childrenNode node = new childrenNode
                {

                    label = child["user_nm"].ToString(),
                    value = child["user_id"].ToString(),
                    color = color,
                    children = BuildTree(allUsers, child["user_id"].ToString())
                };

                nodes.Add(node);
            }

            return nodes;
        }








    }
}