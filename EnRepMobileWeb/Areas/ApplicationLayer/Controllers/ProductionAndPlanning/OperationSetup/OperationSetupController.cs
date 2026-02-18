using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OperationSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OperationSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.OperationSetup
{
    public class OperationSetupController : Controller
    {
        // GET: ApplicationLayer/OperationSetup
        string comp_id;
        string DocumentMenuId = "105105110";
        string CompID, language, UserID, Br_ID,title = String.Empty;
        Common_IServices _Common_IServices;
        OperationSetup_ISERVICES _OperationSetup_ISERVICES;
        //OperationSetupModel _OperationSetupModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        public OperationSetupController(Common_IServices _Common_IServices,OperationSetup_ISERVICES _OperationSetup_ISERVICES)
        {
            this._OperationSetup_ISERVICES = _OperationSetup_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        public ActionResult OperationSetup(OperationSetupModel _OperationSetupModel,string op_name ,string op_type 
            ,string op_remarks,string Typ ,string cmd ,string bt,string op_id, string shfl_id, string wrkstn_id, string supervisor)
        {
            try
            {
                //if (Session["compid"] != null)
                //{
                    //if (Session["BtnName"] == null || Session["BtnName"].ToString() == "")
                    //{
                    //    Session["TransType"] = "Refresh";
                    //    Session["BtnName"] = "BtnNew";
                    //    Session["Command"] = "Add";
                    //}
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
                        Br_ID = Session["BranchId"].ToString();
                    }
              var OperationSetupModel1 =   TempData["Modeldata"] as OperationSetupModel;
                if (OperationSetupModel1 != null)
                {
                    if (Typ != null)
                    {
                        OperationSetupModel1.TransType = Typ;
                        OperationSetupModel1.Command = cmd;
                        OperationSetupModel1.BtnName = bt;
                    }

                    ViewBag.MenuPageName = getDocumentName();
                    //OperationSetupModel _OperationSetupModel = new OperationSetupModel();
                    OperationSetupModel1.Title = title;
                    //if (Session["op_name"] != null)
                    if (OperationSetupModel1.TransType == "Update" || OperationSetupModel1.Command == "Edit")
                    {
                        if (op_name != null)
                        {
                            OperationSetupModel1.op_name = op_name;
                            OperationSetupModel1.op_id = Convert.ToInt32(op_id);
                            OperationSetupModel1.op_remarks = op_remarks;
                            OperationSetupModel1.op_type = op_type;
                            if (OperationSetupModel1.Shopfloor == null && OperationSetupModel1.Shopfloor == "")
                            {
                                OperationSetupModel1.Shopfloor = shfl_id;
                            }
                            else
                            {
                                OperationSetupModel1.Shopfloor = OperationSetupModel1.Shopfloor;
                            }
                            if (OperationSetupModel1.Supervisor == null && OperationSetupModel1.Supervisor == "")
                            {
                                OperationSetupModel1.Supervisor = supervisor;
                            }
                            else
                            {
                                OperationSetupModel1.Supervisor = OperationSetupModel1.Supervisor;
                            }

                            BindWorkStationList(OperationSetupModel1, Convert.ToInt32(OperationSetupModel1.Shopfloor));
                            if (OperationSetupModel1.Workstation == null && OperationSetupModel1.Workstation == "")
                            {
                                OperationSetupModel1.Workstation = wrkstn_id;
                            }
                            else
                            {
                                OperationSetupModel1.Workstation = OperationSetupModel1.Workstation;
                            }
                        }
                        else
                        {
                            //OperationSetupModel1.op_name = Session["op_name"].ToString();
                            //OperationSetupModel1.op_type = Session["op_type_id"].ToString();
                            //OperationSetupModel1.op_id = Convert.ToInt32(Session["OP_ID"].ToString());
                            //OperationSetupModel1.op_remarks = Session["op_remarks"].ToString();
                            OperationSetupModel1.op_name = OperationSetupModel1.op_name;
                            OperationSetupModel1.op_type = OperationSetupModel1.op_type;
                            OperationSetupModel1.op_id = OperationSetupModel1.op_id;
                            OperationSetupModel1.op_remarks = OperationSetupModel1.op_remarks;
                            OperationSetupModel1.Shopfloor = OperationSetupModel1.Shopfloor;
                            OperationSetupModel1.Supervisor = OperationSetupModel1.Supervisor;
                            BindWorkStationList(OperationSetupModel1, Convert.ToInt32( OperationSetupModel1.Shopfloor));
                            OperationSetupModel1.Workstation = OperationSetupModel1.Workstation;
                        }
                      
                    }
                    else
                    {
                        List<Workstation> work = new List<Workstation>();
                        work.Insert(0, new Workstation() { wrk_id = "0", wrk_name = "---Select---" });
                        OperationSetupModel1.WorkstationList = work;
                    }
                   
                    if (OperationSetupModel1.BtnName == null && OperationSetupModel1.Command == null)
                    {
                        OperationSetupModel1.BtnName = "BtnNew";
                        OperationSetupModel1.TransType = "Refresh";
                        OperationSetupModel1.Command = "Add";
                      
                    }
                    ViewBag.BindOperationDetails = GetOperationDetails();
                    ViewBag.VBRoleList = GetRoleList();

                    List<ShopFloor> _shflList = new List<ShopFloor>();
                    DataTable dt = BindShopfloorList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ShopFloor _shflr = new ShopFloor();
                        _shflr.shfl_id = dr["shfl_id"].ToString();
                        _shflr.shfl_name = dr["shfl_name"].ToString();
                        _shflList.Add(_shflr);

                    }
                    _shflList.Insert(0, new ShopFloor() { shfl_id = "0", shfl_name = "---Select---" });
                    OperationSetupModel1.ShopFloorList = _shflList;

                    ////List<Supervisor> supp = new List<Supervisor>();
                    ////supp.Insert(0, new Supervisor() { super_id = "0", super_name = "---Select---" });
                    ////OperationSetupModel1.SupervisorList = supp;

                  

                    
                  

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OperationSetup/OperationSetupDetail.cshtml", OperationSetupModel1);
                }
                else
                { /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        Br_ID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    if (Typ != null && Typ!= "")
                    {
                        _OperationSetupModel.TransType = Typ;
                        _OperationSetupModel.Command = cmd;
                        _OperationSetupModel.BtnName = bt;
                    }

                    ViewBag.MenuPageName = getDocumentName();
                    //OperationSetupModel _OperationSetupModel = new OperationSetupModel();
                    _OperationSetupModel.Title = title;
                    //if (Session["op_name"] != null)
                    if (_OperationSetupModel.TransType == "Update" || _OperationSetupModel.Command == "Edit")
                    {
                        if (op_name != null)
                        {
                            _OperationSetupModel.op_name = op_name;
                            _OperationSetupModel.op_id = Convert.ToInt32(op_id);
                            _OperationSetupModel.op_remarks = op_remarks;
                            _OperationSetupModel.op_type = op_type;
                            if(_OperationSetupModel.Shopfloor == null && _OperationSetupModel.Shopfloor =="")
                            {
                                _OperationSetupModel.Shopfloor = shfl_id;
                            }
                            else
                            {
                                _OperationSetupModel.Shopfloor = _OperationSetupModel.Shopfloor;
                            }
                            if (_OperationSetupModel.Supervisor == null && _OperationSetupModel.Supervisor == "")
                            {
                                _OperationSetupModel.Supervisor = supervisor;
                            }
                            else
                            {
                                _OperationSetupModel.Supervisor = _OperationSetupModel.Supervisor;
                            }
                           
                            BindWorkStationList(_OperationSetupModel, Convert.ToInt32(_OperationSetupModel.Shopfloor));
                            if (_OperationSetupModel.Workstation == null && _OperationSetupModel.Workstation == "")
                            {
                                _OperationSetupModel.Workstation = wrkstn_id;
                            }
                            else
                            {
                                _OperationSetupModel.Workstation = _OperationSetupModel.Workstation;
                            }
                           
                        }
                        else
                        {
                            //_OperationSetupModel.op_name = Session["op_name"].ToString();
                            //_OperationSetupModel.op_type = Session["op_type_id"].ToString();
                            //_OperationSetupModel.op_id = Convert.ToInt32(Session["OP_ID"].ToString());
                            //_OperationSetupModel.op_remarks = Session["op_remarks"].ToString();
                            _OperationSetupModel.op_name = _OperationSetupModel.op_name;
                            _OperationSetupModel.op_type = _OperationSetupModel.op_type;
                            _OperationSetupModel.op_id = _OperationSetupModel.op_id;
                            _OperationSetupModel.op_remarks = _OperationSetupModel.op_remarks;
                            _OperationSetupModel.Shopfloor = _OperationSetupModel.Shopfloor;
                            _OperationSetupModel.Supervisor = _OperationSetupModel.Supervisor;
                            BindWorkStationList(_OperationSetupModel, Convert.ToInt32(_OperationSetupModel.Shopfloor));
                            _OperationSetupModel.Workstation = _OperationSetupModel.Workstation;

                        }
                     
                    }
                    else
                    {
                        List<Workstation> work = new List<Workstation>();
                        work.Insert(0, new Workstation() { wrk_id = "0", wrk_name = "---Select---" });
                        _OperationSetupModel.WorkstationList = work;
                    }
                   
                    if (_OperationSetupModel.BtnName == null && _OperationSetupModel.Command == null)
                    {
                        _OperationSetupModel.BtnName = "BtnNew";
                        _OperationSetupModel.TransType = "Refresh";
                        _OperationSetupModel.Command = "Add";
                    }
                    ViewBag.BindOperationDetails = GetOperationDetails();
                    ViewBag.VBRoleList = GetRoleList();

                    List<ShopFloor> _shflList = new List<ShopFloor>();
                  DataTable  dt = BindShopfloorList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ShopFloor _shflr = new ShopFloor();
                        _shflr.shfl_id = dr["shfl_id"].ToString();
                        _shflr.shfl_name = dr["shfl_name"].ToString();
                        _shflList.Add(_shflr);

                    }
                    _shflList.Insert(0, new ShopFloor() { shfl_id = "0", shfl_name = "---Select---" });
                    _OperationSetupModel.ShopFloorList = _shflList;
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OperationSetup/OperationSetupDetail.cshtml", _OperationSetupModel);
                }
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        [NonAction]
        private DataTable BindShopfloorList()
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataSet dt = _OperationSetup_ISERVICES.BindShopFloore(CompID, BrchID);
                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        //[NoDirectAccess]
        public ActionResult OperationSave(OperationSetupModel _OperationSetupModel,string command)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();


                    int op_id = _OperationSetupModel.op_id;
                    string op_name = _OperationSetupModel.op_name;
                    string op_type = _OperationSetupModel.op_type;
                    string op_remarks = _OperationSetupModel.op_remarks;
                    string shfl_id = _OperationSetupModel.Shopfloor;
                    string wrkstn_id = _OperationSetupModel.Workstation;
                    string supervisor = _OperationSetupModel.Supervisor;
                    var Typ = "";
                    var cmd = "";
                    var bt = "";
                   
                    //string op_remarks = _OperationSetupModel.h;
                    string create_id = Session["UserId"].ToString();
                   // string action = "";
                    //String SaveMessage = _OperationSetup_ISERVICES.insertOperationDetail(Convert.ToInt32( comp_id), op_id, op_name, op_type, op_remarks,Convert.ToInt32(create_id), action);
                    if (_OperationSetupModel.DeleteCommand == "Delete")
                    {
                        command = "Delete";
                    }
                    //start show and hide tools
                    switch (command)
                    {
                        case "Edit":
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                Br_ID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                return RedirectToAction("dbClickEdit", new { op_id1 = _OperationSetupModel.op_id, op_name = _OperationSetupModel.op_name, op_type_id = _OperationSetupModel.op_type, op_remarks = _OperationSetupModel.op_remarks, shfl_id= _OperationSetupModel.Shopfloor, wrkstn_id= _OperationSetupModel.Workstation, supervisor= _OperationSetupModel.Supervisor });
                            }
                           /*End to chk Financial year exist or not*/
                           //Session["Message"] = "";
                           //Session["Command"] = command;
                           //Session["OP_ID"] = _OperationSetupModel.op_id;
                           ////Session["TransType"] = "EditNew";
                           //Session["BtnName"] = "BtnAddNew";
                           //Session["TransType"] = "Update";
                           _OperationSetupModel.BtnName = "BtnAddNew";
                            _OperationSetupModel.TransType = "Update";
                            _OperationSetupModel.op_name = op_name;
                            _OperationSetupModel.op_type = op_type;
                            _OperationSetupModel.op_remarks = op_remarks;
                            _OperationSetupModel.op_id = op_id;
                            _OperationSetupModel.Shopfloor = shfl_id;
                            _OperationSetupModel.Workstation = wrkstn_id;
                            _OperationSetupModel.Shopfloor = shfl_id;
                            TempData["ModelData"] = _OperationSetupModel;
                            Typ = _OperationSetupModel.TransType;
                            cmd = _OperationSetupModel.Command;
                            bt = _OperationSetupModel.BtnName;
                            //_OperationSetupModel.op_name = Session["op_name"].ToString();
                            //_OperationSetupModel.op_type = Session["op_type_id"].ToString();
                            //_OperationSetupModel.op_id = Convert.ToInt32(Session["OP_ID"].ToString());
                            //_OperationSetupModel.op_remarks = Session["op_remarks"].ToString();
                            //String SaveMessage = _OperationSetup_ISERVICES.insertOperationDetail(Convert.ToInt32(comp_id), op_id, op_name, op_type, op_remarks, Convert.ToInt32(create_id), "Update");
                            return RedirectToAction("OperationSetup", new { op_id, op_name, op_type, op_remarks, Typ, cmd, bt, shfl_id, wrkstn_id, supervisor });

                        case "Add":/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            OperationSetupModel adddnew = new OperationSetupModel();
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                Br_ID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";

                                if (_OperationSetupModel.op_id != 0)
                                    return RedirectToAction("dbClickEdit", new { op_id1 = _OperationSetupModel.op_id, op_name = _OperationSetupModel.op_name, op_type_id = _OperationSetupModel.op_type, op_remarks = _OperationSetupModel.op_remarks, shfl_id = _OperationSetupModel.Shopfloor, wrkstn_id = _OperationSetupModel.Workstation, supervisor = _OperationSetupModel.Supervisor });
                                else
                                    adddnew.Command = "Refresh";
                                adddnew.TransType = "Refresh";
                                adddnew.BtnName = "BtnRefresh";
                                TempData["ModelData"] = adddnew;
                                return RedirectToAction("OperationSetup", adddnew);
                            }
                            /*End to chk Financial year exist or not*/
                            //Session["Message"] = "";
                            //Session["Command"] = command;
                            //Session["TaxCode"] = "";
                            ////Session["AppStatus"] = "D";
                            ////_TaxDetailModel = null;
                            //Session["TransType"] = "Save";
                            //Session["BtnName"] = "BtnAddNew";
                            
                            adddnew.Command = "Add";
                            adddnew.TransType = "Save";
                            adddnew.BtnName = "BtnAddNew";
                            TempData["ModelData"] = adddnew;
                            Typ = adddnew.TransType;
                            cmd = adddnew.Command;
                            bt = adddnew.BtnName;
                            
                            return RedirectToAction("OperationSetup", new { Typ, cmd, bt });

                        case "Delete":
                            //Session["Command"] = command;
                            //Session["TransType"] = "Delete";
                            // var Trtype1 = Session["TransType"].ToString();
                            var Trtype1 = command;
                            if (ModelState.IsValid)
                            {
                                string SaveMessage = _OperationSetup_ISERVICES.insertOperationDetail(Convert.ToInt32(comp_id), op_id, op_name, op_type, op_remarks, Convert.ToInt32(create_id), Trtype1, shfl_id, wrkstn_id, supervisor);
                                if (SaveMessage.Trim() == "Used")
                                {
                                    // Session["Message"] = "Used";
                                    OperationSetupModel UsedModel = new OperationSetupModel();
                                    UsedModel.Message = "Used";
                                    UsedModel.Command = "Refresh";
                                    UsedModel.TransType = "Update";
                                    UsedModel.BtnName = "BtnEdit";
                                    UsedModel.op_name = op_name;
                                    UsedModel.op_type = op_type;
                                    UsedModel.op_remarks = op_remarks;
                                    UsedModel.op_id = op_id;
                                    TempData["ModelData"] = UsedModel;
                                    Typ = _OperationSetupModel.TransType;
                                     cmd = _OperationSetupModel.Command;
                                     bt = _OperationSetupModel.BtnName;
                                    return RedirectToAction("OperationSetup",new { op_id,op_name, op_type , op_remarks, Typ , cmd , bt, shfl_id, wrkstn_id, supervisor } );
                                }
                                else
                                {
                                    // Session["Message"] = "Deleted";
                                    OperationSetupModel DeleteModel = new OperationSetupModel();
                                    DeleteModel.Message = "Deleted";
                                    DeleteModel.Command = "Refresh";
                                    DeleteModel.TransType = "Refresh";
                                    DeleteModel.BtnName = "BtnDelete";
                                    TempData["ModelData"] = DeleteModel;
                                    Typ = _OperationSetupModel.TransType;
                                    cmd = _OperationSetupModel.Command;
                                    bt = _OperationSetupModel.BtnName;
                                    return RedirectToAction("OperationSetup", new { Typ, cmd, bt });
                                }                              
                                //Session["Command"] = "Refresh";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnDelete";
                                //ViewBag.Message = Session["Message"].ToString();

                                //Session.Remove("op_name");
                                //Session.Remove("op_type_id");
                                //Session.Remove("OP_ID");
                                //Session.Remove("op_remarks");
                               // return RedirectToAction("OperationSetup");

                            }
                            else
                            {
                                return RedirectToAction("OperationSetup");
                            }

                        case "Save":
                            //Session["Command"] = command;
                            _OperationSetupModel.Command = command;
                            //Session.Remove("Message");
                            //var Trtype = Session["TransType"].ToString();
                            if (ModelState.IsValid)
                            {
                                string Trtype = string.Empty;
                                if (_OperationSetupModel.op_id != 0)
                                {
                                    Trtype = "Update";
                                }
                                else
                                {
                                    Trtype = "Save";
                                }
                                string Message = _OperationSetup_ISERVICES.insertOperationDetail(Convert.ToInt32(comp_id), op_id, op_name, op_type, op_remarks, Convert.ToInt32(create_id), Trtype, shfl_id,wrkstn_id, supervisor);

                                if (Message.Trim() == "Update" || Message.Trim() == "Save")
                                {
                                    _OperationSetupModel.Message = "Save";
                                    //Session["Message"] = "Save";
                                    //Session["Command"] = "EditNew";
                                    //Session["TransType"] = "Update";
                                    //Session["BtnName"] = "BtnSave";
                                    //ViewBag.Message = Session["Message"].ToString();
                                    //Session.Remove("op_name");
                                    //Session.Remove("op_type_id");
                                    //Session.Remove("OP_ID");
                                    //Session.Remove("op_remarks");
                                    OperationSetupModel savedata = new OperationSetupModel();
                                    savedata.Message = "Save";
                                    savedata.TransType = "Update";
                                    savedata.BtnName = "BtnSave";
                                    TempData["ModelData"] = savedata;
                                    Typ = savedata.TransType;
                                    cmd = savedata.Command;
                                    bt = savedata.BtnName;
                                    return RedirectToAction("OperationSetup", new { Typ, cmd, bt });
                                }
                                if (Message.Trim() == "Duplicate")
                                {
                                    _OperationSetupModel.Message = "Duplicate";
                                    _OperationSetupModel.TransType = "Update";
                                    _OperationSetupModel.BtnName = "BtnAddNew";
                                    _OperationSetupModel.Command = "Edit";
                                    _OperationSetupModel.op_name = op_name;
                                    _OperationSetupModel.op_id = op_id;
                                    _OperationSetupModel.op_type = op_type;
                                    _OperationSetupModel.op_remarks = op_remarks;
                                    TempData["ModelData"] = _OperationSetupModel;
                                    Typ = _OperationSetupModel.TransType;
                                    cmd = _OperationSetupModel.Command;
                                    bt = _OperationSetupModel.BtnName;
                                    //cmd = _OperationSetupModel.Command;
                                    //bt = _OperationSetupModel.BtnName;
                                    //Session["TransType"] = "Duplicate";
                                    //Session["Message"] = "Duplicate";
                                    //ViewBag.Message = Session["Message"].ToString();
                                    //ViewBag.BindOperationDetails = GetOperationDetails();
                                    //ViewBag.VBRoleList = GetRoleList();
                                    return RedirectToAction("OperationSetup", new { op_id, op_name, op_type, op_remarks, Typ, cmd, bt,shfl_id,wrkstn_id,supervisor });
                                    // return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OperationSetup/OperationSetupDetail.cshtml", _OperationSetupModel);

                                }


                                //Session.Remove("op_name");
                                //Session.Remove("op_type_id");
                                //Session.Remove("OP_ID");
                                //Session.Remove("op_remarks");
                                return RedirectToAction("OperationSetup");

                            }
                            else
                            {
                                return RedirectToAction("OperationSetup");
                            }
                        case "Refresh":
                            //Session["TransType"] = "Refresh";
                            //Session["BtnName"] = "BtnNew";
                            //Session["Command"] = "Add";
                            //Session.Remove("op_name");
                            //Session.Remove("op_type_id");
                            //Session.Remove("OP_ID");
                            //Session.Remove("op_remarks");
                            //Session.Remove("Message");
                            OperationSetupModel RefreshModel = new OperationSetupModel();
                            RefreshModel.Command = command;
                            RefreshModel.BtnName = "BtnRefresh";
                            RefreshModel.TransType = "Refresh";
                            return RedirectToAction("OperationSetup", RefreshModel);
                    }
                }
                else
                {
                    RedirectToAction("");
                }
                //end show and hide tools

                return RedirectToAction("OperationSetup");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetOperationDetails()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _OperationSetup_ISERVICES.GetOperationDetailsDAL(Convert.ToInt32(Comp_ID));
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public  ActionResult dbClickEdit(string op_id1,string op_name,string op_type_id,string op_remarks, string shfl_id, string wrkstn_id, string supervisor)
        {
            //try
            //{
            //    if(Session["CompId"] != null)
            //    {
            //Session["Message"] = "";
            //Session["Command"] = "View";
            //Session["OP_ID"] = op_id1;
            //Session["TransType"] = "EditNew";
            //Session["BtnName"] = "BtnEdit";
            //Session["TransType"] = "Update";
            //ViewBag.op_name = op_name;
            //Session["op_name"] = op_name;
            //Session["op_type_id"] = op_type_id;
            //Session["op_remarks"] = op_remarks;
            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            OperationSetupModel _OperationSetupModel = new OperationSetupModel();
            _OperationSetupModel.Command = "View";
            _OperationSetupModel.op_name = op_name;
            _OperationSetupModel.op_remarks = op_remarks;
            _OperationSetupModel.op_type = op_type_id;
            _OperationSetupModel.op_id =Convert.ToInt32( op_id1);
            _OperationSetupModel.BtnName = "BtnEdit";
            _OperationSetupModel.TransType = "Update";
            _OperationSetupModel.Shopfloor =shfl_id;
            _OperationSetupModel.Workstation = wrkstn_id;
            _OperationSetupModel.Supervisor = supervisor;
            return RedirectToAction("OperationSetup", _OperationSetupModel);
            //    }
            //    else
            //    {
            //        RedirectToAction("Home", "Index");
            //    }
            //   // return op_id1;
            //    //return RedirectToAction("OperationSetup", "OperationSetup");
            //    return RedirectToAction("OperationSetup");
            //}
            //catch (Exception ex)
            //{
            //    string path = Server.MapPath("~");
            //    Errorlog.LogError(path, ex);
            //    return null;
            //}
        }

        [HttpPost]
        public ActionResult BindWorkStationList(OperationSetupModel OperationSetupModel1, int shfl_id)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    
                    DataSet Data = _OperationSetup_ISERVICES.GetWorkStationDAL(Comp_ID, Br_ID, shfl_id);
                    List<Workstation> work = new List<Workstation>();
                    
                    foreach (DataRow dr in Data.Tables[0].Rows)
                    {
                        Workstation wrk = new Workstation();
                        wrk.wrk_id = dr["ws_id"].ToString();
                        wrk.wrk_name = dr["ws_name"].ToString();
                        work.Add(wrk);

                    }
                    work.Insert(0, new Workstation() { wrk_id = "0", wrk_name = "---Select---" });
                    OperationSetupModel1.WorkstationList = work;
                    DataRows = Json(JsonConvert.SerializeObject(Data));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
    }
}
