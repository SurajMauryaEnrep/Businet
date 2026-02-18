using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserRoleSetup;
using EnRepMobileWeb.MODELS.SecurityLayer.UserRoleSetup;
using System;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EnRepMobileWeb.Areas.SecurityLayer.Controllers.UserRoleSetup
{
    public class UserRoleSetupController : Controller
    {
        string CompID, Br_ID,userid, language = String.Empty;
        string DocumentMenuId = "104105", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        UserRoleSetup_ISERVICE _UserRoleSetup_ISERVICE;
        public UserRoleSetupController(Common_IServices _Common_IServices, UserRoleSetup_ISERVICE _UserRoleSetup_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._UserRoleSetup_ISERVICE = _UserRoleSetup_ISERVICE;
        }
        // GET: SecurityLayer/UserRoleSetup
        public ActionResult UserRoleSetupList()
        {
            UserRoleSetup_Model _UserRoleSetup_Model = new UserRoleSetup_Model();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            DataTable UserRoleList = _UserRoleSetup_ISERVICE.GetUserRoleList_Detail(CompID);
            List<UserRoleSetupList> __UserRoleSetupList = new List<UserRoleSetupList>();

            if (UserRoleList.Rows.Count > 0)
            {
                foreach (DataRow dr in UserRoleList.Rows)
                {
                    UserRoleSetupList _URSList = new UserRoleSetupList();
                    _URSList.RoleName = dr["role_name"].ToString();
                    _URSList.HO = dr["comp_nm"].ToString();
                    _URSList.HO_ID = dr["comp_id"].ToString();
                    _URSList.Role_ID = dr["role_id"].ToString();
                    _URSList.Createdon = dr["create_dt"].ToString();
                    _URSList.AmendedOn = dr["mod_dt"].ToString();
                    __UserRoleSetupList.Add(_URSList);
                }
                _UserRoleSetup_Model.UserRoleSetup_List = __UserRoleSetupList;
            }

            ViewBag.MenuPageName = getDocumentName();
            CommonPageDetails();
            _UserRoleSetup_Model.Title = title;
            return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupList.cshtml", _UserRoleSetup_Model);
        }
        public ActionResult AddUserRoleSetupDetail()
        {
            Session["URS_Message"] = "New";
            Session["URS_Command"] = "AddNewUserRoleSetup";
            //Session["URS_AppStatus"] = 'D';
            Session["URS_TransType"] = "Save";
            Session["URS_BtnName"] = "BtnAddNew";

            ViewBag.MenuPageName = getDocumentName();
            //return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupDetail.cshtml");
            return RedirectToAction("UserRoleSetupDetail");
        }
        public ActionResult UserRoleSetupDetail()
        {
            try
            {
                UserRoleSetup_Model _UserRoleSetup_Model = new UserRoleSetup_Model();
                GetAllMenuList();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                DataTable DT_HoList = _UserRoleSetup_ISERVICE.GetHoDetail();
                List<HOName> _HoList = new List<HOName>();
                if (DT_HoList.Rows.Count > 0)
                {
                    foreach (DataRow dr in DT_HoList.Rows)
                    {
                        HOName _hoName = new HOName();
                        _hoName.Comp_name = dr["comp_nm"].ToString();
                        _hoName.Comp_id = dr["Comp_id"].ToString();
                        _HoList.Add(_hoName);
                    }
                    _UserRoleSetup_Model.HoList = _HoList;
                }
                ViewBag.MenuPageName = getDocumentName();
                _UserRoleSetup_Model.Title = title;
                _UserRoleSetup_Model.HOName = CompID;

                if (Session["URS_TransType"] != null)
                {
                    if (Session["URS_TransType"].ToString() == "Update")
                    {
                        string UserRole_No = Session["URS_No"].ToString();

                        DataSet ds = _UserRoleSetup_ISERVICE.GetUserRoleDetail(CompID, UserRole_No);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _UserRoleSetup_Model.RoleName = ds.Tables[0].Rows[0]["role_name"].ToString();
                            _UserRoleSetup_Model.RoleID = ds.Tables[0].Rows[0]["role_id"].ToString();
                            _UserRoleSetup_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _UserRoleSetup_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            _UserRoleSetup_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _UserRoleSetup_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                            ViewBag.UserRoleList = ds.Tables[0];
                        }
                        ViewBag.UserList = ds.Tables[1];
                        CommonPageDetails();
                        return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupDetail.cshtml", _UserRoleSetup_Model);
                    }
                    else
                    {
                        CommonPageDetails();
                        return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupDetail.cshtml", _UserRoleSetup_Model);
                    }
                }
                else
                {
                    CommonPageDetails();
                    return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupDetail.cshtml", _UserRoleSetup_Model);
                }
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
        private DataSet GetAllMenuList()
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
                DataSet MenuListControll = _UserRoleSetup_ISERVICE.GetMenuList_UserRoleSetup(CompID);

                Session["RoleMenu1"] = MenuListControll.Tables[0];
                Session["RoleMenu2"] = MenuListControll.Tables[1];
                Session["RoleMenu3"] = MenuListControll.Tables[2];
                Session["RoleMenu4"] = MenuListControll.Tables[3];

                return MenuListControll;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserRoleSetupSave(UserRoleSetup_Model _UserRoleSetup_Model, string command)
        {
            try
            {
                if (_UserRoleSetup_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNewUserRoleSetup":
                        Session["URS_Message"] = "New";
                        //Session["URS_AppStatus"] = 'D';
                        Session["URS_BtnName"] = "BtnAddNew";
                        Session["URS_TransType"] = "Save";
                        Session["URS_Command"] = "AddNewUserRoleSetup";
                        return RedirectToAction("UserRoleSetupDetail");

                    case "Edit":
                        Session["URS_TransType"] = "Update";
                        Session["URS_Command"] = command;
                        Session["URS_Message"] = null;
                        Session["URS_BtnName"] = "BtnEdit";
                        //Session["URS_No"] = _UserRoleSetup_Model.;
                        return RedirectToAction("UserRoleSetupDetail");

                    case "Delete":
                        Session["URS_Command"] = command;
                        //Session["URS_BtnName"] = "Refresh";
                        //Dn_no = _UserRoleSetup_Model.dn_no;
                        UserRoleSetup_Delete(_UserRoleSetup_Model, command);
                        return RedirectToAction("UserRoleSetupDetail");

                    case "Save":
                        Session["URS_Command"] = command;
                        Save_UserRoleSetupDetails(_UserRoleSetup_Model);
                        Session["URS_No"] = Session["URS_No"].ToString();

                        if(Session["URS_Message"].ToString()== "Duplicate")
                        {
                            GetAllMenuList();
                            if (Session["CompId"] != null)
                            {
                                CompID = Session["CompId"].ToString();
                            }

                            DataTable DT_HoList = _UserRoleSetup_ISERVICE.GetHoDetail();
                            List<HOName> _HoList = new List<HOName>();
                            if (DT_HoList.Rows.Count > 0)
                            {
                                foreach (DataRow dr in DT_HoList.Rows)
                                {
                                    HOName _hoName = new HOName();
                                    _hoName.Comp_name = dr["comp_nm"].ToString();
                                    _hoName.Comp_id = dr["Comp_id"].ToString();
                                    _HoList.Add(_hoName);
                                }
                                _UserRoleSetup_Model.HoList = _HoList;
                            }
                            ViewBag.MenuPageName = getDocumentName();
                            _UserRoleSetup_Model.Title = title;
                            _UserRoleSetup_Model.HOName = CompID;
                            DataTable DTbl = new DataTable();
                            DTbl = (DataTable)Session["URS_Tbl"];
                            ViewBag.UserRoleList = DTbl;
                            Session.Remove("URS_Tbl");
                            ViewBag.URSMessage = "Duplicate";

                            Session["URS_TransType"] = "Save";
                            Session["URS_Command"] = "Edit";
                            Session["URS_Message"] = "Duplicate";
                            CommonPageDetails();
                            return View("~/Areas/SecurityLayer/Views/UserRoleSetup/UserRoleSetupDetail.cshtml", _UserRoleSetup_Model);
                        }
                        else
                        {
                            return RedirectToAction("UserRoleSetupDetail");
                        }
                        

                    case "Refresh":
                        Session["URS_BtnName"] = "BtnRefresh";
                        Session["URS_Command"] = command;
                        Session["URS_TransType"] = "Save";
                        Session["URS_Message"] = null;
                        //Session["URS_DocumentStatus"] = null;
                        return RedirectToAction("UserRoleSetupDetail");

                    case "Print":
                        return new EmptyResult();

                    case "BacktoList":
                        Session.Remove("URS_Message");// = null;
                        Session.Remove("URS_TransType");
                        Session.Remove("URS_Command");
                        Session.Remove("URS_BtnName");
                        //Session.Remove("URS_DocumentStatus");
                        return RedirectToAction("UserRoleSetupList");

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
        private ActionResult UserRoleSetup_Delete(UserRoleSetup_Model _UserRoleSetup_Model, string command)
        {
            try
            {
                string comp_id = string.Empty;
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                string Message = _UserRoleSetup_ISERVICE.Delete_UserRoleDetail(comp_id, _UserRoleSetup_Model.RoleID);

                if(Message== "Exists")
                {
                    Session["URS_Command"] = "Update";
                    Session["URS_Message"] = "Exists";
                    Session["URS_TransType"] = "Update";
                    Session["URS_BtnName"] = "BtnSave";
                }
                if (Message == "Deleted")
                {
                    Session["URS_Command"] = "Refresh";
                    Session["URS_Message"] = "Deleted";
                    Session["URS_BtnName"] = "BtnDelete";
                    Session["URS_TransType"] = command;
                }
                
                //Session["URS_Command"] = "Refresh";
                Session["URS_No"] = _UserRoleSetup_Model.RoleID;
                //Session["URS_TransType"] = command;
                //Session["URS_AppStatus"] = 'D';
                //Session["URS_BtnName"] = "BtnDelete";
                return RedirectToAction("UserRoleSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult Save_UserRoleSetupDetails(UserRoleSetup_Model _UserRoleSetup_Model)
        {
            try
            {
                string comp_id = string.Empty;
                string userid = string.Empty;
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }

                DataTable UserRoleSetupDetails = new DataTable();

                DataTable URS_List = new DataTable();
                URS_List.Columns.Add("TransType", typeof(string));
                URS_List.Columns.Add("ho_id", typeof(Int32));
                URS_List.Columns.Add("role_name", typeof(string));
                URS_List.Columns.Add("role_id", typeof(string));
                URS_List.Columns.Add("feature_id", typeof(string));
                URS_List.Columns.Add("doc_id", typeof(string));
                URS_List.Columns.Add("grant_access", typeof(string));
                URS_List.Columns.Add("user_id", typeof(string));
                URS_List.Columns.Add("mac_id", typeof(string));

                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                
                JArray jObject = JArray.Parse(_UserRoleSetup_Model.UserRoleSetupList);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowdetails = URS_List.NewRow();
                    dtrowdetails["TransType"] = Session["URS_TransType"].ToString();
                    dtrowdetails["ho_id"] = comp_id;
                    dtrowdetails["role_name"] = _UserRoleSetup_Model.RoleName;
                    dtrowdetails["role_id"] = _UserRoleSetup_Model.RoleID;
                    dtrowdetails["feature_id"] = jObject[i]["feature_id"].ToString();
                    dtrowdetails["doc_id"] = jObject[i]["doc_id"].ToString();
                    dtrowdetails["grant_access"] = jObject[i]["grant_access"].ToString();
                    dtrowdetails["user_id"] = userid;
                    dtrowdetails["mac_id"] = SystemDetail;
                    URS_List.Rows.Add(dtrowdetails);
                }

                UserRoleSetupDetails = URS_List;
                string SaveMessage = _UserRoleSetup_ISERVICE.InsertUserRoleSetupDetails(UserRoleSetupDetails);

                string[] URSDetail = SaveMessage.Split('-');

                string Message = URSDetail[0];
                string URSNo = URSDetail[1];


                if (Message == "Duplicate")
                {
                    Session["URS_Tbl"] = UserRoleSetupDetails;
                    Session["URS_Message"] = "Duplicate";
                }
                else
                {
                    Session["URS_Message"] = "Save";
                }
                Session["URS_Command"] = "Update";
                Session["URS_No"] = URSNo;
                Session["URS_TransType"] = "Update";
                Session["URS_BtnName"] = "BtnSave";
                return RedirectToAction("UserRoleSetupDetail");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult EditUserRoleSetupDetails(string RoleID, string HOID)
        {
            Session["URS_Message"] = "New";
            Session["URS_Command"] = "Update";
            Session["URS_No"] = RoleID;
            Session["URS_TransType"] = "Update";
            //Session["URS_BtnName"] = "BtnEdit";
            Session["URS_BtnName"] = "BtnToDetailPage";
            return RedirectToAction("UserRoleSetupDetail");
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
    }
}