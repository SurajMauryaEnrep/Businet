
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OverheadParameters;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OverheadParameters;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OverheadParameters.OverheadParametersModel;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.OverheadParameters
{
    public class OverheadParametersController : Controller
    {
        string CompID,BranchId, userid, language = String.Empty;
        string DocumentMenuId = "105105112", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        OverheadParameters_ISERVICES _OverheadParameters_ISERVICES;
        //OverheadParametersModel _OverheadParametersModel;
        public OverheadParametersController(Common_IServices _Common_IServices, OverheadParameters_ISERVICES _OverheadParameters_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._OverheadParameters_ISERVICES = _OverheadParameters_ISERVICES;
        }
        // GET: ApplicationLayer/OverheadParameters
        public ActionResult OverheadParameters(string Typ,string cmd,string bt,string ohd_id)
        {
           
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BranchId = Session["BranchId"].ToString();
                }
                OverheadParametersModel OhdParamModel = new OverheadParametersModel();
                var OhdParamModel1 = TempData["ModelData"] as OverheadParametersModel;
                if (OhdParamModel1 != null)
                {
                    DataTable dt = new DataTable();
                    List<UOMList> _uomList = new List<UOMList>();
                    dt = GetUOMList();

                    foreach (DataRow dr in dt.Rows)
                    {
                        UOMList uomList = new UOMList();

                        uomList.uom_id = dr["uom_id"].ToString();
                        uomList.uom_val = dr["uom_name"].ToString();
                        _uomList.Add(uomList);
                    }
                    _uomList.Insert(0, new UOMList() { uom_id = "0", uom_val = "---Select---" });
                    OhdParamModel1._uomList = _uomList;

                    //if (Session["BtnName"] == null || Session["BtnName"].ToString() == "")
                    //{
                    //    Session["TransType"] = "Refresh";
                    //    Session["BtnName"] = "BtnSave";
                    //    Session["Command"] = "Add";
                    //}

                    //Get OHD Details for Edit on this list method on same page
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (OhdParamModel1.TransType == "Update" || OhdParamModel1.TransType == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        // for refresh fields on page loading after click on edit button
                        //string ohd_exp_id = "";
                        //if (Session["OHD_ID"] != null)
                        //{
                        //    //ohd_exp_id = Session["OHD_ID"].ToString();
                        //    
                        //    //Session["OHD_ID"] = null;

                        //}
                        //else
                        //{
                        //    //Session["BtnName"] = "Refresh";
                        //}

                        //if (ohd_exp_id!=null && ohd_exp_id != "")
                        //{
                        var ohd_exp_id = OhdParamModel1.OHD_ID;
                        if (ohd_exp_id != null && ohd_exp_id != "0")
                        {
                            DataSet ds = _OverheadParameters_ISERVICES.getOHDdetailsEdit(CompID, ohd_exp_id);
                            OhdParamModel1.ohd_exp_id = Convert.ToInt32(ds.Tables[0].Rows[0]["ohd_exp_id"]);
                            OhdParamModel1.ohd_exp_name = ds.Tables[0].Rows[0]["ohd_exp_name"].ToString();
                            OhdParamModel1.uom_id = Convert.ToInt32(ds.Tables[0].Rows[0]["uom_id"]);
                            OhdParamModel1.ohd_exp_remarks = ds.Tables[0].Rows[0]["ohd_exp_remarks"].ToString();
                        }
                     
                        //if (Session["TransType"].ToString() == "Edit")
                        //    Session["BtnName"] = "BtnEdit";
                        //for duplicate samedata stable in textbox
                        // one condition also refer above in IF Condition
                        //if (Session["OHD_NAME"] != null)
                        //{
                        //    OhdParamModel1.ohd_exp_name = Session["OHD_NAME"].ToString();
                        //}
                        //}

                    }
                    //check for duplicate on viewbag
                    //if (Session["Message"] != null)
                    //{
                    //    ViewBag.Message = Session["Message"].ToString();
                    //}
                    if (OhdParamModel1.BtnName == null && OhdParamModel1.Command == null)
                    {
                        OhdParamModel1.BtnName = "BtnNew";
                        OhdParamModel1.TransType = "Refresh";
                        OhdParamModel1.Command = "Add";
                    }
                    ViewBag.BindOverheadDetails = GetOverheadExpParamDetails();

                    ViewBag.MenuPageName = getDocumentName();
                    OhdParamModel1.Title = title;
                    ViewBag.VBRoleList = GetRoleList();

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OverheadParameters/OverheadParameters.cshtml", OhdParamModel1);
                }
                else
                { /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BranchId = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    //string ex_name, string remarks,string uom
                    if (ohd_id != null)
                    {
                        OhdParamModel.OHD_ID = ohd_id;                      
                    }
                    OhdParamModel.TransType = Typ;
                    OhdParamModel.Command = cmd;
                    OhdParamModel.BtnName = bt;                   
                    DataTable dt = new DataTable();
                    List<UOMList> _uomList = new List<UOMList>();
                    dt = GetUOMList();

                    foreach (DataRow dr in dt.Rows)
                    {
                        UOMList uomList = new UOMList();

                        uomList.uom_id = dr["uom_id"].ToString();
                        uomList.uom_val = dr["uom_name"].ToString();
                        _uomList.Add(uomList);
                    }
                    _uomList.Insert(0, new UOMList() { uom_id = "0", uom_val = "---Select---" });
                    OhdParamModel._uomList = _uomList;

                    //if (Session["BtnName"] == null || Session["BtnName"].ToString() == "")
                    //{
                    //    Session["TransType"] = "Refresh";
                    //    Session["BtnName"] = "BtnSave";
                    //    Session["Command"] = "Add";
                    //}

                    //Get OHD Details for Edit on this list method on same page
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (OhdParamModel.TransType == "Update" || OhdParamModel.TransType == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        // for refresh fields on page loading after click on edit button
                        //string ohd_exp_id = "";
                        //if (Session["OHD_ID"] != null)
                        //{
                        //    //ohd_exp_id = Session["OHD_ID"].ToString();
                        //    
                        //    //Session["OHD_ID"] = null;

                        //}
                        //else
                        //{
                        //    //Session["BtnName"] = "Refresh";
                        //}

                        //if (ohd_exp_id!=null && ohd_exp_id != "")
                        //{               


                            var ohd_exp_id = OhdParamModel.OHD_ID;
                        if (ohd_exp_id != null && ohd_exp_id != "0")
                        {
                            DataSet ds = _OverheadParameters_ISERVICES.getOHDdetailsEdit(CompID, ohd_exp_id);
                            OhdParamModel.ohd_exp_id = Convert.ToInt32(ds.Tables[0].Rows[0]["ohd_exp_id"]);
                            OhdParamModel.ohd_exp_name = ds.Tables[0].Rows[0]["ohd_exp_name"].ToString();
                            OhdParamModel.uom_id = Convert.ToInt32(ds.Tables[0].Rows[0]["uom_id"]);
                            OhdParamModel.ohd_exp_remarks = ds.Tables[0].Rows[0]["ohd_exp_remarks"].ToString();
                        }
                       
                        //if (Session["TransType"].ToString() == "Edit")
                        //    Session["BtnName"] = "BtnEdit";
                        //for duplicate samedata stable in textbox
                        // one condition also refer above in IF Condition
                        //if (Session["OHD_NAME"] != null)
                        //{
                        //    OhdParamModel.ohd_exp_name = Session["OHD_NAME"].ToString();
                        //}
                        //}

                    }
                    //check for duplicate on viewbag
                    //if (Session["Message"] != null)
                    //{
                    //    ViewBag.Message = Session["Message"].ToString();
                    //}
                    if (OhdParamModel.BtnName == null && OhdParamModel.Command == null)
                    {
                        OhdParamModel.BtnName = "BtnSave";
                        OhdParamModel.TransType = "Refresh";
                        OhdParamModel.Command = "Add";
                    }
                    ViewBag.BindOverheadDetails = GetOverheadExpParamDetails();

                    ViewBag.MenuPageName = getDocumentName();
                    OhdParamModel.Title = title;
                    ViewBag.VBRoleList = GetRoleList();

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OverheadParameters/OverheadParameters.cshtml", OhdParamModel);
                }
                
                
            }
             
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            
        }
        
        public ActionResult OverheadParamSave(OverheadParametersModel OhdParamModel, string command)
        {
            try
            { /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                var Typ = "";
                var cmd = "";
                var bt = "";
                var ohd_id = "";
                string ohd_exp_id1 = string.Empty;
                if (OhdParamModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Add":
                        //Session["Message"] = null;
                        //Session["OHD_ID"] = "";
                        // Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        //Session["OHD_NAME"] = null;
                        OverheadParametersModel adddnew = new OverheadParametersModel();
                        adddnew.Command = "New";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = adddnew;
                         Typ = adddnew.Command;
                         cmd = adddnew.TransType;
                         bt = adddnew.BtnName;
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BranchId = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            //_ShopfloorSetupModel.sh_id = _ShopfloorSetupModel.shfl_id.ToString();
                            if (OhdParamModel.ohd_exp_id != 0)
                                return RedirectToAction("EditOhdParam", new { ohd_exp_id1 = OhdParamModel.ohd_exp_id });
                            else
                                adddnew.Command = "Refresh";
                            adddnew.TransType = "Refresh";
                            adddnew.BtnName = "Refresh";
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("OverheadParameters", "OverheadParameters", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("OverheadParameters", "OverheadParameters",new { Typ, cmd, bt });

                    case "Edit":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BranchId = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("EditOhdParam", new { ohd_exp_id1 = OhdParamModel.ohd_exp_id});
                        }
                        
                        /*End to chk Financial year exist or not*/
                       //Session["TransType"] = "Update";
                       //Session["Command"] = command;
                       //Session["BtnName"] = "BtnAddNew";
                       //Session["Message"] = null;

                       //Session["OHD_ID"] = OhdParamModel.ohd_exp_id;
                       OhdParamModel.Command = command;
                        OhdParamModel.TransType = "Update";
                        OhdParamModel.BtnName = "BtnAddNew";
                        OhdParamModel.OHD_ID = OhdParamModel.ohd_exp_id.ToString();
                        TempData["MOdelData"] = OhdParamModel;
                        Typ = OhdParamModel.Command;
                        cmd = OhdParamModel.TransType;
                        bt = OhdParamModel.BtnName;
                        ohd_id = OhdParamModel.OHD_ID.ToString(); 
                        return RedirectToAction("OverheadParameters", new { Typ, cmd, bt, ohd_id });

                    case "Delete":
                      
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        OHDDelete(OhdParamModel, command);
                        //Session["OHD_NAME"] = null;
                        OverheadParametersModel DeleteModel = new OverheadParametersModel();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        Typ = DeleteModel.TransType;
                        cmd = DeleteModel.Command;
                        bt = DeleteModel.BtnName;
                        return RedirectToAction("OverheadParameters",new { Typ, cmd, bt });

                    case "Save":
                        //Session["Command"] = command;
                        OhdParamModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            SaveOhdParamDetail(OhdParamModel);
                            //Session["OHD_ID"] = OhdParamModel.ohd_exp_id;
                            //Session["OHD_ID"] = Session["OHD_ID"].ToString();
                            if (OhdParamModel.Message == "Update" || OhdParamModel.Message == "Save")
                            {
                                OhdParamModel.Message = "Save";
                                //Session["Message"] = "Save";
                                //Session["Command"] = "Update";
                                //Session["TransType"] = "Save";
                                //Session["OHD_NAME"] = null;
                                //Session["BtnName"] = "BtnSave";
                                OverheadParametersModel savedata = new OverheadParametersModel();
                                savedata.Message = "Save";
                                savedata.TransType = "Save";
                                savedata.BtnName = "BtnSave";
                                savedata.Command = "Update";
                                TempData["ModelData"] = savedata;
                                Typ = savedata.TransType;
                                cmd = savedata.Command;
                                bt = savedata.BtnName;
                                return RedirectToAction("OverheadParameters", new { Typ, cmd, bt });
                            }
                            if (OhdParamModel.Message == "Duplicate")
                            {
                                OhdParamModel.Message = "Duplicate";
                                OhdParamModel.OHD_ID = OhdParamModel.OHD_ID;
                                //OhdParamModel.ohd_exp_name = OhdParamModel.ohd_exp_name;
                                //OhdParamModel.ohd_exp_remarks = OhdParamModel.ohd_exp_remarks;
                                //OhdParamModel.uom_id = OhdParamModel.uom_id;
                                OhdParamModel.TransType = "Update";
                                OhdParamModel.Command = "Edit";
                                OhdParamModel.BtnName = "BtnAddNew";
                                TempData["ModelData"] = OhdParamModel;
                                Typ = OhdParamModel.TransType;
                                cmd = OhdParamModel.Command;
                                bt = OhdParamModel.BtnName;
                                ohd_id = OhdParamModel.OHD_ID.ToString();
                               //var ex_name = OhdParamModel.ohd_exp_name;
                               // var remarks = OhdParamModel.ohd_exp_remarks;
                               //var uom = OhdParamModel.uom_id;
                                //Session["Message"] = "Duplicate";
                                //Session["OHD_ID"] = OHDID;
                                //Session["OHD_NAME"] = ohd_exp_name;
                                //Session["BtnName"] = "BtnAddNew";
                                return RedirectToAction("OverheadParameters", new { Typ, cmd, bt, ohd_id/*, ex_name, remarks, uom*/ });
                            }

                            return RedirectToAction("OverheadParameters");
                        }
                        else
                        {
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/OverheadParameters/OverheadParameters.cshtml", OhdParamModel);
                        }


                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["OHD_NAME"] = null;
                        //Session["DocumentStatus"] = null;
                        OverheadParametersModel RefreshModel = new OverheadParametersModel();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Refresh";
                        TempData["ModelData"] = RefreshModel;
                        return RedirectToAction("OverheadParameters");
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
        private DataTable GetUOMList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _OverheadParameters_ISERVICES.GetUOMDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        
        //get List of detail on table in view page(cshtml)
        private DataTable GetOverheadExpParamDetails()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                
                DataTable dt = _OverheadParameters_ISERVICES.GetOverheadExpParamDetailsDAL(Convert.ToInt32(Comp_ID));
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        
        // method of insert and update data 
        public ActionResult SaveOhdParamDetail(OverheadParametersModel OhdParamModel)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                   int ohd_exp_id = OhdParamModel.ohd_exp_id;
                   string ohd_exp_name = OhdParamModel.ohd_exp_name;
                   int uom_id = OhdParamModel.uom_id;
                   string ohd_exp_remarks = OhdParamModel.ohd_exp_remarks;
                   string create_id = Session["UserId"].ToString();
                //var transtype= Session["TransType"].ToString();
                var transtype = "";
                //if (OhdParamModel.ohd_exp_id != 0)
                //{
                //     transtype = "Update";
                //}
                //else
                //{
                    transtype = "Save";
                //}
                string SaveMessage = _OverheadParameters_ISERVICES.InsertUpdateOverheadParam(Convert.ToInt32(CompID), ohd_exp_id, ohd_exp_name, uom_id, ohd_exp_remarks, Convert.ToInt32(create_id), transtype);
                  string OHDID = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                  string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                
                if (Message == "Update" || Message == "Save")
                {
                    OhdParamModel.Message = "Save";
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Save";
                    //Session["OHD_NAME"] = null;
                    //Session["BtnName"] = "BtnSave";             
                }
                if (Message == "Duplicate")
                {
                    OhdParamModel.Message = "Duplicate";
                    OhdParamModel.OHD_ID = OHDID;
                    //Session["Message"] = "Duplicate";
                    //Session["OHD_ID"] = OHDID;
                    //Session["OHD_NAME"] = ohd_exp_name;
                    //Session["BtnName"] = "BtnAddNew";
                }
               
                return RedirectToAction("OverheadParameters");
            }

            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml"); // commented by nitesh 28-10-2023 for Exception
            }
        }
        public ActionResult EditOhdParam(string ohd_exp_id1)
        {
            try
            { /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BranchId = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                {
                    TempData["Message1"] = "Financial Year not Exist";
                }
                /*End to chk Financial year exist or not*/
                var Typ = "";
                var cmd = "";
                var bt = "";
                var ohd_id = "";
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["OHD_ID"] = ohd_exp_id1;
                //Session["TransType"] = "Update";
                //Session["BtnName"] = "BtnEdit";
                //Session["OHD_NAME"] = null;
                OverheadParametersModel dblclick = new OverheadParametersModel();
                dblclick.Command = "View";             
                dblclick.BtnName = "BtnEdit";
                dblclick.OHD_ID = ohd_exp_id1;
                dblclick.TransType = "Update";
                TempData["ModelData"] = dblclick;
                 Typ = dblclick.TransType;
                cmd = dblclick.Command;
                 bt = dblclick.BtnName;
                 ohd_id = dblclick.OHD_ID;
                return RedirectToAction("OverheadParameters",new { Typ, cmd, bt, ohd_id });
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
           
        }
        private ActionResult OHDDelete(OverheadParametersModel OhdParamModelDEL, string command)
        {
            try
            {
               if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
           
                string Message = _OverheadParameters_ISERVICES.OHDDelete(OhdParamModelDEL, CompID);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["OHD_ID"] = "";
                //OhdParamModelDEL = null;
                //Session["TransType"] = "Refresh";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("OverheadParameters");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
    }

}