using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.BusinessLayer.WarehouseSetup;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.WarehouseSetup;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
//All Session remove by shubham maurya on30-11-2022 for using model
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.WarehouseSetup
{
    public class WarehouseSetupController : Controller
    {
        string comp_id, userid, branchId, language;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string DocumentMenuId = "103175",title;
        WarehouseSetupModel _WarehouseSetupModel;
        WarehouseSetup_ISERVICES _warehouseSetup_ISERVICES;
        SupplierDetail_ISERVICES _SupplierDetail_ISERVICES;
        Common_IServices _Common_IServices;
        public WarehouseSetupController(Common_IServices _Common_IServices,WarehouseSetup_ISERVICES _warehouseSetup_ISERVICES, SupplierDetail_ISERVICES _SupplierDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._warehouseSetup_ISERVICES = _warehouseSetup_ISERVICES;
            this._SupplierDetail_ISERVICES = _SupplierDetail_ISERVICES;

    }
        // GET: BusinessLayer/WarehouseSetup
        public ActionResult WarehouseSetup(string wh_name, string wh_type)
        {
            try
            {
                CommonPageDetails();
                WarehouseSetupModel _WarehouseSetupModel = new WarehouseSetupModel();
                //Session["TransType"] = "";
                //Session["Message"] = "New";
                _WarehouseSetupModel.TransType = "";
                _WarehouseSetupModel.Message= "New";              
                if (wh_type == null)
                {
                    wh_type = "";
                }
                if (wh_name == null)
                {
                    wh_name = "";
                }
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (TempData["ListFilterData"] != null)
                {
                    var ItemListFilter = TempData["ListFilterData"].ToString();
                    if (ItemListFilter != "" && ItemListFilter != null)
                    {
                        _WarehouseSetupModel.ListFilterData = ItemListFilter;
                        var a = ItemListFilter.Split(',');
                        wh_type = a[0].Trim();
                        wh_name = a[1].Trim();
                        _WarehouseSetupModel.WarehouseName = wh_name;
                        _WarehouseSetupModel.WarehouseTypefilter = wh_type;
                        DataTable dtwarehouse = new DataTable();
                        _WarehouseSetupModel.SerchData = "Y";
                        //dtwarehouse = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, wh_name, comp_id).Tables[0];
                        //if (_WarehouseSetupModel.WhList == null)
                        //{
                        //    _WarehouseSetupModel.WhList = dtwarehouse;
                        //}

                    }
                }
                    GetAlldata(_WarehouseSetupModel, wh_type, wh_name);
                //ViewBag.MenuPageName = getDocumentName();
                _WarehouseSetupModel.Title = title;

                //Session["WHSearch"] = "0";
                _WarehouseSetupModel.WHSearch = "0";
                return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupList.cshtml", _WarehouseSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        private void GetAlldata(WarehouseSetupModel _WarehouseSetupModel,string wh_type, string wh_name)/***Added By Nitesh 19-03-2024 ***/
        {
            if (Session["compid"] != null)
            {
                comp_id = Session["compid"].ToString();
            }
          //  wh_type = "O";
            DataSet dtwarehouse = new DataSet();
            dtwarehouse = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, wh_name, comp_id);
            //if (ViewBag.WhList == null)
            if (_WarehouseSetupModel.WhList == null)
            {
                //ViewBag.WhList = dtwarehouse;
                _WarehouseSetupModel.WhList = dtwarehouse.Tables[0];
            }

           
            DataTable dtnamelist = new DataTable();
            //dtnamelist = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, "", comp_id).Tables[1];
            if(_WarehouseSetupModel.SerchData=="Y")
            {
                dtnamelist = dtwarehouse.Tables[2];
            }
            else
            {
                dtnamelist = dtwarehouse.Tables[1];
            }
            List<wh_namelist> _Namelists = new List<wh_namelist>();
            //foreach (DataRow dr in dtnamelist.Rows)
            foreach (DataRow dr in dtnamelist.Rows)
            {
                wh_namelist _wh_Namelist = new wh_namelist();
                _wh_Namelist.WareH_id = Convert.ToInt32(dr["wh_id"].ToString());
                _wh_Namelist.wareH_name = dr["wh_name"].ToString();
                _Namelists.Add(_wh_Namelist);

            }
            _Namelists.Insert(0, new wh_namelist() { WareH_id = 0, wareH_name = "All" });
            _WarehouseSetupModel.wh_Namelists = _Namelists;
        }
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult WarehouseSetuplist(string wh_name, string wh_type)
        {
            try
            {
                CommonPageDetails();
                _WarehouseSetupModel = new WarehouseSetupModel();
                //ViewBag.WhList = null;
                _WarehouseSetupModel.WhList = null;
                if (wh_type == null)
                {
                    wh_type = "";
                }
                if (wh_name == null)
                {
                    wh_name = "";

                }
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                DataTable dtwarehouse = new DataTable();

                dtwarehouse = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, wh_name, comp_id).Tables[0];
                //if (ViewBag.WhList == null)
                if (_WarehouseSetupModel.WhList == null)
                {
                    //Session["WHSearch"] = "WH_Search";
                    _WarehouseSetupModel.WHSearch= "WH_Search";
                    //ViewBag.WhList = dtwarehouse;
                    _WarehouseSetupModel.WhList= dtwarehouse;
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialWarehouseList.cshtml", _WarehouseSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult WarehouseSetupSave(WarehouseSetupModel _WarehouseSetupModel,string  Command)
        {

            try
            {
                if (_WarehouseSetupModel.DeleteCommand != null)
                {
                    Command = _WarehouseSetupModel.DeleteCommand;
                }
                switch (Command)
                {
                    case "AddNew":
                        //Session["details"] = null;
                        _WarehouseSetupModel.hdnSavebtn = null;
                        _WarehouseSetupModel.details = null;
                        //Session["TransType"] = "Save";
                        _WarehouseSetupModel.TransType= "Save";
                        //Session["btnName"] = "BtnAddNew";
                        _WarehouseSetupModel.BtnName= "BtnAddNew";
                        _WarehouseSetupModel.wh_id = 0;
                        //Session["wh_id"] = null;
                        //Session["Message"] = "New";
                        _WarehouseSetupModel.Message = "New";
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("AddWarehouseSetupDetail");
                    case "Refresh":
                        //Session["details"] = null;
                        _WarehouseSetupModel.hdnSavebtn = null;
                        _WarehouseSetupModel.details = null;
                        _WarehouseSetupModel.TransType= "Refresh";
                        _WarehouseSetupModel.BtnName= "BtnRefresh";
                        //Session["TransType"] = "Refresh";
                        //Session["btnName"] = "BtnRefresh";
                        //Session["wh_id"] = null;
                        _WarehouseSetupModel.wh_id = 0;
                        //Session["Message"] = "New";
                        _WarehouseSetupModel.Message = "New";
                        ViewBag.VBRoleList = GetRoleList();
                        TempData["Modeldata"] = _WarehouseSetupModel;
                        TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                        return RedirectToAction("AddWarehouseSetupDetail");
                    case "Save":
                        //Session["Command"] = Command;
                        _WarehouseSetupModel.Command = Command;
                        if (_WarehouseSetupModel.TransType == null)
                        {
                            _WarehouseSetupModel.TransType = "Save";
                        }
                        CommonPageDetails();
                        if (ModelState.IsValid)
                        {
                            InsertWareHouse(_WarehouseSetupModel);
                            DataSet ds = new DataSet();                        
                            string wh_type="";
                            string warehouse_id="";
                            if (_WarehouseSetupModel.Message == "DataNotFound")
                            {
                                _WarehouseSetupModel.hdnSavebtn = null;
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //if (Session["wh_id"] != null)
                            if (_WarehouseSetupModel.wh_id != 0)
                            {
                                warehouse_id = _WarehouseSetupModel.wh_id.ToString();
                            }                          
                            if (Session["compid"] != null)
                            {
                                comp_id = Session["compid"].ToString();
                            }
                            ds = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, warehouse_id, comp_id);
                            //if (ViewBag.CustomerBranchList == null)
                            if (_WarehouseSetupModel.CustomerBranchList == null)
                            {
                                if (_WarehouseSetupModel.Message != null)
                                {
                                    if (_WarehouseSetupModel.Message.ToString() != "Duplicate")
                                    {
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            //ViewBag.CustomerBranchList = ds.Tables[1];
                                            _WarehouseSetupModel.CustomerBranchList = ds.Tables[1];
                                        }
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            _WarehouseSetupModel.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                            _WarehouseSetupModel.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                            _WarehouseSetupModel.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                            _WarehouseSetupModel.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                                        }
                                    }
                                }                                                                                         
                            }
                            
                            if (_WarehouseSetupModel.Message.ToString() == "Duplicate")
                            {
                                
                                _WarehouseSetupModel.hdnSavebtn = null;
                                wh_type = "";
                                warehouse_id = "";
                                //if (Session["wh_id"] != null)
                                if (_WarehouseSetupModel.wh_id != 0)
                                {
                                    warehouse_id = _WarehouseSetupModel.wh_id.ToString();
                                }
                                if (Session["compid"] != null)
                                {
                                    comp_id = Session["compid"].ToString();
                                }
                                ds = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, warehouse_id, comp_id);
                                if (_WarehouseSetupModel.wh_id != 0)
                                {
                                    _WarehouseSetupModel.Command = "Edit";
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        //ViewBag.CustomerBranchList = ds.Tables[1];
                                        _WarehouseSetupModel.CustomerBranchList = ds.Tables[1];
                                    }
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        _WarehouseSetupModel.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                        _WarehouseSetupModel.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                        _WarehouseSetupModel.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                        _WarehouseSetupModel.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                                    }
                                }
                                else
                                {
                                    _WarehouseSetupModel.Duplicate = "test";
                                    DataTable dt = new DataTable();
                                    dt = GetBranchList();
                                    //if (ViewBag.CustomerBranchList == null)
                                    if (_WarehouseSetupModel.CustomerBranchList == null)
                                    {
                                        //ViewBag.CustomerBranchList = dt;
                                        _WarehouseSetupModel.CustomerBranchList = dt;
                                    }
                                    _WarehouseSetupModel.Command = "Add";
                                }
                                


                                //Session["BtnName"] = "BtnAddNew";
                                _WarehouseSetupModel.BtnName= "BtnAddNew";
                                //ViewBag.Message = Session["Message"].ToString();
                                
                                _WarehouseSetupModel.Message = _WarehouseSetupModel.Message.ToString();
                                //ViewBag.VBRoleList = GetRoleList();
                                GetAutoCompleteSuppcity(_WarehouseSetupModel);
                                TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                                return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupDetail.cshtml", _WarehouseSetupModel);
                            }
                            else
                            {
                                //ViewBag.VBRoleList = GetRoleList();
                                GetAutoCompleteSuppcity(_WarehouseSetupModel);
                                _WarehouseSetupModel.BtnName = "ToViewDetail";
                                TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                                return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupDetail.cshtml", _WarehouseSetupModel);
                            }
                        }
                        else
                        {
                            //ViewBag.VBRoleList = GetRoleList();
                            TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                            _WarehouseSetupModel = null;
                            return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxDetail.cshtml");
                        }             
                    case "Delete":
                        _WarehouseSetupModel.hdnSavebtn = null;
                        string wh_id;
                        //wh_id = Session["wh_id"].ToString();
                        wh_id = _WarehouseSetupModel.wh_id.ToString();
                        if (Session["compid"] != null)
                        {
                            comp_id = Session["compid"].ToString();
                        }
                        string massage=_warehouseSetup_ISERVICES.Delete_warehousedetails(wh_id, comp_id);
                        if(massage == "exist")
                        {
                            _WarehouseSetupModel.TransType = "Update";
                            _WarehouseSetupModel.BtnName = "BtnSave";
                            //Session["TransType"] = "Update";
                            //Session["BtnName"] = "BtnSave";
                        }
                        else
                        {
                            //Session["details"] = null;
                            _WarehouseSetupModel.details = null;
                            //Session["TransType"] = "Refresh";
                            //Session["btnName"] = "BtnRefresh";
                            _WarehouseSetupModel.TransType = "Refresh";
                            _WarehouseSetupModel.BtnName = "BtnRefresh";
                            //Session["wh_id"] = null;
                            _WarehouseSetupModel.wh_id = 0;
                        }
                        //Session["Message"] = massage;
                        _WarehouseSetupModel.Message= massage;
                        ViewBag.VBRoleList = GetRoleList();
                        TempData["Modeldata"] = _WarehouseSetupModel;
                        TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                        return RedirectToAction("AddWarehouseSetupDetail");

                    case "Edit":
                        _WarehouseSetupModel.hdnSavebtn = null;
                        _WarehouseSetupModel.Message = "";
                        _WarehouseSetupModel.Command= Command;
                        _WarehouseSetupModel.TransType= "Update";
                        _WarehouseSetupModel.BtnName= "BtnEdit";
                        //Session["Message"] = "";
                        //Session["Command"] = Command;                   
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";
                        GetAutoCompleteSuppcity(_WarehouseSetupModel);
                        TempData["Modeldata"] = _WarehouseSetupModel;
                        TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                        ViewBag.VBRoleList = GetRoleList();
                        return RedirectToAction("AddWarehouseSetupDetail");
                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        _WarehouseSetupModel.hdnSavebtn = null;
                        _WarehouseSetupModel.Message = null;
                        //Session.Remove("Message");
                        _WarehouseSetupModel.TransType = null;
                        //Session.Remove("TransType");
                        _WarehouseSetupModel.Command = null;
                        //Session.Remove("Command");
                        _WarehouseSetupModel.BtnName = null;
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        _WarehouseSetupModel.wh_id = 0;
                        //Session.Remove("wh_id");
                        TempData["ListFilterData"] = _WarehouseSetupModel.ListFilterData1;
                        return RedirectToAction("WarehouseSetup", "WarehouseSetup");
                    default:
                        return new EmptyResult();        
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _warehouseSetup_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
              catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult InsertWareHouse(WarehouseSetupModel _WarehouseSetupModel)
        {

            try
            {

                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }

                DataTable WarehouseDetail = new DataTable();
                DataTable WarehouseBranch = new DataTable();

                DataTable WarehouseDt = new DataTable();
                WarehouseDt.Columns.Add("TransType", typeof(string));
                WarehouseDt.Columns.Add("comp_id", typeof(int));
                WarehouseDt.Columns.Add("wh_id", typeof(int));
                WarehouseDt.Columns.Add("wh_name", typeof(string));
                WarehouseDt.Columns.Add("wh_city", typeof(int));
                WarehouseDt.Columns.Add("wh_dist", typeof(int));
                WarehouseDt.Columns.Add("wh_state", typeof(int));
                WarehouseDt.Columns.Add("wh_cntry", typeof(int));
                WarehouseDt.Columns.Add("wh_address", typeof(string));
                WarehouseDt.Columns.Add("wh_type", typeof(string));
                WarehouseDt.Columns.Add("user_id", typeof(int));
                WarehouseDt.Columns.Add("Reject", typeof(string));
                WarehouseDt.Columns.Add("Rework", typeof(string));
                DataRow WarehouseDtrow = WarehouseDt.NewRow();

                //WarehouseDtrow["TransType"] = Session["TransType"].ToString();
                WarehouseDtrow["TransType"] = _WarehouseSetupModel.TransType.ToString();
                WarehouseDtrow["comp_id"] = comp_id;
                if (!string.IsNullOrEmpty(_WarehouseSetupModel.wh_id.ToString()))
                {
                    WarehouseDtrow["wh_id"] = _WarehouseSetupModel.wh_id;
                }
                else
                {
                    WarehouseDtrow["wh_id"] = "0";
                }
                
                WarehouseDtrow["wh_name"] = _WarehouseSetupModel.WarehouseName;
                if (!string.IsNullOrEmpty(_WarehouseSetupModel.CityAndPIN))
                {
                    WarehouseDtrow["wh_city"] = _WarehouseSetupModel.CityAndPIN;
                }
                else
                {
                    WarehouseDtrow["wh_city"] = "0";
                }
                if (!string.IsNullOrEmpty(_WarehouseSetupModel.ware_dist))
                {
                    WarehouseDtrow["wh_dist"] = _WarehouseSetupModel.ware_dist;
                }
                else
                {
                    WarehouseDtrow["wh_dist"] = "0";
                }
                if (!string.IsNullOrEmpty(_WarehouseSetupModel.ware_state))
                {
                    WarehouseDtrow["wh_state"] = _WarehouseSetupModel.ware_state;
                }
                else
                {
                    WarehouseDtrow["wh_state"] = "0";
                }
                if (!string.IsNullOrEmpty(_WarehouseSetupModel.ware_Country))
                {
                    WarehouseDtrow["wh_cntry"] = _WarehouseSetupModel.ware_Country;
                }
                else
                {
                    WarehouseDtrow["wh_cntry"] = "0";
                }
          
                WarehouseDtrow["wh_address"] = _WarehouseSetupModel.Address;
                WarehouseDtrow["wh_type"] = _WarehouseSetupModel.WarehouseType;
                WarehouseDtrow["user_id"] = userid;
                if (_WarehouseSetupModel.RejWh)
                {
                    WarehouseDtrow["Reject"] = "Y";
                }
                else
                {
                    WarehouseDtrow["Reject"] = "N";
                }
                if (_WarehouseSetupModel.RewWh)
                {
                    WarehouseDtrow["Rework"] = "Y";
                }
                else
                {
                    WarehouseDtrow["Rework"] = "N";
                }

                WarehouseDt.Rows.Add(WarehouseDtrow);

                WarehouseDetail = WarehouseDt;

                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("wh_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_WarehouseSetupModel.branchId);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    if (!string.IsNullOrEmpty(_WarehouseSetupModel.wh_id.ToString()))
                    {
                        dtrowBrdetails["wh_id"] = _WarehouseSetupModel.wh_id;
                    }
                    else
                    {
                        dtrowBrdetails["wh_id"] = "0";
                    }

                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                WarehouseBranch = dtBranch;
                String SaveMessage = _warehouseSetup_ISERVICES.insertWarehouseDetails(WarehouseDetail, WarehouseBranch);

                string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                if (Message == "Update" || Message == "Save")
                {
                    //Session["Message"] = "Save";
                    _WarehouseSetupModel.Message= "Save";
                    // Session["TaxCode"] = TaxCode;
                    _WarehouseSetupModel.TransType= Message;
                    //Session["TransType"] = Message;
                    _WarehouseSetupModel.BtnName= "ToViewDetail";
                    //Session["BtnName"] = "BtnSave";
                    //Session["wh_id"] = TaxCode;
                    _WarehouseSetupModel.wh_id = Convert.ToInt32(TaxCode);
                }
                if (Message == "Data_Not_Found")
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _WarehouseSetupModel.Title = title;
                    var a = TaxCode.Split('-');
                    var msg = Message.Replace("_", " ") + " " + a[0].Trim()+" in " + _WarehouseSetupModel.Title;
                    //var msg = "Data Not Found" +" "+ a[0].Trim();
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _WarehouseSetupModel.Message = Message.Replace("_", "");
                    return RedirectToAction("ItemDetail");
                }
                if (Message == "Duplicate")
                {
                    //Session["TransType"] = "Duplicate";
                    _WarehouseSetupModel.TransType= "Duplicate";
                    _WarehouseSetupModel.Message= "Duplicate";
                    //Session["Message"] = "Duplicate";

                    //Session["TaxCode"] = TaxCode;
                  //  Session["BtnName"] = "BtnSave";
                }
                // Session["wh_id"] = TaxCode;
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupDetail.cshtml", _WarehouseSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        public ActionResult AddWarehouseSetupDetail(string wh_name,string ListFilterData)
        {
            try
            {
                CommonPageDetails();
                var _WarehouseSetupModel = TempData["Modeldata"] as WarehouseSetupModel;
                if (_WarehouseSetupModel != null)
                {
                    DataTable dt = new DataTable();
                    dt = GetBranchList();
                    //ViewBag.CustomerBranchList = dt;
                    _WarehouseSetupModel.CustomerBranchList= dt;
                    //_WarehouseSetupModel = new WarehouseSetupModel();
                    GetAutoCompleteSuppcity(_WarehouseSetupModel);
                    if (Session["compid"] != null)
                    {
                        comp_id = Session["compid"].ToString();
                    }
                    if (_WarehouseSetupModel.TransType == null)
                    {
                        _WarehouseSetupModel.TransType = "";
                    }
                    string wh_type;
                    wh_type = "";
                    //if (Session["wh_id"] != null)
                    if (_WarehouseSetupModel.wh_id != 0)
                    {
                        //wh_name = Session["wh_id"].ToString();
                        wh_name = _WarehouseSetupModel.wh_id.ToString();
                    }
                    if (ListFilterData != null)
                    {
                        _WarehouseSetupModel.ListFilterData1 = ListFilterData;
                    }
                    else
                    {
                        if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                        {
                            _WarehouseSetupModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                        }
                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    getdetails(_WarehouseSetupModel, wh_type, wh_name, comp_id);

                    if (wh_name == null)
                    {
                        //if(Session["Message"]==null || Session["Message"].ToString() == "")
                        if (_WarehouseSetupModel.Message == null || _WarehouseSetupModel.Message.ToString() == "")
                        {
                            //Session["Message"] = "New";
                            _WarehouseSetupModel.Message = "New";
                        }


                        //Session["Command"] = "Add";
                        _WarehouseSetupModel.Command = "Add";
                        Session["AppStatus"] = 'D';
                        //if (Session["TransType"] == null || Session["TransType"].ToString()=="")
                        if (_WarehouseSetupModel.TransType == null || _WarehouseSetupModel.TransType.ToString() == "")
                        {
                            _WarehouseSetupModel.TransType = "Save";
                        }
                        //if (Session["BtnName"] == null || Session["TransType"].ToString()=="")
                        if (_WarehouseSetupModel.BtnName == null || _WarehouseSetupModel.TransType.ToString() == "")
                        {
                            _WarehouseSetupModel.BtnName = "BtnAddNew";
                        }

                    }
                    if (_WarehouseSetupModel.Command == null)
                    {
                        _WarehouseSetupModel.Command = "Add";
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _WarehouseSetupModel.Title = title;
                    _WarehouseSetupModel.DeleteCommand = null;
                    return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupDetail.cshtml", _WarehouseSetupModel);
                }
                else
                {
                    DataTable dt = new DataTable();
                    var _WarehouseSetupModel1 = new WarehouseSetupModel();
                    dt = GetBranchList();
                    //ViewBag.CustomerBranchList = dt;
                    _WarehouseSetupModel1.CustomerBranchList= dt;
                    
                    GetAutoCompleteSuppcity(_WarehouseSetupModel1);
                    if (Session["compid"] != null)
                    {
                        comp_id = Session["compid"].ToString();
                    }
                    if (_WarehouseSetupModel1.TransType == null)
                    {
                        _WarehouseSetupModel1.TransType = "";
                    }
                    string wh_type;
                    wh_type = "";
                    //if (Session["wh_id"] != null)
                    if (_WarehouseSetupModel1.wh_id != 0)
                    {
                        //wh_name = Session["wh_id"].ToString();
                        wh_name = _WarehouseSetupModel1.wh_id.ToString();
                    }
                    if (ListFilterData != null)
                    {
                        _WarehouseSetupModel1.ListFilterData1 = ListFilterData.ToString();
                    }
                    getdetails(_WarehouseSetupModel1, wh_type, wh_name, comp_id);

                    if (wh_name == null)
                    {
                        //if(Session["Message"]==null || Session["Message"].ToString() == "")
                        if (_WarehouseSetupModel1.Message == null || _WarehouseSetupModel1.Message.ToString() == "")
                        {
                            //Session["Message"] = "New";
                            _WarehouseSetupModel1.Message = "New";
                        }


                        //Session["Command"] = "Add";
                        _WarehouseSetupModel1.Command = "Add";
                        Session["AppStatus"] = 'D';
                        //if (Session["TransType"] == null || Session["TransType"].ToString()=="")
                        if (_WarehouseSetupModel1.TransType == null || _WarehouseSetupModel1.TransType.ToString() == "")
                        {
                            _WarehouseSetupModel1.TransType = "Save";
                        }
                        //if (Session["BtnName"] == null || Session["TransType"].ToString()=="")
                        if (_WarehouseSetupModel1.BtnName == null || _WarehouseSetupModel1.TransType.ToString() == "")
                        {
                            _WarehouseSetupModel1.BtnName = "BtnAddNew";
                        }

                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    if (_WarehouseSetupModel1.BtnName == null)
                    {
                        _WarehouseSetupModel1.BtnName = "ToViewDetail";
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _WarehouseSetupModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/WarehouseSetup/WarehouseSetupDetail.cshtml", _WarehouseSetupModel1);
                }
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = "";
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(comp_id, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public void getdetails(WarehouseSetupModel _WarehouseSetupModel, string wh_type,string wh_name,string comp_id)
        {
            try
            {
                DataSet dt = new DataSet();
                if (wh_name != null)
                {
                    //if (Session["TransType"] != null)
                    if (_WarehouseSetupModel.TransType != null)
                    {
                        //if (Session["TransType"].ToString() != "Update")
                        if (_WarehouseSetupModel.TransType.ToString() != "Update")
                        {
                            //Session["TransType"] = "Update";
                            _WarehouseSetupModel.TransType= "Update";
                            //Session["Message"] = "New";
                            _WarehouseSetupModel.Message= "New";
                            //Session["Command"] = "Add";
                            _WarehouseSetupModel.Command= "Add";
                            Session["AppStatus"] = 'D';

                            //Session["BtnName"] = "BtnSave";
                            //_WarehouseSetupModel.BtnName= "BtnSave";
                            _WarehouseSetupModel.BtnName= "BtnAddNew";
                        }
                        else
                        {
                            if(_WarehouseSetupModel.BtnName == "BtnEdit")
                            {
                                _WarehouseSetupModel.BtnName = "Edit";
                            }
                        }
                    }
                    else
                    {
                        //Session["TransType"] = "";
                        _WarehouseSetupModel.TransType = "";
                    }


                    dt = _warehouseSetup_ISERVICES.GetWarehouseDetails(wh_type, wh_name, comp_id);

                    Boolean  Reject, Rework;
                    if (dt.Tables[0].Rows[0]["Reject"].ToString() == "Y")
                        Reject = true;
                    else
                        Reject = false;
                    if (dt.Tables[0].Rows[0]["Rework"].ToString() == "Y")
                        Rework = true;
                    else
                        Rework = false;
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        if (_WarehouseSetupModel.BtnName != "Edit")
                        {
                            _WarehouseSetupModel.BtnName = "ToViewDetail";
                        }
                        _WarehouseSetupModel.WarehouseName = dt.Tables[0].Rows[0]["wh_name"].ToString();
                        //Session["wh_id"] = dt.Tables[0].Rows[0]["wh_id"].ToString();
                        _WarehouseSetupModel.wh_id = Convert.ToInt32(dt.Tables[0].Rows[0]["wh_id"]);
                        _WarehouseSetupModel.WarehouseType = dt.Tables[0].Rows[0]["wh_type_id"].ToString();
                        _WarehouseSetupModel.Address = dt.Tables[0].Rows[0]["wh_address"].ToString();
                        _WarehouseSetupModel.CityAndPIN = dt.Tables[0].Rows[0]["wh_city"].ToString();
                        if (dt.Tables[0].Rows[0]["wh_dist"].ToString() == "0")
                        {
                            _WarehouseSetupModel.DistrictAndZone = "";
                        }
                        else
                        {
                            _WarehouseSetupModel.DistrictAndZone = dt.Tables[0].Rows[0]["wh_dist"].ToString();
                        }
                        if (dt.Tables[0].Rows[0]["wh_state"].ToString() == "0")
                        {
                            _WarehouseSetupModel.StateAndProvince = "";
                        }
                        else
                        {
                            _WarehouseSetupModel.StateAndProvince = dt.Tables[0].Rows[0]["wh_state"].ToString();
                        }
                        if (dt.Tables[0].Rows[0]["wh_cntry"].ToString() == "0")
                        {
                            _WarehouseSetupModel.Country = "";
                        }
                        else
                        {
                            _WarehouseSetupModel.Country = dt.Tables[0].Rows[0]["wh_cntry"].ToString();
                        }



                        _WarehouseSetupModel.wh_id = Convert.ToInt32(dt.Tables[0].Rows[0]["wh_id"].ToString());
                        _WarehouseSetupModel.create_id = dt.Tables[0].Rows[0]["create_id"].ToString();
                        _WarehouseSetupModel.creat_dt = dt.Tables[0].Rows[0]["create_dt"].ToString();
                        _WarehouseSetupModel.mod_id = dt.Tables[0].Rows[0]["mod_id"].ToString();
                        _WarehouseSetupModel.mod_dt = dt.Tables[0].Rows[0]["mod_dt"].ToString();
                        _WarehouseSetupModel.RejWh = Reject;
                        _WarehouseSetupModel.RewWh = Rework;
                        //ViewBag.CustomerBranchList = dt.Tables[1];
                        _WarehouseSetupModel.CustomerBranchList= dt.Tables[1];
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
              
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(comp_id, branchId, userid, DocumentMenuId, language);
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //[HttpPost]
        public ActionResult GetAutoCompleteSuppcity(WarehouseSetupModel _warehouseSetupModel)
        {
            try
            {
                string GroupName = string.Empty;
                Dictionary<string, string> SuppCity = new Dictionary<string, string>();

                try
                {
                    if (string.IsNullOrEmpty(Convert.ToString(_warehouseSetupModel.ddlcity)))
                    {
                        GroupName = "0";
                    }
                    else
                    {
                        GroupName = Convert.ToString(_warehouseSetupModel.ddlcity);
                    }
                    SuppCity = _SupplierDetail_ISERVICES.SuppCityDAL(GroupName);

                    List<CityAndPIN> _wareCityList = new List<CityAndPIN>();
                    foreach (var dr in SuppCity)
                    {
                        CityAndPIN _WareCityname = new CityAndPIN();
                        _WareCityname.CityAndPin_id = dr.Key;
                        _WareCityname.CityAndPin_val = dr.Value;
                        _wareCityList.Add(_WareCityname);
                    }
                    _warehouseSetupModel._CityAndPIN_list = _wareCityList;
                }
                catch (Exception ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, ex);
                    return Json("ErrorPage");
                }
                return Json(SuppCity.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}