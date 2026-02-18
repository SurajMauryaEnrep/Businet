using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.SecurityLayer.UserSetup;
using Newtonsoft.Json;

namespace EnRepMobileWeb.Areas.SecurityLayer.Controllers.UserSetup
{
    public class UserListController : Controller
    {
        DataTable UserListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, title, Br_ID,userid, language = String.Empty;
        string DocumentMenuId = "104101";
        Common_IServices _Common_IServices;
        UserList_ISERVICES _UserList_ISERVICES;
        public UserListController(Common_IServices _Common_IServices,UserList_ISERVICES _UserList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._UserList_ISERVICES = _UserList_ISERVICES;
        }
        // GET: SecurityLayer/UserList
        public ActionResult UserList()
        {
            //UserDetailModel userDetailModel = new UserDetailModel();          
            string Comp_ID = string.Empty;
            //string Language = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            //if (Session["Language"] != null)
            //{
            //    Language = Session["Language"].ToString();
            //}


            DataTable UserListDs = new DataTable();
            UserListDs = _UserList_ISERVICES.GettopUser(Comp_ID);
            string topUserID = "";

            // Check if table has at least one row
            if (UserListDs != null && UserListDs.Rows.Count > 0)
            {
                // Get only the UserID of the first row (top user)
                 topUserID = UserListDs.Rows[0]["user_id"].ToString(); // adjust column name if different
             

               
            }
            //ViewBag.VBUserList = UserListDs;
            //ViewBag.MenuPageName = getDocumentName();
            //userDetailModel.Title = title;
            //CommonPageDetails();
            Session["Message"] = "PageLoad";
            Session["Command"] = "Refresh";
            Session["AppStatus"] = 'D';
            Session["TransType"] = "Update";
            Session["BtnName"] = "BtnRefresh";

            return RedirectToAction("UserDetail", "UserDetail", new { UserId = topUserID });
            //return View("~/Areas/SecurityLayer/Views/UserSetup/UserDetail.cshtml", userDetailModel);
        }
       
        //private void CommonPageDetails()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            userid = Session["userid"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, userid, DocumentMenuId, language);
        //        ViewBag.AppLevel = ds.Tables[0];
        //        ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
        //        string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
        //        ViewBag.VBRoleList = ds.Tables[3];
        //        ViewBag.StatusList = ds.Tables[4];

        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        ViewBag.MenuPageName = DocumentName;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        throw ex;
        //    }

        //}
        //public ActionResult AddNewUser()
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["AppStatus"] = 'D';
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    return RedirectToAction("UserDetail", "UserDetail");
        //}
        //private string getDocumentName()
        //{

        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    if (Session["Language"] != null)
        //    {
        //        language = Session["Language"].ToString();
        //    }
        //    string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //    string[] Docpart = DocumentName.Split('>');
        //    int len = Docpart.Length;
        //    if (len > 1)
        //    {
        //        title = Docpart[len - 1].Trim();
        //    }
        //    return DocumentName;
        //}
    }
}