using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.WorkstationSetup;//.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.WorkstationSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using Status = EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.WorkstationSetup.Status;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.WorkstationSetup
{
    public class WorkstationSetupController : Controller
    {
        string compn_id,branch_id, UserID;
        string DocumentMenuId = "105105105";
        string CompID, title = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string language = String.Empty;
        
        WorkstationSetup_ISERVICES _WorkstationSetup_ISERVICES;
        Common_IServices _Common_IServices;

        public WorkstationSetupController(Common_IServices _Common_IServices, WorkstationSetup_ISERVICES _WorkstationSetup_ISERVICES)
        {
            this._WorkstationSetup_ISERVICES = _WorkstationSetup_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/WorkstationSetup
        WorkstationSetupModel _WorkstationSetupModel = new WorkstationSetupModel();
        
        public ActionResult WorkstationSetup(string command)
        {
            WorkstationSetupModel _WorkstationSetupModel = new WorkstationSetupModel();
            try
            {
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _WorkstationSetupModel.Title = title;
                // ViewBag.MenuPageName = Session["MenuPageName"].ToString();
                BindShopFloorList(_WorkstationSetupModel);
                int shfl_id = 0;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    shfl_id = Convert.ToInt32(a[0].Trim());
                    var Command = a[1].Trim();
                    _WorkstationSetupModel.Status = shfl_id.ToString();
                    ViewBag.ListData = TempData["ListFilterData"].ToString();
                    ViewBag.GetWSDetails = GetWSDetails(shfl_id, Command);
                }
                else
                {
                    ViewBag.GetWSDetails = GetWSDetails(shfl_id, "");
                }
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupList.cshtml", _WorkstationSetupModel);
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
                    branch_id = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branch_id, UserID, DocumentMenuId, language);
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
                string UserID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult WorkstationSetup(WorkstationSetupModel _WorkstationSetupModel, string command)
        {
            try
            {
                DataTable dt = new DataTable();
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _WorkstationSetupModel.Title = title;
                //Comp_ID = Session["CompId"].ToString();
                //string br_id = Session["BranchId"].ToString();
                string shfl_id = _WorkstationSetupModel.Status;
                //dt = _WorkstationSetup_ISERVICES.GetWSDetailsDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id),Convert.ToInt32(shfl_id), command);
                //if(dt.Rows.Count>0)
                // {
                BindShopFloorList(_WorkstationSetupModel);
                ViewBag.GetWSDetails = GetWSDetails(Convert.ToInt32(shfl_id), command);
                if (command == "Search")
                {
                    var ListfilterData = shfl_id + ',' + command;
                    ViewBag.ListData = ListfilterData;
                }
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupList.cshtml", _WorkstationSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable GetWSDetails(int shfl_id,string command)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branch_id = Session["BranchId"].ToString();
                }
                if (shfl_id == 0)
                {
                    command = "";
                }
                dt = _WorkstationSetup_ISERVICES.GetWSDetailsDAL(Convert.ToInt32(CompID), Convert.ToInt32(branch_id), shfl_id, command);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
            return dt;
        }
        public ActionResult AddWorkstationSetupDetail()
        {
            try
            {

                DataTable dt = new DataTable();
                WorkstationSetupModel _WorkstationSetupModel = new WorkstationSetupModel();
                CommonPageDetails();
                BindShopFloorList(_WorkstationSetupModel);
                //_WorkstationSetupModel.ShopFloorList = lst;
                //ViewBag.MenuPageName = Session["MenuPageName"].ToString();
                //ViewBag.MenuPageName = Session["MenuPageName"].ToString();
               GetAutoCompleteSearchSuggestion(_WorkstationSetupModel);
                //ViewBag.MenuPageName = getDocumentName();
                _WorkstationSetupModel.Title = title;
                _WorkstationSetupModel.Message = "New";
                _WorkstationSetupModel.Command = "Add";
                _WorkstationSetupModel.AppStatus = "D";
                _WorkstationSetupModel.TransType = "Save";
                _WorkstationSetupModel.BtnName = "BtnAddNew";

                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["AppStatus"] = 'D';
                //Session["TransType"] = "Save";
                //Session["BtnName"] = "BtnAddNew";
                //ViewBag.VBRoleList = GetRoleList();
                TempData["ListFilterData"] = null;
                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branch_id = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("WorkstationSetup");
                }
                /*End to chk Financial year exist or not*/
                _WorkstationSetupModel.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupDetail.cshtml", _WorkstationSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            // return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupDetail.cshtml");
        }
        public ActionResult GetAutoCompleteSearchSuggestion(WorkstationSetupModel _WorkstationSetupModel)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> GroupList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_WorkstationSetupModel.ddlgroup_name))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _WorkstationSetupModel.ddlgroup_name;
                }
                GroupList = _WorkstationSetup_ISERVICES.GetGroupList(Comp_ID, GroupName);

                List<GroupName> _GroupList = new List<GroupName>();

                foreach (var data in GroupList)
                {
                    GroupName _GroupDetail = new GroupName();
                    _GroupDetail.item_grp_id = data.Key;
                    _GroupDetail.ItemGroupChildNood = data.Value;
                    _GroupList.Add(_GroupDetail);
                }
                _WorkstationSetupModel.GroupList = _GroupList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(GroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult WorkStationSave(WorkstationSetupModel _WorkstationSetupModel, string command)
        {

            /*----- Attatchment Section start--------*/
            string SaveMessage = "";
            CommonPageDetails();
            //string PageName = _WorkstationSetupModel.Title.Replace(" ", "");
            //getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            /*----- Attatchment Section End--------*/
            try
            { /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    compn_id = Session["compid"].ToString();
                    branch_id = Session["BranchId"].ToString();
                    _WorkstationSetupModel.comp_id = Convert.ToInt32(Session["compid"].ToString());
                    _WorkstationSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    _WorkstationSetupModel.create_id = Convert.ToInt32(Session["UserId"].ToString());
                    string ws_name = _WorkstationSetupModel.ws_name;
                    string shflid = _WorkstationSetupModel.Status;
                    Int32 shfl_id = Convert.ToInt32(shflid);
                    string op_st_date = _WorkstationSetupModel.op_st_date;
                    string op_name = _WorkstationSetupModel.op_name;
                    string sr_no = _WorkstationSetupModel.sr_no;
                    string Make = _WorkstationSetupModel.Make;
                    string Model_no = _WorkstationSetupModel.Model_no;
                    string Grp_nm = "";
                    if (_WorkstationSetupModel.Group_name == null)
                    {
                         Grp_nm = "0";
                    }
                    else
                    {
                         Grp_nm = _WorkstationSetupModel.Group_name;
                    }
                    Int32 create_id = _WorkstationSetupModel.create_id;
                    Int32 br_id = _WorkstationSetupModel.br_id;
                    Int32 ws_id = _WorkstationSetupModel.ws_id;
                    Int32 comp_id = _WorkstationSetupModel.comp_id;
                    string mac_id = string.Empty;
                    mac_id = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    
                    //string shfl_remarks = _WorkstationSetupModel.;
                    //ViewData[""]
                    //Session["shfl_name"] = shfl_name;
                    //Session["shfl_loc"] = shfl_loc;
                    //Session["shfl_remarks"] = shfl_remarks;
                    //_ShopfloorSetupModel.TransType = command;
                    //_ShopfloorSetupModel.create_id = Session["UserId"].ToString();
                    //_ShopfloorSetupModel.comp_id = Convert.ToInt32(Session["CompId"].ToString());
                    //_ShopfloorSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());


                    if (_WorkstationSetupModel.DeleteCommand == "Delete")
                    {
                        command = "Delete";
                    }

                    switch (command)
                    {
                        case "Edit":
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                branch_id = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                return RedirectToAction("dbClickEdit", new { comp_id = _WorkstationSetupModel.comp_id, br_id = _WorkstationSetupModel.br_id, ws_id = _WorkstationSetupModel.ws_id, ListFilterData = _WorkstationSetupModel.ListFilterData1 });
                            }
                            /*End to chk Financial year exist or not*/
                            //Session["Message"] = "";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["TransType"] = "Update";
                            UrlModelData UrlData = new UrlModelData();
                            _WorkstationSetupModel.Message = "";
                            _WorkstationSetupModel.Command = command;
                            _WorkstationSetupModel.BtnName = "BtnAddNew";
                            _WorkstationSetupModel.TransType = "Update";
                            UrlData.BtnName= "BtnAddNew";
                            UrlData.TransType= "Update";
                            UrlData.Command= command;
                            UrlData.ws_id= _WorkstationSetupModel.ws_id;
                            TempData["ModelData"] = _WorkstationSetupModel;
                            TempData["ListFilterData"] = _WorkstationSetupModel.ListFilterData1;
                            return RedirectToAction("WorkstationSetupDetail", UrlData);

                        case "Add":
                            WorkstationSetupModel _WorkstationSetupModelAddNew= new WorkstationSetupModel();
                            _WorkstationSetupModelAddNew.Message = "";
                            _WorkstationSetupModelAddNew.Command = command;
                            _WorkstationSetupModelAddNew.TransType = "Save";
                            _WorkstationSetupModelAddNew.BtnName = "BtnAddNew";
                            //Session["Message"] = "";
                            //Session["Command"] = command;
                            //Session["TransType"] = "Save";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session.Remove("ws_id");
                            //Session.Remove("br_id");
                            //Session.Remove("SaveUpd");
                            TempData["ModelData"] = _WorkstationSetupModelAddNew;
                            TempData["ListFilterData"] = null;
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                branch_id = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                //_ShopfloorSetupModel.sh_id = _ShopfloorSetupModel.shfl_id.ToString();
                                if (_WorkstationSetupModel.ws_id != 0)
                                    return RedirectToAction("dbClickEdit", new { comp_id= _WorkstationSetupModel.comp_id, br_id = _WorkstationSetupModel.br_id, ws_id = _WorkstationSetupModel.ws_id, ListFilterData= _WorkstationSetupModel.ListFilterData1 });
                                else
                                    _WorkstationSetupModelAddNew.Command = "Refresh";
                                _WorkstationSetupModelAddNew.TransType = "Refresh";
                                _WorkstationSetupModelAddNew.BtnName = "BtnRefresh";
                                TempData["ModelData"] = _WorkstationSetupModelAddNew;
                                return RedirectToAction("WorkstationSetupDetail", _WorkstationSetupModelAddNew);
                            }
                            /*End to chk Financial year exist or not*/
                            return RedirectToAction("WorkstationSetupDetail");
                           
                        case "Delete":
                            WorkstationSetupModel _WorkstationSetupModelDelete = new WorkstationSetupModel();
                            //Session["Command"] = command;
                            //Session["TransType"] = "Delete";
                            _WorkstationSetupModel.TransType = "Delete";
                            var Trtype1 = _WorkstationSetupModel.TransType;
                            var Trtype = _WorkstationSetupModel.TransType;
                            //var Trtype1 = Session["TransType"].ToString();
                            //var Trtype = Session["TransType"].ToString();
                            _WorkstationSetupModel.TransType = Trtype;
                            // _ShopfloorSetupModel.shfl_id = shfl_id;
                            int brid = Convert.ToInt32(Session["BranchId"].ToString());

                            /*-------------------WorkStation Capacity------------------------*/

                            DataTable WorkstationCapacity1 = new DataTable();
                            DataTable WrkStation1 = new DataTable();

                            WorkstationCapacity1.Columns.Add("item_id", typeof(string));
                            WorkstationCapacity1.Columns.Add("uom_id", typeof(string));
                            WorkstationCapacity1.Columns.Add("optm_qty", typeof(string));
                            WorkstationCapacity1.Columns.Add("per_unit", typeof(string));

                            DataRow WrkStationCp = WorkstationCapacity1.NewRow();

                            //JArray JAObj = JArray.Parse(_WorkstationSetupModel.WorkstationCap);
                            //for (int i = 0; i < JAObj.Count; i++)
                            //{
                            //    DataRow dtrowLines = WrkStation1.NewRow();

                            //    dtrowLines["item_id"] = JAObj[i]["item_id"].ToString();
                            //    dtrowLines["uom_id"] = JAObj[i]["uom_id"].ToString();
                            //    dtrowLines["optm_qty"] = JAObj[i]["CstNameId"].ToString();
                            //    dtrowLines["per_unit"] = JAObj[i]["per_unit"].ToString();

                            //    WrkStation1.Rows.Add(dtrowLines);
                            //}
                           

                            /*---------Attachments Section Start----------------*/
                            DataTable wrksAttachments1 = new DataTable();

                            wrksAttachments1.Columns.Add("id", typeof(string));
                            wrksAttachments1.Columns.Add("file_name", typeof(string));
                            wrksAttachments1.Columns.Add("file_path", typeof(string));
                            wrksAttachments1.Columns.Add("file_def", typeof(char));
                            wrksAttachments1.Columns.Add("comp_id", typeof(Int32));

                            DataRow drAttachments = wrksAttachments1.NewRow();
                            /*---------Attachments Section End----------------*/
                            string compid = Convert.ToInt32(_WorkstationSetupModel.comp_id).ToString();
                            string wrks_id = Convert.ToInt32(_WorkstationSetupModel.ws_id).ToString();

                            SaveMessage = _WorkstationSetup_ISERVICES.insertWorkStationDetail(comp_id, br_id, ws_id, ws_name, shfl_id, op_st_date, op_name, sr_no, create_id, Trtype, Make, Model_no, Grp_nm, mac_id, wrksAttachments1,WorkstationCapacity1);
                            string[] splitmsg = SaveMessage.Split('-');
                            string MessageWRKDel = splitmsg[0].ToString().Trim();
                            TempData["ListFilterData"] = _WorkstationSetupModel.ListFilterData1;
                            /*---------Attachments Section Start----------------*/
                            if ((!string.IsNullOrEmpty(wrks_id)) && (splitmsg[0] != "Used"))
                            {
                                getDocumentName(); /* To set Title*/
                                PageName = title.Replace(" ", "");
                                var other = new CommonController(_Common_IServices);
                                other.DeleteTempFile(compn_id+ brid, PageName, wrks_id, Server);
                            }
                            /*---------Attachments Section End----------------*/
                            if (MessageWRKDel == "Delete")
                            {
                                _WorkstationSetupModelDelete.Message = "Deleted";
                                _WorkstationSetupModelDelete.Command = "Refresh";
                                _WorkstationSetupModelDelete.TransType = "Refresh";
                                _WorkstationSetupModelDelete.BtnName = "BtnDelete";
                                ViewBag.Message = _WorkstationSetupModelDelete.Message;
                                //Session["Message"] = "Deleted";
                                //Session["Command"] = "Refresh";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnDelete";
                                //ViewBag.Message = Session["Message"].ToString();
                                TempData["ModelData"] = _WorkstationSetupModelDelete;
                                //_WorkstationSetupModel = null;
                                return RedirectToAction("WorkstationSetupDetail");
                            }
                            else if(MessageWRKDel == "Used")
                            {
                                var Wsid = splitmsg[1];
                                _WorkstationSetupModelDelete.Message = "Used";
                                _WorkstationSetupModelDelete.Command = "View";
                                _WorkstationSetupModelDelete.TransType = "Update";
                                _WorkstationSetupModelDelete.BtnName = "BtnEdit";
                                _WorkstationSetupModelDelete.ws_id = Convert.ToUInt16(Wsid);
                                UrlModelData UrlDelete = new UrlModelData();
                                UrlDelete.TransType = _WorkstationSetupModelDelete.TransType;
                                UrlDelete.BtnName = _WorkstationSetupModelDelete.BtnName;
                                UrlDelete.Command = _WorkstationSetupModelDelete.Command;
                                UrlDelete.ws_id = _WorkstationSetupModelDelete.ws_id;

                                TempData["ModelData"] = _WorkstationSetupModelDelete;
                                //Session["Message"] = "Used";
                                //Session["Command"] = "View";
                                //Session["TransType"] = "Update";
                                return RedirectToAction("WorkstationSetupDetail", UrlDelete);
                            }
                            else
                            {
                                TempData["ModelData"] = _WorkstationSetupModel;
                                return RedirectToAction("WorkstationSetupDetail");
                            }
                        case "Save":
                            //Session["Command"] = command;
                            UrlModelData UrlDataSave = new UrlModelData();
                            _WorkstationSetupModel.Command = command;
                            //Trtype = Session["TransType"].ToString();
                            Trtype = _WorkstationSetupModel.TransType;
                            _WorkstationSetupModel.TransType = Trtype;
                            branch_id = Convert.ToInt32(_WorkstationSetupModel.br_id).ToString();
                            /*-------------------WorkStation Capacity------------------------*/

                            DataTable WorkstationCapacity = new DataTable();
                            DataTable WrkStation = new DataTable();

                            WrkStation.Columns.Add("item_id", typeof(string));
                            WrkStation.Columns.Add("uom_id", typeof(string));
                            WrkStation.Columns.Add("optm_qty", typeof(string));
                            WrkStation.Columns.Add("per_unit", typeof(string));



                            JArray JAObj = JArray.Parse(_WorkstationSetupModel.WorkstationCap);
                            for (int i = 0; i < JAObj.Count; i++)
                            {
                                DataRow dtrowLines = WrkStation.NewRow();

                                dtrowLines["item_id"] = JAObj[i]["item_id"].ToString();
                                dtrowLines["uom_id"] = JAObj[i]["uom_id"].ToString();
                                dtrowLines["optm_qty"] = JAObj[i]["optm_qty"].ToString();
                                dtrowLines["per_unit"] = JAObj[i]["per_unit"].ToString();

                                WrkStation.Rows.Add(dtrowLines);
                            }
                            WorkstationCapacity = WrkStation;
                            ViewData["WorkstationProdCap"] = WorkStaionCapacity(JAObj);

                            /*-------------------WorkStation Capacity End------------------------*/

                            /*-----------------Attachment Section Start------------------------*/
                            DataTable WRKSAttachments = new DataTable();
                            DataTable wrkdtAttachment = new DataTable();
                            var _WorkstationSetupAttchModel = TempData["ModelDataattch"] as WorkstationSetupAttchModel;
                            TempData["ModelDataattch"] = null;
                            if (_WorkstationSetupModel.attatchmentdetail != null)
                            {
                                if (_WorkstationSetupAttchModel != null)
                                {
                                    if (_WorkstationSetupAttchModel.AttachMentDetailItmStp != null)
                                    {
                                        wrkdtAttachment = _WorkstationSetupAttchModel.AttachMentDetailItmStp as DataTable;
                                    }
                                    else
                                    {
                                        wrkdtAttachment.Columns.Add("id", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_name", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_path", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_def", typeof(char));
                                        wrkdtAttachment.Columns.Add("comp_id", typeof(Int32));

                                    }
                                }
                                else
                                {
                                    if (_WorkstationSetupModel.AttachMentDetailItmStp != null)
                                    {
                                        wrkdtAttachment = _WorkstationSetupModel.AttachMentDetailItmStp as DataTable;
                                    }
                                    else
                                    {
                                        wrkdtAttachment.Columns.Add("id", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_name", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_path", typeof(string));
                                        wrkdtAttachment.Columns.Add("file_def", typeof(char));
                                        wrkdtAttachment.Columns.Add("comp_id", typeof(Int32));

                                    }
                                }
                                JArray jObject1 = JArray.Parse(_WorkstationSetupModel.attatchmentdetail);
                                for (int i = 0; i < jObject1.Count; i++)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in wrkdtAttachment.Rows)
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

                                        DataRow dtrowAttachment1 = wrkdtAttachment.NewRow();
                                        if (!string.IsNullOrEmpty((_WorkstationSetupModel.ws_id).ToString()))
                                        {
                                            dtrowAttachment1["id"] = _WorkstationSetupModel.ws_id;
                                        }
                                        else
                                        {
                                            dtrowAttachment1["id"] = "0";
                                        }
                                        dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                        dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                        dtrowAttachment1["file_def"] = "Y";
                                        dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                        wrkdtAttachment.Rows.Add(dtrowAttachment1);
                                    }
                                }
                                /** Commented by satya Veer on 12-10-2023**/
                                //if (Session["TransType"].ToString() == "Update")
                                //if (_WorkstationSetupModel.TransType == "Update")
                                //{
                                //    //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                //    //string br_id = Session["BranchId"].ToString();
                                //   // _WorkstationSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                //    string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");

                                //    if (Directory.Exists(AttachmentFilePath))
                                //    {
                                //        string WRKS_CODE = string.Empty;
                                //        if (!string.IsNullOrEmpty((_WorkstationSetupModel.ws_id).ToString()))
                                //        {
                                //            WRKS_CODE = (_WorkstationSetupModel.ws_id).ToString();

                                //        }
                                //        else
                                //        {
                                //            WRKS_CODE = "0";
                                //        }
                                //        string[] filePaths = Directory.GetFiles(AttachmentFilePath, compn_id + branch_id + WRKS_CODE.Replace("/", "") + "*");

                                //        foreach (var fielpath in filePaths)
                                //        {
                                //            string flag = "Y";
                                //            foreach (DataRow dr in wrkdtAttachment.Rows)
                                //            {
                                //                string drImgPath = dr["file_path"].ToString();
                                //                if (drImgPath == fielpath.Replace("/",@"\"))
                                //                {
                                //                    flag = "N";
                                //                }
                                //            }
                                //            if (flag == "Y")
                                //            {
                                //                System.IO.File.Delete(fielpath);
                                //            }
                                //        }
                                //    }
                                //}
                                WRKSAttachments = wrkdtAttachment;
                            }
                            /*-----------------Attachment Section End------------------------*/


                            //Message = _WorkstationSetup_ISERVICES.insertWorkStationDetail(comp_id, br_id, ws_id, ws_name, shfl_id, op_st_date, op_name, sr_no, create_id, Trtype, Make,Model_no,Grp_nm,mac_id);
                            //splitmsg = Message.Split('-');
                            SaveMessage = _WorkstationSetup_ISERVICES.insertWorkStationDetail(comp_id, br_id, ws_id, ws_name, shfl_id, op_st_date, op_name, sr_no, create_id, Trtype, Make, Model_no, Grp_nm, mac_id, WRKSAttachments,WorkstationCapacity);
                            splitmsg = SaveMessage.Split('-');
                            string WRKSCode = splitmsg[1].ToString().Trim();
                            string MessageWRK = splitmsg[0].ToString().Trim();

                            //if(MessageWRK == "Duplicate")
                            //{
                            //    string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //}
                            
                            /*-----------------Attachment Section Start------------------------*/
                            if (MessageWRK == "Save")

                            {
                               
                                string Guid = "";
                                if (_WorkstationSetupAttchModel != null)
                                {
                                    if (_WorkstationSetupAttchModel.Guid != null)
                                    {
                                        //Guid = Session["Guid"].ToString();
                                        Guid = _WorkstationSetupAttchModel.Guid;
                                    }
                                }
                                string guid = Guid;
                                var comCont = new CommonController(_Common_IServices);
                                comCont.ResetImageLocation(CompID, branch_id, guid, PageName, WRKSCode, _WorkstationSetupModel.TransType, WRKSAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, compn_id + branch_id + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in WRKSAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = compn_id + branch_id + WRKSCode + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            /*-----------------Attachment Section End------------------------*/

                            //if (splitmsg[0].ToString().Trim() == "Update" || splitmsg[0].ToString().Trim() == "Save")
                            TempData["ListFilterData"] = _WorkstationSetupModel.ListFilterData1;
                            if (MessageWRK == "Update" || MessageWRK == "Save")
                            {

                                if (_WorkstationSetupModel.TransType == "Update")
                                {
                                    //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                    //string br_id = Session["BranchId"].ToString();
                                    // _WorkstationSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                    string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";

                                    if (Directory.Exists(AttachmentFilePath))
                                    {
                                        string WRKS_CODE = string.Empty;
                                        if (!string.IsNullOrEmpty((_WorkstationSetupModel.ws_id).ToString()))
                                        {
                                            WRKS_CODE = (_WorkstationSetupModel.ws_id).ToString();

                                        }
                                        else
                                        {
                                            WRKS_CODE = "0";
                                        }
                                        string[] filePaths = Directory.GetFiles(AttachmentFilePath, compn_id + branch_id + WRKS_CODE.Replace("/", "") + "*");

                                        foreach (var fielpath in filePaths)
                                        {
                                            string flag = "Y";
                                            foreach (DataRow dr in wrkdtAttachment.Rows)
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

                                _WorkstationSetupModel.Message = "Save";
                                _WorkstationSetupModel.Command = "EditNew";
                                _WorkstationSetupModel.TransType = "Update";
                                _WorkstationSetupModel.BtnName = "BtnSave";
                                ViewBag.Message = _WorkstationSetupModel.Message.ToString();
                                _WorkstationSetupModel.SaveUpd = "AfterSaveUpdate";
                                _WorkstationSetupModel.ws_id =Convert.ToInt32(WRKSCode);
                                //Session["Message"] = "Save";
                                //Session["Command"] = "EditNew";
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnSave";
                                //ViewBag.Message = Session["Message"].ToString();
                                //Session["SaveUpd"] = "AfterSaveUpdate";                               
                                //Session["ws_id"] = WRKSCode;
                                Session["br_id"] = Session["BranchId"].ToString();
                                //Session["ws_id"] = splitmsg[1].ToString().Trim();
                                UrlDataSave.TransType = _WorkstationSetupModel.TransType;
                                UrlDataSave.BtnName = _WorkstationSetupModel.BtnName;
                                UrlDataSave.Command = _WorkstationSetupModel.Command;
                                UrlDataSave.ws_id = _WorkstationSetupModel.ws_id;
                                TempData["ModelData"] = _WorkstationSetupModel;
                                //_WorkstationSetupModel = null;
                                //_ShopfloorSetupModel.shfl_name
                                return RedirectToAction("WorkstationSetupDetail", UrlDataSave);
                            }
                            /** Commented by satya Veer on 12-10-2023**/
                            //else if (MessageWRK == "Duplicate")
                            // {
                            //     _WorkstationSetupModel.DupName = ws_name;
                            //     _WorkstationSetupModel.Message = "Duplicate";
                            //     ViewBag.Message = _WorkstationSetupModel.Message;
                            //     _WorkstationSetupModel.Command = "Edit";
                            //     // _WorkstationSetupModel = null;
                            //     //Session["DupName"] = ws_name;
                            //     //Session["Message"] = "Duplicate";
                            //     //ViewBag.Message = Session["Message"].ToString();
                            //     //Session["Command"] = "Edit";
                            //     UrlDataSave.TransType = _WorkstationSetupModel.TransType;
                            //     UrlDataSave.BtnName = _WorkstationSetupModel.BtnName;
                            //     UrlDataSave.Command = _WorkstationSetupModel.Command;
                            //     UrlDataSave.ws_id = _WorkstationSetupModel.ws_id;
                            //     TempData["ModelData"] = _WorkstationSetupModel;
                            //     TempData["WorkstationProdCap"] = ViewData["WorkstationProdCap"];
                            //     return RedirectToAction("WorkstationSetupDetailBeforeSave", _WorkstationSetupModel);
                            // }
                            else if (MessageWRK == "Duplicate")
                            {
                                if (_WorkstationSetupModel.TransType == "Update")
                                {
                                    //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                    //string br_id = Session["BranchId"].ToString();
                                    // _WorkstationSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                                    string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";

                                    if (Directory.Exists(AttachmentFilePath))
                                    {
                                        string WRKS_CODE = string.Empty;
                                        if (!string.IsNullOrEmpty((_WorkstationSetupModel.ws_id).ToString()))
                                        {
                                            WRKS_CODE = (_WorkstationSetupModel.ws_id).ToString();

                                        }
                                        else
                                        {
                                            WRKS_CODE = "0";
                                        }
                                        string[] filePaths = Directory.GetFiles(AttachmentFilePath, compn_id + branch_id + WRKS_CODE.Replace("/", "") + "*");
                                        TempData["Attachment"] = filePaths;
                                        foreach (var fielpath in filePaths)
                                        {
                                            string flag = "Y";
                                            //string flag = "N";
                                            foreach (DataRow dr in wrkdtAttachment.Rows)
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
                                
                                _WorkstationSetupModel.DupName = ws_name;
                                _WorkstationSetupModel.Message = "Duplicate";
                                ViewBag.Message = _WorkstationSetupModel.Message;
                                _WorkstationSetupModel.Command = "Edit";
                                _WorkstationSetupModel.BtnName = "BtnAddNew";
                                // _WorkstationSetupModel = null;
                                //Session["DupName"] = ws_name;
                                //Session["Message"] = "Duplicate";
                                //ViewBag.Message = Session["Message"].ToString();
                                //Session["Command"] = "Edit";
                                UrlDataSave.TransType = _WorkstationSetupModel.TransType;
                                UrlDataSave.BtnName = _WorkstationSetupModel.BtnName;
                                UrlDataSave.Command = _WorkstationSetupModel.Command;
                                UrlDataSave.ws_id = _WorkstationSetupModel.ws_id;
                                TempData["ModelData"] = _WorkstationSetupModel;
                                TempData["WorkstationProdCap"] = ViewData["WorkstationProdCap"];
                                return RedirectToAction("WorkstationSetupDetail", _WorkstationSetupModel);
                            }
                            else if (MessageWRK == "DuplicateWS")
                            {
                                _WorkstationSetupModel.DupName = ws_name;
                                _WorkstationSetupModel.Message = "DuplicateWS";
                                _WorkstationSetupModel.Command = "Edit";
                                UrlDataSave.TransType = _WorkstationSetupModel.TransType;
                                UrlDataSave.BtnName = _WorkstationSetupModel.BtnName;
                                UrlDataSave.Command = _WorkstationSetupModel.Command;
                                UrlDataSave.ws_id = _WorkstationSetupModel.ws_id;
                                TempData["ModelData"] = _WorkstationSetupModel;
                                //Session["DupName"] = ws_name;
                                //Session["Message"] = "DuplicateWS";
                                //Session["Command"] = "Edit";
                                return RedirectToAction("WorkstationSetupDetailBeforeSave", _WorkstationSetupModel);
                            }
                            UrlDataSave.TransType = _WorkstationSetupModel.TransType;
                            UrlDataSave.BtnName = _WorkstationSetupModel.BtnName;
                            UrlDataSave.Command = _WorkstationSetupModel.Command;
                            UrlDataSave.ws_id = _WorkstationSetupModel.ws_id;
                            TempData["ModelData"] = _WorkstationSetupModel;
                            return RedirectToAction("WorkstationSetupDetail", UrlDataSave);
                        case "Refresh":
                            WorkstationSetupModel _WorkstationSetupModelRefresh = new WorkstationSetupModel();
                            _WorkstationSetupModelRefresh.BtnName = "BtnRefresh";
                            _WorkstationSetupModelRefresh.Command = command;
                            _WorkstationSetupModelRefresh.TransType = "Refresh";
                            TempData["ModelData"] = _WorkstationSetupModelRefresh;
                            //Session["BtnName"] = "BtnRefresh";
                            //Session["Command"] = command;
                            //Session["TransType"] = "Refresh";
                            //Session["Message"] = "";
                            //Session["AppStatus"] = "";
                            //Session.Remove("ws_id");
                            //Session.Remove("SaveUpd");
                            //Session.Remove("shfl_name");
                            //Session.Remove("shfl_loc");
                            TempData["ListFilterData"] = _WorkstationSetupModel.ListFilterData1;
                            //_WorkstationSetupModel = null;
                            return RedirectToAction("WorkstationSetupDetail");
                        case "BacktoList":
                            //Session.Remove("Message");
                            //Session.Remove("TransType");
                            //Session.Remove("Command");
                            //Session.Remove("BtnName");
                            //Session.Remove("DocumentStatus");
                            //Session.Remove("ws_id");
                            TempData["ListFilterData"] = _WorkstationSetupModel.ListFilterData1;
                            _WorkstationSetupModel = null;
                            return RedirectToAction("WorkstationSetup", "WorkstationSetup");
                    }
                }
                else
                {
                    RedirectToAction("");
                }
                return RedirectToAction("WorkstationSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                            if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_WorkstationSetupModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_WorkstationSetupModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _WorkstationSetupModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(compn_id+branch_id, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public DataTable WorkStaionCapacity(JArray JAObj)
        {
            DataTable WrkStation = new DataTable();
            WrkStation.Columns.Add("item_id", typeof(string));
            WrkStation.Columns.Add("item_name", typeof(string));
            WrkStation.Columns.Add("uom_alias", typeof(string));
            WrkStation.Columns.Add("uom_id", typeof(string));
            WrkStation.Columns.Add("optm_qty", typeof(string));
            WrkStation.Columns.Add("per_unit", typeof(string));




            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = WrkStation.NewRow();

                dtrowLines["item_id"] = JAObj[i]["item_id"].ToString();
                dtrowLines["item_name"] = JAObj[i]["item_name"].ToString();
                dtrowLines["uom_alias"] = JAObj[i]["uom_alias"].ToString();
                dtrowLines["uom_id"] = JAObj[i]["uom_id"].ToString();
                dtrowLines["optm_qty"] = JAObj[i]["optm_qty"].ToString();
                dtrowLines["per_unit"] = JAObj[i]["per_unit"].ToString();

                WrkStation.Rows.Add(dtrowLines);
            }
            return WrkStation;
        }
        public ActionResult WorkstationSetupDetailBeforeSave(WorkstationSetupModel _WorkstationSetupModel)
        {
            try
            {
                //WorkstationSetupModel _WorkstationSetupModel = new WorkstationSetupModel();
                CommonPageDetails();
                if (Session["compid"] != null)
                {
                    //BindShopFloorList();
                    BindShopFloorList(_WorkstationSetupModel);
                    //ViewBag.MenuPageName = getDocumentName();
                    _WorkstationSetupModel.Title = title;
                    //if (Session["BtnName"] == null)
                    //{
                    //    if (Session["BtnName"].ToString() == "")
                    //    {
                    //        Session["TransType"] = "Refresh";
                    //        Session["BtnName"] = "BtnNew";
                    //        Session["Command"] = "Add";
                    //    }
                    //}
                    if (_WorkstationSetupModel.BtnName == null)
                    {
                        if (_WorkstationSetupModel.BtnName == "")
                        {
                            _WorkstationSetupModel.TransType = "Refresh";
                            _WorkstationSetupModel.BtnName = "BtnAddNew";
                            _WorkstationSetupModel.Command = "Add";
                        }
                        else
                        {
                            _WorkstationSetupModel.BtnName = "BtnAddNew";
                        }
                    }
                    //if(Session["dbclick"] !=null)
                    //{
                    Int32 comp_id = Convert.ToInt32(Session["compid"].ToString());
                    Int32 br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    Int32 ws_id = 0;
                    //if (Session["ws_id"] != null)
                    if (_WorkstationSetupModel.ws_id != 0)
                    {
                        //ws_id = Convert.ToInt32(Session["ws_id"].ToString());
                        ws_id = Convert.ToInt32(_WorkstationSetupModel.ws_id);
                    }
                    //Session.Remove("dbclick");
                    //}
                }
                ViewBag.WorkStationCapacity = TempData["WorkstationProdCap"];
                GetAutoCompleteSearchSuggestion(_WorkstationSetupModel);
                _WorkstationSetupModel.DocumentMenuId = DocumentMenuId;
                //return View(_WorkstationSetupModel);
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupDetail.cshtml", _WorkstationSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult WorkstationSetupDetail(UrlModelData UrlData)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                var _WorkstationSetupModel = TempData["ModelData"] as WorkstationSetupModel;
                if (_WorkstationSetupModel != null)
                {
                    CommonPageDetails();
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _WorkstationSetupModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (Session["compid"] != null)
                    {                    
                        //BindShopFloorList();
                        BindShopFloorList(_WorkstationSetupModel);
                        //ViewBag.MenuPageName = getDocumentName();
                        _WorkstationSetupModel.Title = title;
                        //if (Session["BtnName"] == null)
                        if (_WorkstationSetupModel.BtnName == null)
                        {
                            //if (Session["BtnName"].ToString() == "")
                            if (_WorkstationSetupModel.BtnName == "")
                            {
                                _WorkstationSetupModel.TransType = "Refresh";
                                _WorkstationSetupModel.BtnName = "BtnNew";
                                _WorkstationSetupModel.Command = "Add";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnNew";
                                //Session["Command"] = "Add";
                            }
                        }
                        Int32 comp_id = Convert.ToInt32(Session["compid"].ToString());
                        Int32 br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        Int32 ws_id = 0;
                        //if (Session["ws_id"] != null)
                        if (_WorkstationSetupModel.ws_id != 0)
                        {
                            //ws_id = Convert.ToInt32(Session["ws_id"].ToString());
                            ws_id = Convert.ToInt32(_WorkstationSetupModel.ws_id);
                        }
                        DataSet dt = _WorkstationSetup_ISERVICES.GetWSDoubleClickEdit(comp_id, br_id, ws_id);
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            //if (Session["Message"] != null)
                            if (_WorkstationSetupModel.Message != null)
                            {
                                //if (Session["Message"].ToString() == "Duplicate" || Session["Message"].ToString() == "DuplicateWS")
                                if (_WorkstationSetupModel.Message == "Duplicate" || _WorkstationSetupModel.Message == "DuplicateWS")
                                {
                                    //_WorkstationSetupModel.ws_name = Session["DupName"].ToString();
                                    _WorkstationSetupModel.ws_name = _WorkstationSetupModel.DupName;
                                    //Session.Remove("DupName");
                                }
                                else
                                {
                                    _WorkstationSetupModel.ws_name = Convert.ToString(dt.Tables[0].Rows[0]["ws_name"]);
                                }
                            }
                            else
                            {
                                _WorkstationSetupModel.ws_name = Convert.ToString(dt.Tables[0].Rows[0]["ws_name"]);
                            }
                            
                            _WorkstationSetupModel.shfl_id = Convert.ToInt32(dt.Tables[0].Rows[0]["shfl_id"]);
                            _WorkstationSetupModel.Status = dt.Tables[0].Rows[0]["shfl_id"].ToString();
                            _WorkstationSetupModel.ws_id = Convert.ToInt32(dt.Tables[0].Rows[0]["ws_id"]);
                            _WorkstationSetupModel.op_st_date = Convert.ToString(dt.Tables[0].Rows[0]["op_st_date"]);
                            _WorkstationSetupModel.create_name = Convert.ToString(dt.Tables[0].Rows[0]["created_id"]);
                            _WorkstationSetupModel.create_dt = Convert.ToString(dt.Tables[0].Rows[0]["create_dt"]);
                            _WorkstationSetupModel.mod_name = Convert.ToString(dt.Tables[0].Rows[0]["mod_id"]);
                            _WorkstationSetupModel.mod_dt = Convert.ToString(dt.Tables[0].Rows[0]["mod_dt"]);
                            _WorkstationSetupModel.DeleteCommand = Convert.ToString(dt.Tables[0].Rows[0]["ws_id"]);
                            /** Changed by satya Veer on 12-10-2023**/
                            if (_WorkstationSetupModel.Message != "Duplicate")
                            {
                                _WorkstationSetupModel.sr_no = Convert.ToString(dt.Tables[0].Rows[0]["sr_no"]);
                                _WorkstationSetupModel.op_name = Convert.ToString(dt.Tables[0].Rows[0]["op_name"]);
                                _WorkstationSetupModel.Make = Convert.ToString(dt.Tables[0].Rows[0]["make"]);
                                _WorkstationSetupModel.Model_no = Convert.ToString(dt.Tables[0].Rows[0]["model_no"]);
                            }
                            


                        }
                        if (_WorkstationSetupModel.Message != "Duplicate")
                        {
                            ViewBag.WorkStationCapacity = dt.Tables[2];
                        }
                        else
                        {
                            ViewBag.WorkStationCapacity = TempData["WorkstationProdCap"];
                        }
                        ViewBag.AttechmentDetails = dt.Tables[1];
                        //if (_WorkstationSetupModel.Message != "Duplicate")
                        //{
                        //    ViewBag.AttechmentDetails = dt.Tables[1];
                        //}
                        //else
                        //{
                        //    ViewBag.AttechmentDetails = (TempData["Attachment"]);
                        //}

                    }
                    GetAutoCompleteSearchSuggestion(_WorkstationSetupModel);
                    _WorkstationSetupModel.DocumentMenuId = DocumentMenuId;
                    //return View(_WorkstationSetupModel);
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupDetail.cshtml", _WorkstationSetupModel);
                }
                else
                {
                    WorkstationSetupModel _WorkstationSetupModel1 = new WorkstationSetupModel();
                    CommonPageDetails();
                    if (UrlData.TransType != null)
                    {
                        _WorkstationSetupModel1.TransType = UrlData.TransType;
                    }
                    else
                    {
                        _WorkstationSetupModel1.TransType = "Refresh";
                    }
                    if (UrlData.BtnName != null)
                    {
                        _WorkstationSetupModel1.BtnName = UrlData.BtnName;
                    }
                    else
                    {
                        _WorkstationSetupModel1.BtnName="BtnRefresh";
                    }
                    if (UrlData.Command != null)
                    {
                        _WorkstationSetupModel1.Command = UrlData.Command;
                    }
                    else
                    {
                        _WorkstationSetupModel1.Command = "Refresh";
                    }
                    if (UrlData.ws_id != 0)
                    {
                        _WorkstationSetupModel1.ws_id = UrlData.ws_id;
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _WorkstationSetupModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (Session["compid"] != null)
                    {
                        //BindShopFloorList();
                        BindShopFloorList(_WorkstationSetupModel1);
                        //ViewBag.MenuPageName = getDocumentName();
                        _WorkstationSetupModel1.Title = title;
                        //if (Session["BtnName"] == null)
                        if (_WorkstationSetupModel1.BtnName == null)
                        {
                            //if (Session["BtnName"].ToString() == "")
                            if (_WorkstationSetupModel1.BtnName == "")
                            {
                                _WorkstationSetupModel1.TransType = "Refresh";
                                _WorkstationSetupModel1.BtnName = "BtnNew";
                                _WorkstationSetupModel1.Command = "Add";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnNew";
                                //Session["Command"] = "Add";
                            }
                        }
                        Int32 comp_id = Convert.ToInt32(Session["compid"].ToString());
                        Int32 br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        Int32 ws_id = 0;
                        //if (Session["ws_id"] != null)
                        if (_WorkstationSetupModel1.ws_id != 0)
                        {
                            //ws_id = Convert.ToInt32(Session["ws_id"].ToString());
                            ws_id = Convert.ToInt32(_WorkstationSetupModel1.ws_id);
                        }
                        DataSet dt = _WorkstationSetup_ISERVICES.GetWSDoubleClickEdit(comp_id, br_id, ws_id);
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            //if (Session["Message"] != null)
                            if (_WorkstationSetupModel1.Message != null)
                            {
                                //if (Session["Message"].ToString() == "Duplicate" || Session["Message"].ToString() == "DuplicateWS")
                                if (_WorkstationSetupModel1.Message == "Duplicate" || _WorkstationSetupModel1.Message == "DuplicateWS")
                                {
                                    //_WorkstationSetupModel.ws_name = Session["DupName"].ToString();
                                    _WorkstationSetupModel1.ws_name = _WorkstationSetupModel1.DupName;
                                    //Session.Remove("DupName");
                                }
                                else
                                {
                                    _WorkstationSetupModel1.ws_name = Convert.ToString(dt.Tables[0].Rows[0]["ws_name"]);
                                }
                            }
                            else
                            {
                                _WorkstationSetupModel1.ws_name = Convert.ToString(dt.Tables[0].Rows[0]["ws_name"]);
                            }
                            //_WorkstationSetupModel1.ws_name = Convert.ToString(dt.Tables[0].Rows[0]["ws_name"]);
                            _WorkstationSetupModel1.shfl_id = Convert.ToInt32(dt.Tables[0].Rows[0]["shfl_id"]);
                            _WorkstationSetupModel1.Status = dt.Tables[0].Rows[0]["shfl_id"].ToString();
                            _WorkstationSetupModel1.ws_id = Convert.ToInt32(dt.Tables[0].Rows[0]["ws_id"]);
                            _WorkstationSetupModel1.op_name = Convert.ToString(dt.Tables[0].Rows[0]["op_name"]);
                            _WorkstationSetupModel1.op_st_date = Convert.ToString(dt.Tables[0].Rows[0]["op_st_date"]);
                            _WorkstationSetupModel1.sr_no = Convert.ToString(dt.Tables[0].Rows[0]["sr_no"]);
                            _WorkstationSetupModel1.create_name = Convert.ToString(dt.Tables[0].Rows[0]["created_id"]);
                            _WorkstationSetupModel1.create_dt = Convert.ToString(dt.Tables[0].Rows[0]["create_dt"]);
                            _WorkstationSetupModel1.mod_name = Convert.ToString(dt.Tables[0].Rows[0]["mod_id"]);
                            _WorkstationSetupModel1.mod_dt = Convert.ToString(dt.Tables[0].Rows[0]["mod_dt"]);
                            _WorkstationSetupModel1.DeleteCommand = Convert.ToString(dt.Tables[0].Rows[0]["ws_id"]);
                            _WorkstationSetupModel1.Make = Convert.ToString(dt.Tables[0].Rows[0]["make"]);
                            _WorkstationSetupModel1.Model_no = Convert.ToString(dt.Tables[0].Rows[0]["model_no"]);
                            //_WorkstationSetupModel1.Group_name = Convert.ToString(dt.Tables[0].Rows[0]["item_grp_id"]);

                            /** Changed by satya Veer on 12-10-2023**/
                        }
                        if (_WorkstationSetupModel1.Message != "Duplicate")
                        {
                            ViewBag.WorkStationCapacity = dt.Tables[2];
                        }
                        else
                        {
                            ViewBag.WorkStationCapacity = TempData["WorkstationProdCap"];
                        }

                        ViewBag.AttechmentDetails = dt.Tables[1];

                    }
                    GetAutoCompleteSearchSuggestion(_WorkstationSetupModel1);
                    _WorkstationSetupModel1.DocumentMenuId = DocumentMenuId;
                    //return View(_WorkstationSetupModel1);
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/WorkstationSetup/WorkstationSetupDetail.cshtml", _WorkstationSetupModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult dbClickEdit(Int32 comp_id, Int32 br_id, Int32 ws_id, string ListFilterData)
        {
            //JsonResult DataRows = null;
            WorkstationSetupModel _WorkstationSetupModel = new WorkstationSetupModel();
            UrlModelData UrlData = new UrlModelData();
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branch_id = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                {
                    TempData["Message1"] = "Financial Year not Exist";
                }
                /*End to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                {
                  
                    _WorkstationSetupModel.Command = "View";
                    _WorkstationSetupModel.TransType = "EditNew";
                    _WorkstationSetupModel.BtnName = "BtnEdit";
                    _WorkstationSetupModel.TransType = "Update";
                    _WorkstationSetupModel.br_id = br_id;
                    _WorkstationSetupModel.ws_id = ws_id;
                    _WorkstationSetupModel.SaveUpd = "0";
                    _WorkstationSetupModel.dbclick = "dbclick";

                    
                    UrlData.Command= "View";
                    UrlData.TransType= "Update";
                    UrlData.BtnName= "BtnEdit";
                    UrlData.ws_id = ws_id;
                    UrlData.dbclick = "dbclick";

                  
                    string Comp_ID = Session["CompId"].ToString();
                    TempData["ModelData"] = _WorkstationSetupModel;
                    TempData["ListFilterData"] = ListFilterData;
                   
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                return RedirectToAction("WorkstationSetupDetail", UrlData);
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public void BindShopFloorList(WorkstationSetupModel _WorkstationSetupModel)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    dt = _WorkstationSetup_ISERVICES.GetShopFloorDetailsDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id));
                    List<Status> _Status = new List<Status>();
                    if (dt.Rows.Count > 0)
                    {
                        //Status _Statuslist1 = new Status();/*commented by Hina on 13-09-2024 to add below out of table*/
                        //_Statuslist1.status_id = "0";
                        ////_Statuslist1.status_name = "ALL";
                        //_Statuslist1.status_name = "---Select---";
                        //_Status.Add(_Statuslist1);
                        foreach (DataRow data in dt.Rows)
                        {
                            Status _Statuslist = new Status();
                            _Statuslist.status_id = data["shfl_id"].ToString();
                            _Statuslist.status_name = data["shfl_name"].ToString();
                            _Status.Add(_Statuslist);
                        }
                    }
                    _Status.Insert(0, new Status() { status_id = "0", status_name = "---Select---" });/*Add by Hina on 13-09-2024*/

                    _WorkstationSetupModel.StatusList = _Status;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
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
        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                WorkstationSetupAttchModel _WorkstationSetupAttchModel = new WorkstationSetupAttchModel();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string WrksCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                    //TransType = Session["TransType"].ToString();
                //}
                //if (Session["ItemCode"] != null)
                //{
                //    ItemCode = Session["ItemCode"].ToString();
                //}

                //if (Session["ws_id"] != null)
                //{
                    //DocNo = Session["ws_id"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _WorkstationSetupAttchModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    compn_id = Session["CompId"].ToString();
                }
                
                branch_id = Session["BranchId"].ToString();
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, compn_id+ branch_id, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _WorkstationSetupAttchModel.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _WorkstationSetupAttchModel.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _WorkstationSetupAttchModel;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/
    }
}

