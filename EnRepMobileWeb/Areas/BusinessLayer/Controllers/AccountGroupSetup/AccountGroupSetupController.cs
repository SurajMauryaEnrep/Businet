using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AccountGroupSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.AccountGroup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
//***All Session Remove By Shubham Maurya on 11-01-2023 ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.AccountGroupSetup
{
    public class AccountGroupSetupController : Controller
    {
        //string userid;
        int Comp_Id;
        // SearchItemBOL _SearchItemBOL;
        string CompID,br_id, userid, language = String.Empty;
        string DocumentMenuId = "103165",title;
        Common_IServices _Common_IServices;
        AccountGroupModel _AccountGroupModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        AccountGroup_ISERVICES _AccountGroup_ISERVICES;

        // GET: BusinessLayer/AccountGroupSetup

        public AccountGroupSetupController(Common_IServices _Common_IServices,AccountGroup_ISERVICES _AccountGroup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AccountGroup_ISERVICES = _AccountGroup_ISERVICES;
        }
        public ActionResult AccountGroupSetup()
        {
            try
            {
                var _AccountGroupModel = TempData["ModelData"] as AccountGroupModel;
                if (_AccountGroupModel != null)
                {
                    //_AccountGroupModel = new AccountGroupModel();
                    int Comp_ID = 0;
                    _AccountGroupModel.acc_grp_id = null;
                    ViewBag.MenuPageName = getDocumentName();
                    _AccountGroupModel.Title = title;
                    ////Session["TransType"] = "Update";
                    //Session["Command"] = "Test";
                    //Session["BtnName"] = "BtnToDetailPage";
                    _AccountGroupModel.Command = "Test";
                    _AccountGroupModel.BtnName = "BtnToDetailPage";

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetAccGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                        _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });
                    _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;
                    string GroupID = string.Empty;
                    //if (Session["GroupID"] != null )
                    if (_AccountGroupModel.GroupID != null)
                    {
                        //if (Session["GroupID"].ToString() == "")
                        if (_AccountGroupModel.GroupID == "")
                        {

                            GroupID = "0";
                        }
                        else
                        {
                            //GroupID = Session["GroupID"].ToString();
                            GroupID = _AccountGroupModel.GroupID;
                        }

                    }
                    else
                    {
                        GroupID = "0";
                    }


                    DataSet ds = _AccountGroup_ISERVICES.GetDefaultAccGrp(Comp_ID, GroupID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AccountGroupModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _AccountGroupModel.acc_group_name = ds.Tables[0].Rows[0]["acc_group_name"].ToString();
                        _AccountGroupModel.parent_acc_grp_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _AccountGroupModel.grp_type = ds.Tables[0].Rows[0]["grp_type"].ToString().Trim();
                        _AccountGroupModel.alt_grp_id = ds.Tables[0].Rows[0]["alt_grp_id"].ToString();
                        _AccountGroupModel.grp_seq_no = ds.Tables[0].Rows[0]["grp_seq_no"].ToString();
                        _AccountGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Delete_dependcy"].ToString();
                        var CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        if (CreatedBy == "")
                        {
                            _AccountGroupModel.CreatedOn = null;
                            _AccountGroupModel.AmmendedBy = null;
                            _AccountGroupModel.AmmendedOn = null;
                        }
                        else
                        {
                            _AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                        _AccountGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        //_AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();

                    }

                    //if (Session["TransType"] != null)
                    if (_AccountGroupModel.TransType != null)
                    {
                        //_AccountGroupModel.TransType = Session["TransType"].ToString();
                        _AccountGroupModel.TransType = _AccountGroupModel.TransType;

                    }
                    else
                    {
                        _AccountGroupModel.TransType = "Save";
                        //ViewBag.Message = "Save";
                        _AccountGroupModel.Message = "Save";
                        //Session["TransType"] = "Save";
                        _AccountGroupModel.TransType = "Save";
                    }
                    //if (ViewBag.Message == "Save")
                    if (_AccountGroupModel.Message == "Save")
                    {
                        //ViewBag.Message = "New";
                        _AccountGroupModel.Message = "New";
                    }
                    else
                    {
                        //if (Session["Message"] != null)
                        if (_AccountGroupModel.Message != null)
                        {
                            //ViewBag.Message = Session["Message"].ToString();
                            _AccountGroupModel.Message = _AccountGroupModel.Message;
                        }
                        else
                        {
                            //ViewBag.Message = "New";
                            _AccountGroupModel.Message = "New";
                        }
                    }
                    _AccountGroupModel.DeleteCommand = null;
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                }
                else
                {
                    _AccountGroupModel = new AccountGroupModel();
                    int Comp_ID = 0;
                    _AccountGroupModel.acc_grp_id = null;
                    ViewBag.MenuPageName = getDocumentName();
                    _AccountGroupModel.Title = title;
                    ////Session["TransType"] = "Update";
                    //Session["Command"] = "Test";
                    //Session["BtnName"] = "BtnToDetailPage";
                    _AccountGroupModel.Command = "Test";
                    _AccountGroupModel.BtnName = "BtnToDetailPage";

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetAccGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                        _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });
                    _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;
                    string GroupID = string.Empty;
                    //if (Session["GroupID"] != null )
                    if (_AccountGroupModel.GroupID != null)
                    {
                        //if (Session["GroupID"].ToString() == "")
                        if (_AccountGroupModel.GroupID == "")
                        {

                            GroupID = "0";
                        }
                        else
                        {
                            //GroupID = Session["GroupID"].ToString();
                            GroupID = _AccountGroupModel.GroupID;
                        }

                    }
                    else
                    {
                        GroupID = "0";
                    }


                    DataSet ds = _AccountGroup_ISERVICES.GetDefaultAccGrp(Comp_ID, GroupID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AccountGroupModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _AccountGroupModel.acc_group_name = ds.Tables[0].Rows[0]["acc_group_name"].ToString();
                        _AccountGroupModel.parent_acc_grp_id = ds.Tables[0].Rows[0]["ItemParId"].ToString();
                        _AccountGroupModel.grp_type = ds.Tables[0].Rows[0]["grp_type"].ToString().Trim();
                        _AccountGroupModel.alt_grp_id = ds.Tables[0].Rows[0]["alt_grp_id"].ToString();
                        _AccountGroupModel.grp_seq_no = ds.Tables[0].Rows[0]["grp_seq_no"].ToString();
                        _AccountGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Delete_dependcy"].ToString();
                        var CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        if (CreatedBy == "")
                        {
                            _AccountGroupModel.CreatedOn = null;
                            _AccountGroupModel.AmmendedBy = null;
                            _AccountGroupModel.AmmendedOn = null;
                        }
                        else
                        {
                            _AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                        _AccountGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();            
                    }

                    //if (Session["TransType"] != null)
                    if (_AccountGroupModel.TransType != null)
                    {
                        //_AccountGroupModel.TransType = Session["TransType"].ToString();
                        _AccountGroupModel.TransType = _AccountGroupModel.TransType;

                    }
                    else
                    {
                        _AccountGroupModel.TransType = "Save";
                        //ViewBag.Message = "Save";
                        _AccountGroupModel.Message = "Save";
                        //Session["TransType"] = "Save";
                        _AccountGroupModel.TransType = "Save";
                    }
                    //if (ViewBag.Message == "Save")
                    if (_AccountGroupModel.Message == "Save")
                    {
                        //ViewBag.Message = "New";
                        _AccountGroupModel.Message = "New";
                    }
                    else
                    {
                        //if (Session["Message"] != null)
                        if (_AccountGroupModel.Message != null)
                        {
                            //ViewBag.Message = Session["Message"].ToString();
                            _AccountGroupModel.Message = _AccountGroupModel.Message;
                        }
                        else
                        {
                            //ViewBag.Message = "New";
                            _AccountGroupModel.Message = "New";
                        }
                    }
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                }              
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult AccountGroupSetupAdd(string GroupID, string TransType, string BtnName, string command,string ChkPgroup)
        {
            try
            {
                var _AccountGroupModel = TempData["ModelData"] as AccountGroupModel;
                if (_AccountGroupModel != null)
                {
                    //_AccountGroupModel = new AccountGroupModel();
                    int Comp_ID = 0;
                    //if (Session["GroupID"] != null)
                    if (_AccountGroupModel.GroupID != null)
                    {
                        //_AccountGroupModel.acc_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                        _AccountGroupModel.acc_grp_id = Convert.ToInt32(_AccountGroupModel.GroupID);
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    _AccountGroupModel.Title = title;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetAccGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                        _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });

                    _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;



                    //var transtype = Session["TransType"].ToString();
                    var transtype = _AccountGroupModel.TransType;

                    if (transtype == "Update" || transtype == "Edit")
                    {

                        //string GroupID = Session["GroupID"].ToString();
                         GroupID = _AccountGroupModel.GroupID;
                        string AccGrpId = (_AccountGroupModel.acc_grp_id).ToString();
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = int.Parse(Session["CompId"].ToString());
                        }

                        DataSet ds = _AccountGroup_ISERVICES.GetAccDetail(AccGrpId, Comp_ID);


                        _AccountGroupModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _AccountGroupModel.acc_group_name = ds.Tables[0].Rows[0]["acc_group_name"].ToString();
                        _AccountGroupModel.parent_acc_grp_id = ds.Tables[0].Rows[0]["AccParId"].ToString();
                        _AccountGroupModel.grp_type = ds.Tables[0].Rows[0]["grp_type"].ToString().Trim();
                        _AccountGroupModel.alt_grp_id = ds.Tables[0].Rows[0]["alt_grp_id"].ToString();
                        _AccountGroupModel.grp_seq_no = ds.Tables[0].Rows[0]["grp_seq_no"].ToString();
                        _AccountGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Delete_dependcy"].ToString();
                        var CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        if (CreatedBy == "")
                        {
                            _AccountGroupModel.CreatedOn = null;
                            _AccountGroupModel.AmmendedBy = null;
                            _AccountGroupModel.AmmendedOn = null;
                        }
                        else
                        {
                            _AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                        _AccountGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        //_AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();


                    }
                    //_AccountGroupModel.TransType = Session["TransType"].ToString();
                    _AccountGroupModel.TransType = _AccountGroupModel.TransType;
                    //if (Session["Message"] != null)
                    //if (_AccountGroupModel.Message != null)
                    //{
                    //    //ViewBag.Message = Session["Message"].ToString();
                    //    ViewBag.Message = _AccountGroupModel.Message;
                    //}
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                }
                else
                {
                    _AccountGroupModel = new AccountGroupModel();
                    if (_AccountGroupModel.GroupID == null)
                    {
                        _AccountGroupModel.GroupID = GroupID;
                    }
                    if (_AccountGroupModel.TransType == null)
                    {
                        _AccountGroupModel.TransType = TransType;
                    }
                    if (_AccountGroupModel.BtnName == null)
                    {
                        _AccountGroupModel.BtnName = BtnName;
                    }
                    if (_AccountGroupModel.Command == null)
                    {
                        _AccountGroupModel.Command = command;
                    }
                    if (_AccountGroupModel.ChkPgroup == null)
                    {
                        _AccountGroupModel.ChkPgroup = ChkPgroup;
                    }
                    int Comp_ID = 0;
                    //if (Session["GroupID"] != null)
                    if (_AccountGroupModel.GroupID != null)
                    {
                        //_AccountGroupModel.acc_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                        _AccountGroupModel.acc_grp_id = Convert.ToInt32(_AccountGroupModel.GroupID);
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    _AccountGroupModel.Title = title;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    dt = GetAccGroupParentList();
                    List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                    foreach (DataRow dt in dt.Rows)
                    {
                        AccountGroup _AccountGroupList = new AccountGroup();
                        _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                        _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                        _ItemAccountGroupList.Add(_AccountGroupList);
                    }
                    _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });

                    _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;



                    //var transtype = Session["TransType"].ToString();
                    var transtype = _AccountGroupModel.TransType;

                    if (transtype == "Update" || transtype == "Edit")
                    {

                        //string GroupID = Session["GroupID"].ToString();
                        GroupID = _AccountGroupModel.GroupID;
                        string AccGrpId = (_AccountGroupModel.acc_grp_id).ToString();
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = int.Parse(Session["CompId"].ToString());
                        }

                        DataSet ds = _AccountGroup_ISERVICES.GetAccDetail(AccGrpId, Comp_ID);


                        _AccountGroupModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        _AccountGroupModel.acc_group_name = ds.Tables[0].Rows[0]["acc_group_name"].ToString();
                        _AccountGroupModel.parent_acc_grp_id = ds.Tables[0].Rows[0]["AccParId"].ToString();
                        _AccountGroupModel.grp_type = ds.Tables[0].Rows[0]["grp_type"].ToString().Trim();
                        _AccountGroupModel.alt_grp_id = ds.Tables[0].Rows[0]["alt_grp_id"].ToString();
                        _AccountGroupModel.grp_seq_no = ds.Tables[0].Rows[0]["grp_seq_no"].ToString();
                        _AccountGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        _AccountGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Delete_dependcy"].ToString();
                        var CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        if (CreatedBy == "")
                        {
                            _AccountGroupModel.CreatedOn = null;
                            _AccountGroupModel.AmmendedBy = null;
                            _AccountGroupModel.AmmendedOn = null;
                        }
                        else
                        {
                            _AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            _AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            _AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                        }
                        //_AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    }
                    //_AccountGroupModel.TransType = Session["TransType"].ToString();
                    _AccountGroupModel.TransType = _AccountGroupModel.TransType;
                    //if (Session["Message"] != null)
                    //if (_AccountGroupModel.Message != null)
                    //{
                    //    //ViewBag.Message = Session["Message"].ToString();
                    //    ViewBag.Message = _AccountGroupModel.Message;
                    //}
                    if (_AccountGroupModel.BtnName == null)
                    {
                        _AccountGroupModel.BtnName = "BtnAddNew";
                        _AccountGroupModel.grp_type = "0";
                    }
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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
        public ActionResult AccountGroupSetupView(string AccGrpId)
        {
            try
            {
                _AccountGroupModel = new AccountGroupModel();
                //Session["TransType"] = "Save";
                _AccountGroupModel.TransType = "Save";
                //Session["Command"] = "Test";
                _AccountGroupModel.Command = "Test";
                //Session["BtnName"] = "BtnToDetailPage";
                _AccountGroupModel.BtnName = "BtnToDetailPage";
               
                int Comp_ID = 0;
               // _AccountGroupModel.acc_grp_id = Convert.ToInt32(Session["GroupID"].ToString());
                ViewBag.MenuPageName = getDocumentName();
                _AccountGroupModel.Title = title;
                if (Session["CompId"] != null)
                {
                    Comp_ID = int.Parse(Session["CompId"].ToString());
                }

                dt = GetAccGroupParentList();
                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                foreach (DataRow dt in dt.Rows)
                {
                    AccountGroup _AccountGroupList = new AccountGroup();
                    _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                    _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                    _ItemAccountGroupList.Add(_AccountGroupList);
                }
                _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });

                _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;

                    if (Session["CompId"] != null)
                    {
                        Comp_ID = int.Parse(Session["CompId"].ToString());
                    }

                    DataSet ds = _AccountGroup_ISERVICES.GetAccViewDetail(AccGrpId, Comp_ID);


                    _AccountGroupModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                    _AccountGroupModel.acc_group_name = ds.Tables[0].Rows[0]["acc_group_name"].ToString();
                    _AccountGroupModel.parent_acc_grp_id = ds.Tables[0].Rows[0]["AccParId"].ToString();
                    _AccountGroupModel.grp_type = ds.Tables[0].Rows[0]["grp_type"].ToString().Trim();
                    _AccountGroupModel.alt_grp_id = ds.Tables[0].Rows[0]["alt_grp_id"].ToString();
                    _AccountGroupModel.grp_seq_no = ds.Tables[0].Rows[0]["grp_seq_no"].ToString();
                _AccountGroupModel.Delete_Dependcy = ds.Tables[0].Rows[0]["Delete_dependcy"].ToString();
                var CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                if (CreatedBy == "")
                {
                    _AccountGroupModel.CreatedOn = null;
                    _AccountGroupModel.AmmendedBy = null;
                    _AccountGroupModel.AmmendedOn = null;
                }
                else
                {
                    _AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    _AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    _AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                }
                _AccountGroupModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    //_AccountGroupModel.CreatedOn = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                    //_AccountGroupModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    //_AccountGroupModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();

                //}
                //_AccountGroupModel.TransType = Session["TransType"].ToString();
                _AccountGroupModel.TransType = _AccountGroupModel.TransType;
                //ViewBag.Message = Session["Message"].ToString();
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
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

        public ActionResult AccountGroupSave(AccountGroupModel _AccountGroupModel, string command, string acc_grp_id)
        {
            try
            {
                if (_AccountGroupModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        _AccountGroupModel.Message = "";
                        _AccountGroupModel.hdnSavebtn = null;
                        //Session["Command"] = command;
                        _AccountGroupModel.Command = command;
                        //Session["GroupID"] = _AccountGroupModel.acc_grp_id;
                        _AccountGroupModel.GroupID = _AccountGroupModel.acc_grp_id.ToString();
                        _AccountGroupModel.FormMode = "1";
                        //Session["TransType"] = "Update";
                        _AccountGroupModel.TransType = "Update";
                        //Session["BtnName"] = "BtnEdit";
                        _AccountGroupModel.BtnName = "BtnEdit";
                        var GroupID = _AccountGroupModel.acc_grp_id.ToString();
                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        string status = Check_GroupDependency(GroupID);
                        if (status == "Y")
                        {
                            //Session["ChkPgroup"] = "Y";
                            _AccountGroupModel.ChkPgroup = "Y";
                        }
                        else
                        {
                            //Session["ChkPgroup"] =null;
                            _AccountGroupModel.ChkPgroup = null;
                        }
                        var ChkPgroup = _AccountGroupModel.ChkPgroup;
                        TempData["ModelData"] = _AccountGroupModel;
                        return( RedirectToAction("AccountGroupSetupAdd", "AccountGroupSetup", new { GroupID = GroupID, TransType, BtnName, command, ChkPgroup }));

                    case "Add":
                        //Session["Message"] = "";
                        _AccountGroupModel.hdnSavebtn = null;
                        _AccountGroupModel.Message = "";
                        //Session["Command"] = command;
                        _AccountGroupModel.Command = command;
                        //Session["GroupID"] = 0;
                        _AccountGroupModel.GroupID = "0";
                        //Session["AppStatus"] = "D";
                        //_AccountGroupModel = null;
                        //Session["TransType"] = "Save";
                        _AccountGroupModel.TransType = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        _AccountGroupModel.BtnName = "BtnAddNew";
                        _AccountGroupModel.grp_type = "0";
                        TempData["ModelData"] = _AccountGroupModel;
                        return RedirectToAction("AccountGroupSetupAdd", "AccountGroupSetup");


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        //Session["TransType"] = "Refresh";
                        _AccountGroupModel.hdnSavebtn = null;
                        _AccountGroupModel.Command = command;
                        _AccountGroupModel.BtnName = "Delete";
                        _AccountGroupModel.TransType = "Refresh";
                        acc_grp_id = Convert.ToInt32(_AccountGroupModel.acc_grp_id).ToString();
                         DeleteAccGroup(_AccountGroupModel);
                        TempData["ModelData"] = _AccountGroupModel;
                        return RedirectToAction("AccountGroupSetup");
                        //return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);

                    case "Save":
                        //Session["Command"] = command;
                        _AccountGroupModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertAccGroupDetail(_AccountGroupModel);
                            //Session["GroupID"] = Session["GroupID"].ToString();
                            _AccountGroupModel.GroupID = _AccountGroupModel.GroupID;

                            //if (Session["Message"].ToString() == "Duplicate")
                            if (_AccountGroupModel.Message == "Duplicate")
                            {

                                dt = GetAccGroupParentList();
                                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroupList = new AccountGroup();
                                    _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                                    _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                                    _ItemAccountGroupList.Add(_AccountGroupList);
                                }
                                _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });
                                _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;
                                string AccGrpId = (_AccountGroupModel.acc_grp_id).ToString();


                                //Session["GroupID"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                //Session["Command"] = "Add";

                                _AccountGroupModel.GroupID = "";
                                _AccountGroupModel.AppStatus = "D";
                                _AccountGroupModel.TransType = "Save";
                                _AccountGroupModel.BtnName = "BtnAddNew";
                                _AccountGroupModel.Command = "Add";
                                _AccountGroupModel.Message = "Duplicate";
                                //ViewBag.Message = Session["Message"].ToString();
                                //ViewBag.Message = _AccountGroupModel.Message;
                                return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                            }
                            //if (Session["Message"].ToString() == "ValidationForSameName")
                            if (_AccountGroupModel.Message == "ValidationForSameName")
                            {
                                CommonPageDetails();
                                dt = GetAccGroupParentList();

                                List<AccountGroup> _ItemAccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroupList = new AccountGroup();
                                    _AccountGroupList.acc_grp_struc = dt["acc_grp_struc"].ToString();
                                    _AccountGroupList.acc_group_name = dt["acc_group_name"].ToString();
                                    _ItemAccountGroupList.Add(_AccountGroupList);
                                }
                                _ItemAccountGroupList.Insert(0, new AccountGroup() { acc_grp_struc = "-1", acc_group_name = "---Select---" });
                                _AccountGroupModel.ParentItemGroup = _ItemAccountGroupList;
                                string AccGrpId = (_AccountGroupModel.acc_grp_id).ToString();


                                //Session["GroupID"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                //Session["Command"] = "Add";
                                //ViewBag.Message = Session["Message"].ToString();

                                _AccountGroupModel.GroupID = "";
                                _AccountGroupModel.AppStatus = "D";
                                _AccountGroupModel.TransType = "Save";
                                _AccountGroupModel.BtnName = "BtnAddNew";
                                _AccountGroupModel.Command = "Add";
                                _AccountGroupModel.hdnSavebtn = null;
                                //ViewBag.Message = _AccountGroupModel.Message.ToString();
                                return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                            }
                            else
                                GroupID = _AccountGroupModel.GroupID;
                            TransType = _AccountGroupModel.TransType;
                            BtnName = _AccountGroupModel.BtnName;
                            TempData["ModelData"] = _AccountGroupModel;
                                return( RedirectToAction("AccountGroupSetupAdd", new { GroupID = GroupID, TransType, BtnName, command }));
                        }
                        else
                        {
                            _AccountGroupModel = null;
                            return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        _AccountGroupModel.hdnSavebtn = null;
                        //Session["Command"] = command;
                        _AccountGroupModel.Command = command;
                        acc_grp_id = Convert.ToInt32(_AccountGroupModel.acc_grp_id).ToString();
                        //Session["GroupID"] = acc_grp_id;
                        _AccountGroupModel.GroupID = acc_grp_id;
                        // ItemApprove(_AccountGroupModel, command);
                        TempData["ModelData"] = _AccountGroupModel;
                        return RedirectToAction("AccountGroupSetupAdd");


                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        //_AccountGroupModel = null;
                        _AccountGroupModel.hdnSavebtn = null;
                        _AccountGroupModel.BtnName = "BtnRefresh";
                        _AccountGroupModel.Command = command;
                        _AccountGroupModel.TransType = "Refresh";
                        _AccountGroupModel.Message = "";
                        _AccountGroupModel.AppStatus = "";
                        TempData["ModelData"] = _AccountGroupModel;
                        return RedirectToAction("AccountGroupSetup");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        return RedirectToAction("Index", "DashboardHome", new { area = "Dashboard" });

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
        public JsonResult GetAllAccGrp(AccMenuSearchModel ObjAccMenuSearchModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    ObjAccMenuSearchModel.Comp_ID = Session["CompId"].ToString();
                }
                JsonResult DataRows = null;
                string FinalData = string.Empty;
                Newtonsoft.Json.Linq.JObject FData = new Newtonsoft.Json.Linq.JObject();
                FData = _AccountGroup_ISERVICES.GetAllAccGrpBl(ObjAccMenuSearchModel);
                DataRows = Json(JsonConvert.SerializeObject(FData), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        //public ActionResult AccountGroup()
        //{

        //    int Comp_ID = 0;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = int.Parse(Session["CompId"].ToString());
        //    }
        //    AccountGroupSetupModel ObjModel = new AccountGroupSetupModel();
        //    ObjModel.GetAccGroupSetup(Comp_ID);
        //    return View("AccountGroup", ObjModel);
        //}

        public ActionResult InsertAccGroupDetail(AccountGroupModel _AccountGroupModel)
        {
            try
            {
                //throw new Exception("Something went wrong");
                //JsonResult Result = Json("please fill all mandatory field");
                //ViewBag.PageHeader = PageHeader;
                //if (Session["TransType"].ToString() == "Update")
                if (_AccountGroupModel.TransType == "Update")
                {
                    _AccountGroupModel.FormMode = "1";
                }
                _AccountGroupModel.ChkPgroup = null;
                if (Session["CompId"] != null)
                {
                    _AccountGroupModel.comp_id = int.Parse(Session["CompId"].ToString());
                    _AccountGroupModel.create_id = int.Parse(Session["UserId"].ToString());
                    if (_AccountGroupModel.FormMode == "1")
                    {
                        _AccountGroupModel.mod_id = int.Parse(Session["UserId"].ToString());
                    }
                }
                string ParGrpID = _AccountGroupModel.parent_acc_grp_id;
                string GrpID = ParGrpID.Substring(ParGrpID.Length - 3, 3);
                if (_AccountGroupModel.acc_grp_id.ToString() != GrpID)
                {
                    string status = _AccountGroup_ISERVICES.InsertAccGrpDetail(_AccountGroupModel);

                    string GroupID = status.Substring(status.IndexOf('-') + 1);

                    string Message = status.Substring(0, status.IndexOf("-"));

                    if (Message == "Update" || Message == "Save")
                    {
                        //Session["Message"] = "Save";
                        //Session["GroupID"] = GroupID;
                        //Session["TransType"] = "Update";
                        //ViewBag.Message = Session["Message"].ToString();

                        _AccountGroupModel.Message = "Save";
                        _AccountGroupModel.GroupID = GroupID;
                        _AccountGroupModel.TransType = "Update";
                        //ViewBag.Message = _AccountGroupModel.Message;
                    }
                    if (Message == "Duplicate")
                    {
                        //Session["TransType"] = "Duplicate";
                        //Session["Message"] = "Duplicate";
                        //Session["GroupID"] = GroupID;
                        //ViewBag.Message = Session["Message"].ToString();
                        _AccountGroupModel.hdnSavebtn = null;
                        _AccountGroupModel.TransType = "Duplicate";
                        _AccountGroupModel.Message = "Duplicate";
                        _AccountGroupModel.GroupID = GroupID;
                        //ViewBag.Message = _AccountGroupModel.Message;
                    }

                    //Session["BtnName"] = "BtnSave";
                    _AccountGroupModel.BtnName = "BtnSave";
                    TempData["ModelData"] = _AccountGroupModel;
                    return RedirectToAction("AccountGroupSetupAdd", "AccountGroupSetup");

                }
                else
                {
                    //Session["Message"] = "ValidationForSameName";
                    _AccountGroupModel.Message = "ValidationForSameName";
                    //ViewBag.Message = Session["Message"].ToString();
                    //ViewBag.Message = _AccountGroupModel.Message;
                }
                TempData["ModelData"] = _AccountGroupModel;
                return RedirectToAction("AccountGroupSetup", "AccountGroupSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }

        //public ActionResult Check_GroupDependency(string AccGroupID)
        public string Check_GroupDependency(string AccGroupID)
        {
            try
            {
                //_AccountGroupModel = new AccountGroupModel();
                //JsonResult Result = Json("");
                int comp_id = 0;
                int acc_grp_id = 0;
                if (Session["CompId"] != null)
                {
                    comp_id = int.Parse(Session["CompId"].ToString());
                }
                if (AccGroupID != "")
                {
                    acc_grp_id = int.Parse(AccGroupID);
                }

                string status = _AccountGroup_ISERVICES.ChkPGroupDependency(acc_grp_id, comp_id);
                //if (status == "Y")
                //{
                //    //Session["ChkPgroup"] = "Y";
                //    _AccountGroupModel.ChkPgroup = "Y";
                //}
                //else
                //{
                //    //Session["ChkPgroup"] = null;
                //    _AccountGroupModel.ChkPgroup = null;
                //}
                //return Json(status);
                return status;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path,Ex);
                //return Json("ErrorPage");
                throw Ex;
            }
        }
        public string get_grptype(string grp_val)
        {
            try
            {
                int comp_id = 0;
                //int acc_grp_id = 0;
                if (Session["CompId"] != null)
                {
                    comp_id = int.Parse(Session["CompId"].ToString());
                }

                string status = _AccountGroup_ISERVICES.get_grptype(comp_id, grp_val);
                return status;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public JsonResult GetDefaultAccGrp()
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _AccountGroup_ISERVICES.GetDefaultAccGrp(Comp_Id,"0");
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult GetAccDetail(string AccGrpId)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _AccountGroup_ISERVICES.GetAccDetail(AccGrpId, Comp_Id);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public JsonResult GetAccViewDetail(string AccGrpId)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    if (Session["CompId"] != null)
                    {
                        Comp_Id = int.Parse(Session["CompId"].ToString());
                    }
                    DataSet result = _AccountGroup_ISERVICES.GetAccViewDetail(AccGrpId, Comp_Id);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult DeleteAccGroup(AccountGroupModel _AccountGroupModel)
        {
            try
            {
                JsonResult Result = Json("");
                //ViewBag.PageHeader = PageHeader;
                int comp_id = 0;
                if (Session["CompId"] != null)
                {
                    comp_id = int.Parse(Session["CompId"].ToString());
                }
                int AccGrpID = int.Parse(_AccountGroupModel.acc_grp_id.ToString());
                string status = _AccountGroup_ISERVICES.DeleteAccGroup(AccGrpID, comp_id);
                if (status == "Deleted")
                {
                    //Session["Message"] = "Deleted";
                    //Session["Command"] = "Delete";
                    //Session["GroupID"] = "";
                    //_AccountGroupModel = null;
                    //Session["TransType"] = "Refresh";
                    //Session["BtnName"] = "Delete";
                    //ViewBag.Message = Session["Message"].ToString();

                    _AccountGroupModel.Message = "Deleted";
                    _AccountGroupModel.Command = "Delete";
                    _AccountGroupModel.GroupID = "";
                    _AccountGroupModel.TransType = "Refresh";
                    _AccountGroupModel.BtnName = "Delete";
                    //ViewBag.Message = _AccountGroupModel.Message;
                    TempData["ModelData"] = _AccountGroupModel;
                    return RedirectToAction("AccountGroupSetup", "AccountGroupSetup");
                }
                else
                {
                    //Session["TransType"] = "Refresh";
                    //Session["Message"] = "Dependency";
                    //Session["Command"] = "Delete";
                    //Session["GroupID"] = AccGrpID;
                    //ViewBag.Message = Session["Message"].ToString();

                    _AccountGroupModel.TransType = "Refresh";
                    _AccountGroupModel.Message = "Dependency";
                    _AccountGroupModel.Command = "Delete";
                    _AccountGroupModel.GroupID = AccGrpID.ToString();
                    //ViewBag.Message = _AccountGroupModel.Message.ToString();
                    return View("~/Areas/BusinessLayer/Views/AccountGroupSetup/AccountGroupSetup.cshtml", _AccountGroupModel);
                }
               
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //public void LogError(Exception ex)
        //{
        //    try
        //    {
        //        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        message += Environment.NewLine;
        //        message += "-----------------------------------------------------------";
        //        message += Environment.NewLine;
        //        message += string.Format("Message: {0}", ex.Message);
        //        message += Environment.NewLine;
        //        message += string.Format("StackTrace: {0}", ex.StackTrace);
        //        message += Environment.NewLine;
        //        message += string.Format("Source: {0}", ex.Source);
        //        message += Environment.NewLine;
        //        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        //        message += Environment.NewLine;
        //        message += "-----------------------------------------------------------";
        //        message += Environment.NewLine;
        //        string path = Server.MapPath("~/Log/ErrorLog.txt");
        //        using (StreamWriter writer = new StreamWriter(path, true))
        //        {
        //            writer.WriteLine(message);
        //            writer.Close();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //}
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
                    br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, br_id, userid, DocumentMenuId, language);
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
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetAccGroupParentList()
        {
            try
            {
                int CompId = 0;
                if (Session["CompId"] != null)
                {
                    CompId = int.Parse(Session["CompId"].ToString());
                }

                DataTable dt = _AccountGroup_ISERVICES.GetAccGroupSetup(CompId);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
    }
}
