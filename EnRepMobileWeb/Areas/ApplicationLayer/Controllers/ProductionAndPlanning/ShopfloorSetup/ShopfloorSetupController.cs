using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ShopfloorSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ShopfloorSetup
{
    public class ShopfloorSetupController : Controller
    {
        // GET: ApplicationLayer/ShopfloorSetup
        string comp_id,branch_id;
        string CompID, language, title, UserID = String.Empty;
        Common_IServices _Common_IServices;
        string DocumentMenuId = "105105101";
        ShopfloorSetup_ISERVICES _ShopfloorSetup_ISERVICES;
        ShopfloorSetupModel _ShopfloorSetupModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        public ShopfloorSetupController(Common_IServices _Common_IServices, ShopfloorSetup_ISERVICES _ShopfloorSetup_ISERVICES)
        {
            this._ShopfloorSetup_ISERVICES = _ShopfloorSetup_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        public ActionResult ShopfloorSetup()
        {
            try
            {
                //if (Session["compid"] != null)
                //{
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
                    branch_id = Session["BranchId"].ToString();
                }
                ShopfloorSetupModel _ShopfloorSetupModel = new ShopfloorSetupModel();
                    ViewBag.MenuPageName = getDocumentName();
                    _ShopfloorSetupModel.Title = title;
                    ViewBag.GetShopFloorDetails = GetShopFloorDetails();
                    ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupList.cshtml", _ShopfloorSetupModel);
                //}
                //else
                //{
                //    RedirectToAction("Home", "Index");
                //}
              

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
        private DataTable GetShopFloorDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id));
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }

            return dt;
        }
        //[HttpPost]
        public ActionResult dbClickEdit(string br_id, string shfl_id)
        {
            //JsonResult DataRows = null;
            //try
            //{
            //    if (Session["CompId"] != null)
            //    {
            //Session["Message"] = "";
            //Session["Command"] = "View";
            //Session["TransType"] = "EditNew";
            //Session["BtnName"] = "BtnEdit";
            //Session["TransType"] = "Update";
            //Session["br_id"] = br_id;
            //Session["shfl_id"] = shfl_id;
            //Session["SaveUpd"] = "0";
            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
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
            ShopfloorSetupModel dblclick = new ShopfloorSetupModel();
                    UrlModel _url = new UrlModel();
                    //dblclick.Branchid = br_id;
                    dblclick.sh_id = shfl_id;
                    dblclick.TransType = "Update";
                    dblclick.BtnName = "BtnEdit";
                    dblclick.Command = "Refresh";
                    TempData["ModelData"] = dblclick;
                    _url.tp = "Update";
                    _url.bt ="BtnEdit";
                    _url.SV_No = shfl_id;
                    _url.Cmd = "Refresh";
                   // _url.SV_DT = shfl_id;
                    string Comp_ID = Session["CompId"].ToString();
                    return RedirectToAction("ShopfloorSetupDetail", _url);
                    // DataSet dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsBranchWiseDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), Convert.ToInt32(shfl_id));
                    //DataRows = Json(JsonConvert.SerializeObject(dt.Tables[1]));
            //    }
            //    else
            //    {
            //        RedirectToAction("Home", "Index");
            //    }
            //    return RedirectToAction("ShopfloorSetupDetail");
            //}
            //catch (Exception ex)
            //{
            //    string path = Server.MapPath("~");
            //    Errorlog.LogError(path, ex);
            //    return View("~/Views/Shared/Error.cshtml");
            //}
            //return DataRows;
        }
       
        public ActionResult AddShopfloorSetupDetail()
        {
            try
            {
                //if (Session["compid"] != null)
                //{
                    //ViewBag.MenuPageName = getDocumentName();
                    //_ShopfloorSetupModel = new ShopfloorSetupModel();
                    //_ShopfloorSetupModel.Title = title;
                    //Session["Message"] = "New";
                    //Session["Command"] = "Add";
                    //Session["AppStatus"] = 'D';
                    //Session["TransType"] = "Save";
                    //Session["BtnName"] = "BtnAddNew";
                    ShopfloorSetupModel AddNewModel = new ShopfloorSetupModel();
                    AddNewModel.Command = "Add";
                    AddNewModel.TransType = "Save";
                    AddNewModel.BtnName = "BtnAddNew";
                    TempData["ModelData"] = AddNewModel;
                    UrlModel AddNew_Model = new UrlModel();
                    AddNew_Model.bt = "BtnAddNew";
                    AddNew_Model.Cmd = "Add";
                    AddNew_Model.tp = "Save";
                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branch_id = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("ShopfloorSetup");
                }
                /*End to chk Financial year exist or not*/
                return RedirectToAction("ShopfloorSetupDetail", AddNew_Model);
                }
            //    ViewBag.VBRoleList = GetRoleList();
            //    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupDetail.cshtml", _ShopfloorSetupModel);
            //}
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult ShopfloorSetupDetail(UrlModel _urlModel)
        {
            try
            {   /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                   var  br_id = Session["BranchId"].ToString();
                }
                //if (Session["compid"] != null)
                //{
               var _ShopfloorSetupModel1 = TempData["ModelData"] as ShopfloorSetupModel;
                if(_ShopfloorSetupModel1 != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    //ShopfloorSetupModel _ShopfloorSetupModel11 = new ShopfloorSetupModel();
                    _ShopfloorSetupModel1.Title = title;
                    //if (Session["BtnName"] == null || Session["BtnName"].ToString() == "")
                    //{
                    //    Session["TransType"] = "Refresh";
                    //    Session["BtnName"] = "BtnNew";
                    //    Session["Command"] = "Add";
                    //}
                    //if (Session["br_id"] != null || Session["SaveUpd"] != null)
                    if (_ShopfloorSetupModel1.TransType == "Update" || _ShopfloorSetupModel1.Command == "Edit")
                    {

                        //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        //int shfl_id = Convert.ToInt32(Session["shfl_id"].ToString());

                        //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        int shfl_id = Convert.ToInt32(_ShopfloorSetupModel1.sh_id);
                        string Comp_ID = Session["CompId"].ToString();

                        DataSet dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsBranchWiseDAL(Convert.ToInt32(Comp_ID), br_id, shfl_id);
                        if(_ShopfloorSetupModel1.Message == "Duplicate")
                        {
                            _ShopfloorSetupModel1.shfl_id = Convert.ToInt32(_ShopfloorSetupModel1.sh_id);
                            _ShopfloorSetupModel1.shfl_name = _ShopfloorSetupModel1.shfl_name;
                            _ShopfloorSetupModel1.shfl_loc = _ShopfloorSetupModel1.shfl_loc;
                            _ShopfloorSetupModel1.shfl_remarks = _ShopfloorSetupModel1.shfl_remarks;
                            if (_ShopfloorSetupModel1.sh_id != "" && _ShopfloorSetupModel1.sh_id != "0" && _ShopfloorSetupModel1.sh_id != null && _ShopfloorSetupModel1.TransType == "Update")   // added By Nitesh 16-10-2023 11:55 for header Table when msg Duplicate
                            {
                                _ShopfloorSetupModel1.create_id = Convert.ToString(dt.Tables[0].Rows[0]["created_id"]);
                                _ShopfloorSetupModel1.create_dt = Convert.ToString(dt.Tables[0].Rows[0]["create_dt"]);
                                _ShopfloorSetupModel1.mod_id = Convert.ToString(dt.Tables[0].Rows[0]["mod_id"]);
                                _ShopfloorSetupModel1.mod_dt = Convert.ToString(dt.Tables[0].Rows[0]["mod_dt"]);
                               
                            }
                            else
                            {
                                _ShopfloorSetupModel1.create_id =null;
                                _ShopfloorSetupModel1.create_dt = null;
                                _ShopfloorSetupModel1.mod_id = null;
                                _ShopfloorSetupModel1.mod_dt = null;
                            }
                            //_ShopfloorSetupModel1.ProductionCapacityList = ViewBag.ItemData;   // added By Nitesh 16-10-2023 11:55 for item Table when msg Duplicate
                        }
                        else
                        {
                            if (dt.Tables[0].Rows.Count > 0)
                            {
                                _ShopfloorSetupModel1.shfl_id = Convert.ToInt32(dt.Tables[0].Rows[0]["shfl_id"]);
                                _ShopfloorSetupModel1.shfl_name = Convert.ToString(dt.Tables[0].Rows[0]["shfl_name"]);
                                _ShopfloorSetupModel1.shfl_loc = Convert.ToString(dt.Tables[0].Rows[0]["shfl_loc"]);
                                _ShopfloorSetupModel1.shfl_remarks = Convert.ToString(dt.Tables[0].Rows[0]["shfl_remarks"]);

                                _ShopfloorSetupModel1.create_id = Convert.ToString(dt.Tables[0].Rows[0]["created_id"]);
                                _ShopfloorSetupModel1.create_dt = Convert.ToString(dt.Tables[0].Rows[0]["create_dt"]);
                                _ShopfloorSetupModel1.mod_id = Convert.ToString(dt.Tables[0].Rows[0]["mod_id"]);
                                _ShopfloorSetupModel1.mod_dt = Convert.ToString(dt.Tables[0].Rows[0]["mod_dt"]);
                                //Bind production Capacity


                                ViewBag.AttechmentDetails = dt.Tables[2];
                            }
                            if (dt.Tables[1].Rows.Count > 0)
                            {
                                List<ProductionCapacity> ArrProductionCapacity = new List<ProductionCapacity>();
                                for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                                {
                                    ProductionCapacity pc = new ProductionCapacity();
                                    pc.item_id = Convert.ToString(dt.Tables[1].Rows[i]["item_id"]);
                                    pc.item_name = Convert.ToString(dt.Tables[1].Rows[i]["item_name"]);
                                    pc.uom_id = Convert.ToString(dt.Tables[1].Rows[i]["uom_id"]);
                                    pc.uom_alias = Convert.ToString(dt.Tables[1].Rows[i]["uom_alias"]);
                                    pc.optm_qty = Convert.ToString(dt.Tables[1].Rows[i]["optm_qty"]);
                                    pc.per_unit = Convert.ToString(dt.Tables[1].Rows[i]["per_unit"]);
                                    pc.per_unit_val = Convert.ToString(dt.Tables[1].Rows[i]["per_unit_val"]);
                                    ArrProductionCapacity.Add(pc);
                                }
                                _ShopfloorSetupModel1.ProductionCapacityList = ArrProductionCapacity;
                            }
                        }
                       
                       
                        if (dt.Tables[3].Rows.Count > 0)   // Added By Nitesh 13102023 1018 for Production History
                        {
                            List<ProductionHistory> ArrProductionHistory = new List<ProductionHistory>();
                            for (int i = 0; i < dt.Tables[3].Rows.Count; i++)
                            {
                                ProductionHistory Pro_his = new ProductionHistory();
                                Pro_his.item_id = Convert.ToString(dt.Tables[3].Rows[i]["item_id"]);
                                Pro_his.item_name = Convert.ToString(dt.Tables[3].Rows[i]["item_name"]);
                                Pro_his.uom_id = Convert.ToString(dt.Tables[3].Rows[i]["uom_id"]);
                                Pro_his.uom_alias = Convert.ToString(dt.Tables[3].Rows[i]["uom_alias"]);
                                Pro_his.prod_qty = Convert.ToString(dt.Tables[3].Rows[i]["prod_qty"]);
                                Pro_his.reject_qty = Convert.ToString(dt.Tables[3].Rows[i]["reject_qty"]);
                                Pro_his.cnf_dt = Convert.ToString(dt.Tables[3].Rows[i]["cnf_dt"]);
                                ArrProductionHistory.Add(Pro_his);
                            }
                            _ShopfloorSetupModel1.ProductionHistoryList = ArrProductionHistory;
                        }
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupDetail.cshtml", _ShopfloorSetupModel1);

                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _ShopfloorSetupModel1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupDetail.cshtml", _ShopfloorSetupModel1);
                    }
                    //else
                    //{

                    //    if (Session["shfl_name"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_name = Session["shfl_name"].ToString();
                    //        Session.Remove("shfl_name");
                    //    }
                    //    if (Session["shfl_loc"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_loc = Session["shfl_loc"].ToString();
                    //        Session.Remove("shfl_loc");
                    //    }
                    //    if (Session["shfl_remarks"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_remarks = Session["shfl_remarks"].ToString();
                    //        Session.Remove("shfl_remarks");
                    //    }



                    //}

                    // }
                    //else
                    //{
                    //    RedirectToAction("Home", "Index");
                    //}
                   
                }            
                else
                {
                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        branch_id = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    ShopfloorSetupModel _ShopfloorSetupModel = new ShopfloorSetupModel();
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _ShopfloorSetupModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _ShopfloorSetupModel.BtnName = _urlModel.bt;
                        }
                        _ShopfloorSetupModel.sh_id = _urlModel.SV_No;                      
                        _ShopfloorSetupModel.Command = _urlModel.Cmd;
                        _ShopfloorSetupModel.TransType = _urlModel.tp;
                    }
                  
                    //if (Session["BtnName"] == null || Session["BtnName"].ToString() == "")
                    //{
                    //    Session["TransType"] = "Refresh";
                    //    Session["BtnName"] = "BtnNew";
                    //    Session["Command"] = "Add";
                    //}
                    //if (Session["br_id"] != null || Session["SaveUpd"] != null)
                    if (_ShopfloorSetupModel.TransType == "Update" || _ShopfloorSetupModel.Command == "Edit")
                    {

                        //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        //int shfl_id = Convert.ToInt32(Session["shfl_id"].ToString());

                        //int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        int br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        int shfl_id = Convert.ToInt32(_ShopfloorSetupModel.sh_id);
                        string Comp_ID = Session["CompId"].ToString();

                        DataSet dt = _ShopfloorSetup_ISERVICES.GetShopFloorDetailsBranchWiseDAL(Convert.ToInt32(Comp_ID), br_id, shfl_id);
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            _ShopfloorSetupModel.shfl_id = Convert.ToInt32(dt.Tables[0].Rows[0]["shfl_id"]);
                            _ShopfloorSetupModel.shfl_name = Convert.ToString(dt.Tables[0].Rows[0]["shfl_name"]);
                            _ShopfloorSetupModel.shfl_loc = Convert.ToString(dt.Tables[0].Rows[0]["shfl_loc"]);
                            _ShopfloorSetupModel.shfl_remarks = Convert.ToString(dt.Tables[0].Rows[0]["shfl_remarks"]);

                       
                        //    _ShopfloorSetupModel1.ProductionCapacityList = ViewBag.ItemData;
                        //if (_ShopfloorSetupModel1.BtnName != "BtnAddNew")
                        //{
                        //    _ShopfloorSetupModel.create_id = Convert.ToString(dt.Tables[0].Rows[0]["created_id"]);
                        //    _ShopfloorSetupModel.create_dt = Convert.ToString(dt.Tables[0].Rows[0]["create_dt"]);
                        //    _ShopfloorSetupModel.mod_id = Convert.ToString(dt.Tables[0].Rows[0]["mod_id"]);
                        //    _ShopfloorSetupModel.mod_dt = Convert.ToString(dt.Tables[0].Rows[0]["mod_dt"]);
                        //}
                            //Bind production Capacity
                           
                        }
                        if (dt.Tables[1].Rows.Count > 0)
                        {
                            List<ProductionCapacity> ArrProductionCapacity = new List<ProductionCapacity>();
                            for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                            {
                                ProductionCapacity pc = new ProductionCapacity();
                                pc.item_id = Convert.ToString(dt.Tables[1].Rows[i]["item_id"]);
                                pc.item_name = Convert.ToString(dt.Tables[1].Rows[i]["item_name"]);
                                pc.uom_id = Convert.ToString(dt.Tables[1].Rows[i]["uom_id"]);
                                pc.uom_alias = Convert.ToString(dt.Tables[1].Rows[i]["uom_alias"]);
                                pc.optm_qty = Convert.ToString(dt.Tables[1].Rows[i]["optm_qty"]);
                                pc.per_unit = Convert.ToString(dt.Tables[1].Rows[i]["per_unit"]);
                                pc.per_unit_val = Convert.ToString(dt.Tables[1].Rows[i]["per_unit_val"]);
                                ArrProductionCapacity.Add(pc);
                            }
                            _ShopfloorSetupModel.ProductionCapacityList = ArrProductionCapacity;
                        }
                        if (dt.Tables[3].Rows.Count > 0)
                        {
                            List<ProductionHistory> ArrProductionHistory = new List<ProductionHistory>();
                            for (int i = 0; i < dt.Tables[3].Rows.Count; i++)
                            {
                                ProductionHistory Pro_his = new ProductionHistory();
                                Pro_his.item_id = Convert.ToString(dt.Tables[3].Rows[i]["item_id"]);
                                Pro_his.item_name = Convert.ToString(dt.Tables[3].Rows[i]["item_name"]);
                                Pro_his.uom_id = Convert.ToString(dt.Tables[3].Rows[i]["uom_id"]);
                                Pro_his.uom_alias = Convert.ToString(dt.Tables[3].Rows[i]["uom_alias"]);
                                Pro_his.prod_qty = Convert.ToString(dt.Tables[3].Rows[i]["prod_qty"]);
                                Pro_his.reject_qty = Convert.ToString(dt.Tables[3].Rows[i]["reject_qty"]);
                                Pro_his.cnf_dt = Convert.ToString(dt.Tables[3].Rows[i]["cnf_dt"]);
                                ArrProductionHistory.Add(Pro_his);
                            }
                            _ShopfloorSetupModel.ProductionHistoryList = ArrProductionHistory;
                        }
                        ViewBag.AttechmentDetails = dt.Tables[2];
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        _ShopfloorSetupModel.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupDetail.cshtml", _ShopfloorSetupModel);

                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _ShopfloorSetupModel.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorSetup/ShopfloorSetupDetail.cshtml", _ShopfloorSetupModel);
                    }
                    //else
                    //{

                    //    if (Session["shfl_name"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_name = Session["shfl_name"].ToString();
                    //        Session.Remove("shfl_name");
                    //    }
                    //    if (Session["shfl_loc"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_loc = Session["shfl_loc"].ToString();
                    //        Session.Remove("shfl_loc");
                    //    }
                    //    if (Session["shfl_remarks"] != null)
                    //    {
                    //        _ShopfloorSetupModel.shfl_remarks = Session["shfl_remarks"].ToString();
                    //        Session.Remove("shfl_remarks");
                    //    }



                    //}

                    // }
                    //else
                    //{
                    //    RedirectToAction("Home", "Index");
                    //}
                  
                }
                   
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult itemtable(ShopfloorSetupModel _ShopfloorSetupModel)  // added By Nitesh 16-10-2023 11:55 for item Table when msg Duplicate
        {
            //DataTable shpflore = new DataTable();
            //WrkStation.Columns.Add("item_id", typeof(string));
            //WrkStation.Columns.Add("item_name", typeof(string));
            //WrkStation.Columns.Add("uom_alias", typeof(string));
            //WrkStation.Columns.Add("uom_id", typeof(string));
            //WrkStation.Columns.Add("optm_qty", typeof(string));
            //WrkStation.Columns.Add("per_unit", typeof(string));


            JArray jObject = JArray.Parse(_ShopfloorSetupModel.shopfloorattrdetails);
            List<ProductionCapacity> ArrProductionCapacity = new List<ProductionCapacity>();
            for (int i = 0; i < jObject.Count; i++)
            {
                //dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                //dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                //dtrowLines["uom_alias"] = jObject[i]["uom_alias"].ToString();
                //dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                //dtrowLines["optm_qty"] = jObject[i]["optm_qty"].ToString();
                //dtrowLines["per_unit"] = jObject[i]["per_unit"].ToString();
                         
                    ProductionCapacity pc = new ProductionCapacity();
                pc.item_id = Convert.ToString(jObject[i]["item_id"].ToString());
                pc.item_name = Convert.ToString(jObject[i]["item_name"]);
                pc.uom_id = Convert.ToString(jObject[i]["uom_id"]);
                pc.uom_alias = Convert.ToString(jObject[i]["uom_alias"]);
                pc.optm_qty = Convert.ToString(jObject[i]["optm_qty"]);
                pc.per_unit = Convert.ToString(jObject[i]["tbl_hdn_PerUnit_Val"]);
                pc.per_unit_val = Convert.ToString(jObject[i]["per_unit "]);
                ArrProductionCapacity.Add(pc);
            }
            _ShopfloorSetupModel.ProductionCapacityList= ArrProductionCapacity;
            return RedirectToAction("ShopfloorSetupDetail", _ShopfloorSetupModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ShopfloorSave(ShopfloorSetupModel _ShopfloorSetupModel, string command)
        {

            /*----- Attatchment Section start--------*/
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            /*----- Attatchment Section End--------*/
            try
            {
                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                    branch_id = Session["BranchId"].ToString();
                    //int shfl_id = _ShopfloorSetupModel.shfl_id;
                    //string shfl_name = _ShopfloorSetupModel.shfl_name;
                    //string shfl_loc = _ShopfloorSetupModel.shfl_loc;
                    //string shfl_remarks = _ShopfloorSetupModel.shfl_remarks;
                    //Session["shfl_name"] = shfl_name;
                    //Session["shfl_loc"] = shfl_loc;
                    //Session["shfl_remarks"] = shfl_remarks;
                    _ShopfloorSetupModel.TransType = command;
                    _ShopfloorSetupModel.create_id = Session["UserId"].ToString();
                    _ShopfloorSetupModel.comp_id = Convert.ToInt32(Session["CompId"].ToString());
                    _ShopfloorSetupModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    string create_id = Session["UserId"].ToString();
                    string action = "";
                    if (_ShopfloorSetupModel.DeleteCommand == "Delete")
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
                                return RedirectToAction("dbClickEdit", new { br_id = _ShopfloorSetupModel.br_id, shfl_id = _ShopfloorSetupModel.shfl_id});
                            }
                           /*End to chk Financial year exist or not*/
                           //Session["Message"] = "";
                           //Session["Command"] = command;
                           //Session["shfl_id"] = _ShopfloorSetupModel.shfl_id;
                           //Session["BtnName"] = "BtnAddNew";
                           //Session["TransType"] = "Update";
                           _ShopfloorSetupModel.TransType = "Update";
                            _ShopfloorSetupModel.Command = command;
                            _ShopfloorSetupModel.BtnName = "BtnAddNew";
                            _ShopfloorSetupModel.sh_id = _ShopfloorSetupModel.shfl_id.ToString();
                            TempData["ModelData"] = _ShopfloorSetupModel;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnAddNew";
                            EditModel.SV_No = _ShopfloorSetupModel.shfl_id.ToString();
                            return RedirectToAction("ShopfloorSetupDetail", EditModel);

                        case "Add":
                            //Session["Message"] = "";
                            //Session["Command"] = command;
                            //Session["TransType"] = "Save";
                            //Session["BtnName"] = "BtnAddNew";

                            //Session.Remove("br_id");
                            //Session.Remove("SaveUpd");
                            ShopfloorSetupModel adddnew = new ShopfloorSetupModel();
                            adddnew.Command = "Add";
                            adddnew.TransType = "Save";
                            adddnew.BtnName = "BtnAddNew";
                            UrlModel NewModel = new UrlModel();
                            NewModel.Cmd = "Add";
                            NewModel.tp = "Save";
                            NewModel.bt = "BtnAddNew";
                            NewModel.DMS = "D";
                            TempData["ModelData"] = adddnew;
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                branch_id = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, branch_id) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                //_ShopfloorSetupModel.sh_id = _ShopfloorSetupModel.shfl_id.ToString();
                                if (_ShopfloorSetupModel.shfl_id!=0)
                                    return RedirectToAction("dbClickEdit", new { br_id = _ShopfloorSetupModel.br_id, shfl_id = _ShopfloorSetupModel.shfl_id });
                                else
                                    adddnew.Command = "Refresh";
                                adddnew.TransType = "Refresh";
                                adddnew.BtnName = "BtnRefresh";
                                TempData["ModelData"] = adddnew;
                                return RedirectToAction("ShopfloorSetupDetail", adddnew);
                            }
                            /*End to chk Financial year exist or not*/
                            return RedirectToAction("ShopfloorSetupDetail", NewModel);

                        case "Delete":
                            _ShopfloorSetupModel.Command = command;
                            //Session["Command"] = command;
                            //Session["TransType"] = "Delete";
                            //var Trtype1 = Session["TransType"].ToString();
                            //var Trtype = Session["TransType"].ToString();
                            // _ShopfloorSetupModel.TransType = Trtype;
                            // _ShopfloorSetupModel.shfl_id = shfl_id;
                            DataTable dtShopFloorSetup1 = new DataTable();
                            dtShopFloorSetup1.Columns.Add("comp_id", typeof(Int32));
                            dtShopFloorSetup1.Columns.Add("br_id", typeof(Int32));
                            dtShopFloorSetup1.Columns.Add("shfl_name", typeof(string));
                            dtShopFloorSetup1.Columns.Add("shfl_loc", typeof(string));
                            dtShopFloorSetup1.Columns.Add("shfl_remarks", typeof(string));
                            dtShopFloorSetup1.Columns.Add("create_id", typeof(Int32));
                            dtShopFloorSetup1.Columns.Add("TransType", typeof(string));
                            dtShopFloorSetup1.Columns.Add("shfl_id", typeof(Int32));

                            DataRow drShopFloorSetup1 = dtShopFloorSetup1.NewRow();

                            drShopFloorSetup1["comp_id"] = _ShopfloorSetupModel.comp_id;// Convert.ToInt32(comp_id);
                            drShopFloorSetup1["br_id"] = _ShopfloorSetupModel.br_id;// Convert.ToInt32(comp_id);
                            drShopFloorSetup1["shfl_name"] = _ShopfloorSetupModel.shfl_name;
                            drShopFloorSetup1["shfl_loc"] = _ShopfloorSetupModel.shfl_loc;
                            drShopFloorSetup1["shfl_remarks"] = _ShopfloorSetupModel.shfl_remarks;
                            drShopFloorSetup1["create_id"] = _ShopfloorSetupModel.create_id;
                            //if (_ShopfloorSetupModel.shfl_name != null)
                            //{
                                drShopFloorSetup1["TransType"] = command;
                            //}
                            //else
                            //{
                            //    drShopFloorSetup1["TransType"] = _ShopfloorSetupModel.TransType;
                            //}
                            
                            drShopFloorSetup1["shfl_id"] = _ShopfloorSetupModel.shfl_id;
                            dtShopFloorSetup1.Rows.Add(drShopFloorSetup1);

                            DataTable dtShopFloorCapacity1 = new DataTable();

                            dtShopFloorCapacity1.Columns.Add("item_id", typeof(string));
                            dtShopFloorCapacity1.Columns.Add("uom_id", typeof(Int32));
                            dtShopFloorCapacity1.Columns.Add("optm_qty", typeof(float));
                            dtShopFloorCapacity1.Columns.Add("per_unit", typeof(char));
                            //JArray jObject = JArray.Parse(_ShopfloorSetupModel.shopfloorattrdetails);
                            //for (int i = 0; i < jObject.Count; i++)
                            //{
                            DataRow drShopFloorCapacity1 = dtShopFloorCapacity1.NewRow();

                            //    drShopFloorCapacity["item_id"] = jObject[i]["item_id"];
                            //    drShopFloorCapacity["uom_id"] = jObject[i]["uom_id"];
                            //    drShopFloorCapacity["optm_qty"] = jObject[i]["optm_qty"];
                            //    drShopFloorCapacity["per_unit"] = jObject[i]["per_unit"];
                            //    dtShopFloorCapacity.Rows.Add(drShopFloorCapacity);
                            //}
                            /*---------Attachments Section Start----------------*/
                            DataTable Attachments1 = new DataTable();
                           
                            Attachments1.Columns.Add("id", typeof(string));
                            Attachments1.Columns.Add("file_name", typeof(string));
                            Attachments1.Columns.Add("file_path", typeof(string));
                            Attachments1.Columns.Add("file_def", typeof(char));
                            Attachments1.Columns.Add("comp_id", typeof(Int32));
                            
                            DataRow drAttachments = Attachments1.NewRow();
                            /*---------Attachments Section End----------------*/

                            string sfs_id= Convert.ToInt32(_ShopfloorSetupModel.shfl_id).ToString();

                            SaveMessage = _ShopfloorSetup_ISERVICES.insertShopfloorDetail(dtShopFloorSetup1, dtShopFloorCapacity1, Attachments1, "");
                            string[] splitmsg = SaveMessage.Split('-');
                            string Message2 = splitmsg[0].ToString().Trim();
                            string Message5 = splitmsg[1].ToString().Trim();
                            if (Message2 == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message2.Replace("_", " ") + " " + Message5;//InvoiceAdjustmentNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _ShopfloorSetupModel.Message = Message2.Split(',')[0].Replace("_", "");
                                return RedirectToAction("ShopfloorSetupDetail");
                            }
                            /*---------Attachments Section Start----------------*/
                            if (!string.IsNullOrEmpty(sfs_id))
                            {
                                getDocumentName(); /* To set Title*/
                                PageName = title.Replace(" ", "");
                                var other = new CommonController(_Common_IServices);
                                other.DeleteTempFile(comp_id+ branch_id, PageName, sfs_id, Server);
                            }
                            /*---------Attachments Section End----------------*/
                            //if (splitmsg[0].ToString().Trim() == "Delete")
                            if (Message2 == "Delete")
                            {
                                //Session["Message"] = "Deleted";
                                //Session["Command"] = "Refresh";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnDelete";
                                //ViewBag.Message = Session["Message"].ToString();
                                ShopfloorSetupModel DeleteModel = new ShopfloorSetupModel();
                                DeleteModel.Message = "Deleted";
                                DeleteModel.Command = "Refresh";
                                DeleteModel.TransType = "Refresh";
                                DeleteModel.BtnName = "BtnDelete";
                                TempData["ModelData"] = DeleteModel;
                                UrlModel Delete = new UrlModel();
                                Delete.Cmd = DeleteModel.Command;
                                Delete.tp = "Refresh";
                                Delete.bt = "BtnDelete";
                                _ShopfloorSetupModel = null;
                                return RedirectToAction("ShopfloorSetupDetail", Delete);
                            }
                            else if(Message2 == "Used")
                            {
                                //Session["Message"] = "Used";
                                //Session["TransType"] = "Refresh";
                                ShopfloorSetupModel Used_Model = new ShopfloorSetupModel();
                                Used_Model.Message = "Used";
                                Used_Model.TransType = "Update";
                                Used_Model.Command = "Refresh";
                                Used_Model.sh_id = sfs_id;
                                Used_Model.BtnName = "BtnSave";
                                TempData["ModelData"] = Used_Model;
                                UrlModel UsedModel = new UrlModel();
                                UsedModel.Cmd = _ShopfloorSetupModel.Command;
                                UsedModel.tp = "Update";
                                UsedModel.Cmd = "Refresh";
                                UsedModel.bt = "BtnSave";
                                UsedModel.SV_No = sfs_id;
                                return RedirectToAction("ShopfloorSetupDetail", UsedModel);
                            }
                            else
                            {
                                return RedirectToAction("ShopfloorSetupDetail");
                            }
                        case "Save":

                            // Session["Command"] = command;
                            _ShopfloorSetupModel.Command= command;
                            // Trtype = Session["TransType"].ToString();
                            branch_id = Convert.ToInt32(_ShopfloorSetupModel.br_id).ToString();

                            DataTable dtShopFloorSetup = new DataTable();
                            dtShopFloorSetup.Columns.Add("comp_id", typeof(Int32));
                            dtShopFloorSetup.Columns.Add("br_id", typeof(Int32));
                            dtShopFloorSetup.Columns.Add("shfl_name", typeof(string));
                            dtShopFloorSetup.Columns.Add("shfl_loc", typeof(string));
                            dtShopFloorSetup.Columns.Add("shfl_remarks", typeof(string));
                            dtShopFloorSetup.Columns.Add("create_id", typeof(Int32));
                            dtShopFloorSetup.Columns.Add("TransType", typeof(string));
                            dtShopFloorSetup.Columns.Add("shfl_id", typeof(Int32));

                            DataRow drShopFloorSetup = dtShopFloorSetup.NewRow();

                            drShopFloorSetup["comp_id"] = _ShopfloorSetupModel.comp_id;// Convert.ToInt32(comp_id);
                            drShopFloorSetup["br_id"] = _ShopfloorSetupModel.br_id;// Convert.ToInt32(comp_id);
                            drShopFloorSetup["shfl_name"] = _ShopfloorSetupModel.shfl_name;
                            drShopFloorSetup["shfl_loc"] = _ShopfloorSetupModel.shfl_loc;
                            drShopFloorSetup["shfl_remarks"] = _ShopfloorSetupModel.shfl_remarks;
                            drShopFloorSetup["create_id"] = _ShopfloorSetupModel.create_id;
                           // drShopFloorSetup["TransType"] = _ShopfloorSetupModel.TransType;// Trtype;// _ShopfloorSetupModel.TransType;Session["shfl_id"]

                            if (_ShopfloorSetupModel.shfl_id.ToString() != null && _ShopfloorSetupModel.shfl_id !=0)
                            {
                                drShopFloorSetup["TransType"] = "Update";
                                _ShopfloorSetupModel.TransType = "Update";
                            }
                            else
                            {
                                drShopFloorSetup["TransType"] = "Save";
                            }
                            if ( _ShopfloorSetupModel.TransType == "Update")
                            {
                                //drShopFloorSetup["shfl_id"] =Convert.ToInt32(Session["shfl_id"].ToString());
                                drShopFloorSetup["shfl_id"] =Convert.ToInt32( _ShopfloorSetupModel.shfl_id);
                            }
                            else
                            {
                                drShopFloorSetup["shfl_id"] = 0;
                            }
                            dtShopFloorSetup.Rows.Add(drShopFloorSetup);

                            DataTable dtShopFloorCapacity = new DataTable();

                            dtShopFloorCapacity.Columns.Add("item_id", typeof(string));
                            dtShopFloorCapacity.Columns.Add("uom_id", typeof(Int32));
                            dtShopFloorCapacity.Columns.Add("optm_qty", typeof(float));
                            dtShopFloorCapacity.Columns.Add("per_unit", typeof(char));
                            JArray jObject = JArray.Parse(_ShopfloorSetupModel.shopfloorattrdetails);
                            for (int i = 0; i < jObject.Count; i++)
                            {
                                DataRow drShopFloorCapacity = dtShopFloorCapacity.NewRow();

                                drShopFloorCapacity["item_id"] = jObject[i]["item_id"];
                                drShopFloorCapacity["uom_id"] = jObject[i]["uom_id"];
                                drShopFloorCapacity["optm_qty"] = jObject[i]["optm_qty"];
                                drShopFloorCapacity["per_unit"] = jObject[i]["per_unit"];
                                dtShopFloorCapacity.Rows.Add(drShopFloorCapacity);
                            }
                            string SystemDetail = string.Empty;
                            SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                           

                            /*-----------------Attachment Section Start------------------------*/
                            DataTable Attachments = new DataTable();
                            DataTable dtAttachment = new DataTable();
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                            var attachData = TempData["IMGDATA"] as Shopflore_Model;
                            TempData["IMGDATA"] = null;
                            //if (_ShopfloorSetupModel.attatchmentdetail != null)
                            //{
                            //    if (attachData != null)
                            //    {
                            //        //if (Session["AttachMentDetailItmStp"] != null)
                            //        if (attachData.AttachMentDetailItmStp != null)
                            //        {
                            //            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //            dtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            //        }
                            //        else
                            //        {
                            //            dtAttachment.Columns.Add("id", typeof(string));
                            //            dtAttachment.Columns.Add("file_name", typeof(string));
                            //            dtAttachment.Columns.Add("file_path", typeof(string));
                            //            dtAttachment.Columns.Add("file_def", typeof(char));
                            //            dtAttachment.Columns.Add("comp_id", typeof(Int32));

                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (_ShopfloorSetupModel.AttachMentDetailItmStp != null)
                            //        {
                            //            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //            dtAttachment = _ShopfloorSetupModel.AttachMentDetailItmStp as DataTable;
                            //        }
                            //        else
                            //        {
                            //            dtAttachment.Columns.Add("id", typeof(string));
                            //            dtAttachment.Columns.Add("file_name", typeof(string));
                            //            dtAttachment.Columns.Add("file_path", typeof(string));
                            //            dtAttachment.Columns.Add("file_def", typeof(char));
                            //            dtAttachment.Columns.Add("comp_id", typeof(Int32));

                            //        }
                            //    }
                            //    JArray jObject1 = JArray.Parse(_ShopfloorSetupModel.attatchmentdetail);
                            //    for (int i = 0; i < jObject1.Count; i++)
                            //    {
                            //        string flag = "Y";
                            //        foreach (DataRow dr in dtAttachment.Rows)
                            //        {
                            //            string drImg = dr["file_name"].ToString();
                            //            string ObjImg = jObject1[i]["file_name"].ToString();
                            //            if (drImg == ObjImg)
                            //            {
                            //                flag = "N";
                            //            }
                            //        }
                            //        if (flag == "Y")
                            //        {

                            //            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            //            if (!string.IsNullOrEmpty((_ShopfloorSetupModel.shfl_id).ToString()))
                            //            {
                            //                dtrowAttachment1["id"] = _ShopfloorSetupModel.shfl_id;
                            //            }
                            //            else
                            //            {
                            //                dtrowAttachment1["id"] = "0";
                            //            }
                            //            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            //            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            //            dtrowAttachment1["file_def"] = "Y";
                            //            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                            //            dtAttachment.Rows.Add(dtrowAttachment1);
                            //        }
                            //    }
                            //    //if (Session["TransType"].ToString() == "Update")
                            //    if (_ShopfloorSetupModel.TransType == "Update")
                            //    {
                            //        string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //        if (Directory.Exists(AttachmentFilePath))
                            //        {
                            //            string SFS_CODE = string.Empty;
                            //            if (!string.IsNullOrEmpty((_ShopfloorSetupModel.shfl_id).ToString()))
                            //            {
                            //                SFS_CODE = (_ShopfloorSetupModel.shfl_id).ToString();

                            //            }
                            //            else
                            //            {
                            //                SFS_CODE = "0";
                            //            }
                            //            string[] filePaths = Directory.GetFiles(AttachmentFilePath, comp_id + branch_id + SFS_CODE.Replace("/", "") + "*");

                            //            foreach (var fielpath in filePaths)
                            //            {
                            //                string flag = "Y";
                            //                foreach (DataRow dr in dtAttachment.Rows)
                            //                {
                            //                    string drImgPath = dr["file_path"].ToString();
                            //                    if (drImgPath == fielpath.Replace("/",@"\"))
                            //                    {
                            //                        flag = "N";
                            //                    }
                            //                }
                            //                if (flag == "Y")
                            //                {
                            //                    System.IO.File.Delete(fielpath);
                            //                }
                            //            }
                            //        }
                            //    }
                            Attachments = dtAttachment;
                            //}
                            /*-----------------Attachment Section End------------------------*/

                            //Message = _ShopfloorSetup_ISERVICES.insertShopfloorDetail(dtShopFloorSetup, dtShopFloorCapacity, SystemDetail);
                            //splitmsg = Message.Split('-');
                            SaveMessage = _ShopfloorSetup_ISERVICES.insertShopfloorDetail(dtShopFloorSetup, dtShopFloorCapacity, Attachments, SystemDetail);
                            splitmsg = SaveMessage.Split('-');
                            string SFSCode = splitmsg[1].ToString().Trim();
                         
                            string Message1 = splitmsg[0].ToString().Trim();
                            if (Message1 == "Data_Not_Found")
                            {
                                //getDocumentName(); /* To set Title*/
                                //PageName = title.Replace(" ", "");
                                //var a = SaveMessage.Split(',');
                                var msg = Message1.Replace("_", " ") + " " + SFSCode+" in "+PageName;//InvoiceAdjustmentNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _ShopfloorSetupModel.Message = Message1.Split(',')[0].Replace("_", "");
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            /*-----------------Attachment Section Start------------------------*/
                            if (Message1 == "Save")
                            
                            {
                                string Guid = "";
                                //if (Session["Guid"] != null)
                                if (attachData != null)
                                {
                                    if (attachData.Guid != null)
                                    {
                                        Guid = attachData.Guid;
                                    }
                                }
                                string guid = Guid;
                                var comCont = new CommonController(_Common_IServices);
                                comCont.ResetImageLocation(CompID, branch_id, guid, PageName, SFSCode, _ShopfloorSetupModel.TransType, Attachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, comp_id + branch_id + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in Attachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = comp_id + branch_id + SFSCode + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            if (Message1 == "Update" || Message1 == "Save")
                              {
                                //Session["Message"] = "Save";
                                //Session["Command"] = "EditNew";
                                //Session["TransType"] = "Update";
                                //Session["BtnName"] = "BtnSave";
                                //ViewBag.Message = Session["Message"].ToString();
                                //Session["SaveUpd"] = "AfterSaveUpdate";
                                //Session["br_id"] = Session["BranchId"].ToString();
                                ////Session["shfl_id"] = splitmsg[1].ToString().Trim();
                                //Session["shfl_id"] = SFSCode;
                                _ShopfloorSetupModel.Message = "Save";
                                _ShopfloorSetupModel.sh_id = SFSCode;
                                _ShopfloorSetupModel.SaveUpd = "AfterSaveUpdate";
                                _ShopfloorSetupModel.TransType = "Update";
                                _ShopfloorSetupModel.BtnName = "BtnSave";
                                _ShopfloorSetupModel.Command = "EditNew";
                                _ShopfloorSetupModel.shfl_id = Convert.ToInt32(SFSCode);
                                //_ShopfloorSetupModel = null;
                                TempData["ModelData"] = _ShopfloorSetupModel;
                                UrlModel SaveModel = new UrlModel();
                                SaveModel.bt = _ShopfloorSetupModel.BtnName;
                                SaveModel.SV_No = _ShopfloorSetupModel.sh_id;
                                SaveModel.tp = _ShopfloorSetupModel.TransType;
                                return RedirectToAction("ShopfloorSetupDetail", SaveModel);
                            }
                            /*if (splitmsg[0].ToString().Trim() == "Duplicate")*/ 
                            if (Message1 == "Duplicate")
                            {
                                //_ShopfloorSetupModel.shfl_name = shfl_name;
                                _ShopfloorSetupModel.shfl_name = _ShopfloorSetupModel.shfl_name;
                                _ShopfloorSetupModel.sh_id = _ShopfloorSetupModel.shfl_id.ToString(); ;
                                _ShopfloorSetupModel.Message = "Duplicate";
                                _ShopfloorSetupModel.Command = "Add";
                                _ShopfloorSetupModel.TransType = "Update";
                                _ShopfloorSetupModel.BtnName = "BtnAddNew";
                               ViewBag.ItemData = itemtable(_ShopfloorSetupModel); // added By Nitesh 16102023 11:55 for item Table when msg Duplicate
                                UrlModel Dulicate_Value = new UrlModel();
                                Dulicate_Value.bt = "BtnRefresh";
                                //SaveModel.SV_No = _ShopfloorSetupModel.shfl_name;
                                Dulicate_Value.SV_No = _ShopfloorSetupModel.shfl_id.ToString();
                                Dulicate_Value.tp ="Update";
                                Dulicate_Value.Cmd = "Refresh";                             
                                TempData["ModelData"] = _ShopfloorSetupModel;
                                return RedirectToAction("ShopfloorSetupDetail", Dulicate_Value);
                            }
                            return RedirectToAction("ShopfloorSetupDetail");
                         case "Refresh":
                            // Session["BtnName"] = "BtnRefresh";
                            // Session["Command"] = command;
                            // Session["TransType"] = "Refresh";
                            //Session["Message"] = "";
                            // Session["AppStatus"] = "";
                            // Session.Remove("br_id");
                            // Session.Remove("SaveUpd");
                            // Session.Remove("shfl_name");
                            // Session.Remove("shfl_loc");
                            // Session.Remove("shfl_remarks");
                            // _ShopfloorSetupModel = null;

                            ShopfloorSetupModel RefreshModel = new ShopfloorSetupModel();
                            RefreshModel.Command = command;
                            RefreshModel.BtnName = "BtnRefresh";
                            RefreshModel.TransType = "Refresh";
                            TempData["ModelData"] = RefreshModel;
                            UrlModel refesh = new UrlModel();
                            refesh.tp = "Refresh";
                            refesh.bt = "BtnRefresh";
                            refesh.Cmd = command;
                            return RedirectToAction("ShopfloorSetupDetail", refesh);
                        case "BacktoList":
                            //Session.Remove("Message");
                            //Session.Remove("TransType");
                            //Session.Remove("Command");
                            //Session.Remove("BtnName");
                            //Session.Remove("DocumentStatus");
                            return RedirectToAction("ShopfloorSetup", "ShopfloorSetup");
                    }
                }
                else
                {
                    RedirectToAction("");
                }
                return RedirectToAction("ShopfloorSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ShopfloorSetupModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ShopfloorSetupModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ShopfloorSetupModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(comp_id+ branch_id, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //[HttpPost]
        //public ActionResult GetSOItemList(ShopfloorSetupModel _ShopfloorSetupModel)
        //{
        //    JsonResult DataRows = null;
        //    string SOItmName = string.Empty;
        //    //Dictionary<string, string> SuppList = new Dictionary<string, string>();
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(_ShopfloorSetupModel.SO_ItemName))
        //        {
        //            SOItmName = "0";
        //        }
        //        else
        //        {
        //            SOItmName = _ShopfloorSetupModel.SO_ItemName;
        //        }


        //        DataSet SOItmList = _ShopfloorSetup_ISERVICES.GetSOItmListDL(Comp_ID, Br_ID, SOItmName);

        //        //DataRow DRow = SOItmList.Tables[0].NewRow();
        //        //DRow["Comp_id"] = "0";
        //        //DRow["comp_nm"] = "---Select---";
        //        //SOItmList.Tables[0].Rows.InsertAt(DRow, 0);

        //        DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return DataRows;
        //}
        [HttpPost]
        public JsonResult GetSOItemUOM(string Itm_ID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet result = _ShopfloorSetup_ISERVICES.GetSOItemUOMDL(Itm_ID, Comp_ID);
                //ViewBag.itemid = Convert.ToString(result.Tables[0].Rows[0]["uom_id"]);//
                //ViewBag.uom_name = Convert.ToString(result.Tables[0].Rows[0]["uom_Alias"]);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
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
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Shopflore_Model _attachmentModel = new Shopflore_Model();
                //string TransType = "";
                //string SfsCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["ItemCode"] != null)
                //{
                //    ItemCode = Session["ItemCode"].ToString();
                //}
               
                //if (Session["shfl_id"] != null)
                //{
                //    SfsCode = Session["shfl_id"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _attachmentModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                branch_id = Session["BranchId"].ToString();
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, comp_id+branch_id, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _attachmentModel.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _attachmentModel.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = _attachmentModel;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
    } /*-----------------Attachment Section End------------------------*/

}