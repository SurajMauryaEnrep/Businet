using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.CostCenterSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CostCenterSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.CostCenterSetup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.CostCenterSetup
{
    public class CostCenterSetupController : Controller
    {
        string CompID, branchId, userId, create_id, language = String.Empty;
        string DocumentMenuId = "103150", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CostCenterSetup_ISERVICES CostCenterSetup_ISERVICES;
        public CostCenterSetupController(Common_IServices _Common_IServices, CostCenterSetup_SERVICES CostCenterSetup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.CostCenterSetup_ISERVICES = CostCenterSetup_ISERVICES;
        }
        public ActionResult CostCenterSetup(string cc_id)
        {
            try
            {
                CommonPageDetails();
                var CCModel = TempData["Modeldata"] as CostCenterSetup_Model;
                if (CCModel != null)
                {
                    //CostCenterSetup_Model CCModel = new CostCenterSetup_Model();

                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    //DataTable dt = new DataTable();
                    //List<CostCenter> costCenterlist = new List<CostCenter>();
                    var other = new CommonController(_Common_IServices);
                    //dt = GetCC_nameDropdown();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    CostCenter CClist = new CostCenter();
                    //    CClist.cc_id = dr["cc_id"].ToString();
                    //    CClist.cc_name = dr["cc_name"].ToString();
                    //    costCenterlist.Add(CClist);
                    //}
                    //costCenterlist.Insert(0, new CostCenter() { cc_id = "0", cc_name = "---Select---" });
                    //CCModel.costCenter = costCenterlist;

                    //DataSet ds1 = CostCenterSetup_ISERVICES.GetModulDetails(CompID);
                    //List<ModuleNoList> _moduleno = new List<ModuleNoList>();
                    //_moduleno.Add(new ModuleNoList { module_name = "---Select---", module_id = "0" });
                    //foreach (DataRow dr in ds1.Tables[0].Rows)
                    //{
                    //    ModuleNoList numbarList = new ModuleNoList();
                    //    numbarList.module_id = dr["doc_id"].ToString();
                    //    numbarList.module_name = dr["doc_name_eng"].ToString();
                    //    _moduleno.Add(numbarList);
                    //}
                    //CCModel._moduleno = _moduleno;
                    GetAllDropDownList(CCModel);
                    #region Commented By Nitesh 08-04-2024 For All Data in One Procedure
                    //DataSet DsBranch = getAppDocDetails();
                    //List<Branch_list> branch_Lists = new List<Branch_list>();
                    //foreach (DataRow dr in DsBranch.Tables[0].Rows)
                    //{
                    //    Branch_list _brlist = new Branch_list();
                    //    _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                    //    _brlist.BranchName = dr["comp_nm"].ToString();
                    //    branch_Lists.Add(_brlist);
                    //}
                    //CCModel._branchList = branch_Lists;
                    #endregion
                    //if (Session["Message"] != null)
                    if (CCModel.Message != null)
                    {
                        //ViewBag.Message = Session["Message"].ToString();
                        CCModel.Message = CCModel.Message.ToString();
                        //Session["Message"] = "New";
                        if (CCModel.Message == null)
                        {
                            CCModel.Message = "New";
                        }

                        //if (Session["TransTypeCCSetup"] != null)
                        if (CCModel.TransTypeCCSetup != null)
                        {
                            //if (Session["TransTypeCCSetup"].ToString() == "Duplicate")
                            if (CCModel.TransTypeCCSetup.ToString() == "Duplicate")
                            {
                                //Session["TransTypeCCSetup"] = null;
                                //CCModel.TransTypeCCSetup = null;
                                //if (Session["cc_id"] != null && Session["cc_name"] != null )
                                if (CCModel.cc_id != null && CCModel.cc_name != null)
                                {
                                    //if (ViewBag.Message == "DuplicateCCsetup")
                                    if (CCModel.Message == "DuplicateCCsetup")
                                    {
                                        CCModel.cc_id = CCModel.cc_id.ToString();
                                        CCModel.cc_name = CCModel.cc_name.ToString();
                                        CCModel.ShowCC = "show";
                                    }
                                }
                                if (CCModel.cc_id != null && CCModel.cc_val_name != null && CCModel.cc_val_name != null)
                                //if (Session["cc_id"] != null && Session["cc_val_name"] != null && Session["cc_val_id"] != null)

                                {

                                    //if (ViewBag.Message == "DuplicateValue")
                                    if (CCModel.Message == "DuplicateValue")
                                    {
                                        //CCModel.DDLcc_id = Session["cc_id"].ToString();
                                        CCModel.DDLcc_id = CCModel.cc_id.ToString();
                                        //CCModel.cc_val_name = Session["cc_val_name"].ToString();
                                        CCModel.cc_val_name = CCModel.cc_val_name.ToString();
                                        //CCModel.cc_val_id = Session["cc_val_id"].ToString();
                                        CCModel.cc_val_id = CCModel.cc_val_id.ToString();
                                    }
                                }
                                //CCModel.BtnName = Session["BtnNameCCSetup"].ToString();
                                //CCModel.BtnName = CCModel.BtnName.ToString();
                                //Session["cc_id"] = null;
                                //Session["cc_name"] = null;
                                //Session["cc_val_name"] = null;
                                //Session["cc_val_id"] = null;
                                //Session["BtnNameCCSetup"] = null;
                            }
                        }
                    }
                    else
                    {
                        CCModel.ShowCC = "show";
                    }
                    if (CCModel.BtnName == "UpdateCC_name")
                    {
                        if (CCModel.cc_id != null)
                        {
                            if (CCModel.TransTypeCCSetup == null)
                            {
                                CCModel.BtnName = "SaveCC_name";
                            }
                            else
                            {
                                CCModel.BtnName = "UpdateCC_name";
                            }
                            CCModel.TransTypeCCSetup = null;
                        }
                        else
                        {
                            CCModel.BtnName = "SaveCC_name";
                        }                      
                    }
                    if (CCModel.BtnName == "UpdateCC_val_name")
                    {
                        if (CCModel.cc_val_id != null)
                        {
                            if (CCModel.TransTypeCCSetup == null)
                            {
                                CCModel.BtnName = "SaveCCVal_name";
                            }
                            else
                            {
                                CCModel.BtnName = "UpdateCC_val_name";
                            }
                            CCModel.TransTypeCCSetup = null;
                        }
                        else
                        {
                            CCModel.BtnName = "SaveCCVal_name";
                        }
                    }
                    if (CCModel.Message== "SaveCC_name")
                    {
                        CCModel.cc_id = null;
                        CCModel.cc_name = null;
                    }
                    if (CCModel.Message == "SaveCCVal_name")
                    {
                        CCModel.DDLcc_id = null;
                        CCModel.cc_val_id = null;
                        CCModel.cc_val_name = null;
                    }
                    if (CCModel.Message == "SaveCC_branch_name")
                    {
                        CCModel.BranchCC_id = null;
                    }
                    if (CCModel.Message == "SaveCC_Module")
                    {
                        CCModel.Modulecc_id = null;
                        CCModel.module_id = null;
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    //DataSet ds = CostCenterSetup_ISERVICES.GetCCSetupTable(CompID);
                    ////ViewBag.CostCenterno = ds.Tables[0];
                    //CCModel.CostCenterno = ds.Tables[0];
                    ////ViewBag.CostCenterValue = ds.Tables[1];
                    //CCModel.CostCenterValue = ds.Tables[1];
                    ////ViewBag.CostCenterBranch = ds.Tables[2];
                    //CCModel.CostCenterBranch = ds.Tables[2];
                    ////ViewBag.CostCenterModule = ds.Tables[3];
                    //CCModel.CostCenterModule = ds.Tables[3];
                    CCModel.Title = title;
                    return View("~/Areas/BusinessLayer/Views/CostCenterSetup/CostCenterSetup.cshtml", CCModel);

                }
                else
                {
                    CostCenterSetup_Model CCModel1 = new CostCenterSetup_Model();

                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    //DataTable dt = new DataTable();
                    //List<CostCenter> costCenterlist = new List<CostCenter>();
                    var other = new CommonController(_Common_IServices);
                    //dt = GetCC_nameDropdown();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    CostCenter CClist = new CostCenter();
                    //    CClist.cc_id = dr["cc_id"].ToString();
                    //    CClist.cc_name = dr["cc_name"].ToString();
                    //    costCenterlist.Add(CClist);
                    //}
                    //costCenterlist.Insert(0, new CostCenter() { cc_id = "0", cc_name = "---Select---" });
                    //CCModel1.costCenter = costCenterlist;

                    //DataSet ds1 = CostCenterSetup_ISERVICES.GetModulDetails(CompID);
                    //List<ModuleNoList> _moduleno = new List<ModuleNoList>();
                    //_moduleno.Add(new ModuleNoList { module_name = "---Select---", module_id = "0" });
                    //foreach (DataRow dr in ds1.Tables[0].Rows)
                    //{
                    //    ModuleNoList numbarList = new ModuleNoList();
                    //    numbarList.module_id = dr["doc_id"].ToString();
                    //    numbarList.module_name = dr["doc_name_eng"].ToString();
                    //    _moduleno.Add(numbarList);
                    //}
                    //CCModel1._moduleno = _moduleno;
                    GetAllDropDownList(CCModel1);

                    #region Commented By Nitesh 08-04-2024 For All Data in One Procedure
                    //DataSet DsBranch = getAppDocDetails();
                    //List<Branch_list> branch_Lists = new List<Branch_list>();
                    //foreach (DataRow dr in DsBranch.Tables[0].Rows)
                    //{
                    //    Branch_list _brlist = new Branch_list();
                    //    _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                    //    _brlist.BranchName = dr["comp_nm"].ToString();
                    //    branch_Lists.Add(_brlist);
                    //}
                    //CCModel1._branchList = branch_Lists;
                    #endregion
                    //if (Session["Message"] != null)
                    if (CCModel1.Message != null)
                    {
                        //ViewBag.Message = Session["Message"].ToString();
                        CCModel1.Message = CCModel1.Message.ToString();
                        //Session["Message"] = "New";
                        if (CCModel1.Message == null)
                        {
                            CCModel1.Message = "New";
                        }

                        //if (Session["TransTypeCCSetup"] != null)
                        //if (CCModel1.TransTypeCCSetup != null)
                        //{
                        //    //if (Session["TransTypeCCSetup"].ToString() == "Duplicate")
                        //    if (CCModel1.TransTypeCCSetup.ToString() == "Duplicate")
                        //    {
                        //        //Session["TransTypeCCSetup"] = null;
                        //        CCModel1.TransTypeCCSetup = null;
                        //        //if (Session["cc_id"] != null && Session["cc_name"] != null )
                        //        if (CCModel.cc_id != null && CCModel1.cc_name != null)
                        //        {
                        //            //if (ViewBag.Message == "DuplicateCCsetup")
                        //            if (CCModel1.Message == "DuplicateCCsetup")
                        //            {
                        //                CCModel1.cc_id = CCModel1.cc_id.ToString();
                        //                CCModel1.cc_name = CCModel1.cc_name.ToString();
                        //                CCModel1.ShowCC = "show";
                        //            }
                        //        }
                        //        if (CCModel1.cc_id != null && CCModel1.cc_val_name != null && CCModel1.cc_val_name != null)
                        //        //if (Session["cc_id"] != null && Session["cc_val_name"] != null && Session["cc_val_id"] != null)

                        //        {

                        //            //if (ViewBag.Message == "DuplicateValue")
                        //            if (CCModel1.Message == "DuplicateValue")
                        //            {
                        //                //CCModel.DDLcc_id = Session["cc_id"].ToString();
                        //                CCModel1.DDLcc_id = CCModel1.cc_id.ToString();
                        //                //CCModel.cc_val_name = Session["cc_val_name"].ToString();
                        //                CCModel1.cc_val_name = CCModel1.cc_val_name.ToString();
                        //                //CCModel.cc_val_id = Session["cc_val_id"].ToString();
                        //                CCModel1.cc_val_id = CCModel1.cc_val_id.ToString();
                        //            }
                        //        }
                        //        //CCModel.BtnName = Session["BtnNameCCSetup"].ToString();
                        //        //CCModel.BtnName = CCModel.BtnName.ToString();
                        //        //Session["cc_id"] = null;
                        //        //Session["cc_name"] = null;
                        //        //Session["cc_val_name"] = null;
                        //        //Session["cc_val_id"] = null;
                        //        //Session["BtnNameCCSetup"] = null;
                        //    }
                        //}
                    }
                    else
                    {
                        CCModel1.ShowCC = "show";
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                   // DataSet ds = CostCenterSetup_ISERVICES.GetCCSetupTable(CompID);
                    
                    CCModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/CostCenterSetup/CostCenterSetup.cshtml", CCModel1);
                }
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
                    branchId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchId, userId, DocumentMenuId, language);
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
        private void GetAllDropDownList(CostCenterSetup_Model CCModel1)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["userid"] != null)
            {
                create_id = Session["userid"].ToString();
            }
            string flag = "ddlbranch";
            List<CostCenter> costCenterlist = new List<CostCenter>();
            DataSet dt = CostCenterSetup_ISERVICES.GetAllDropDown(flag,CompID, create_id);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                CostCenter CClist = new CostCenter();
                CClist.cc_id = dr["cc_id"].ToString();
                CClist.cc_name = dr["cc_name"].ToString();
                costCenterlist.Add(CClist);
            }
            costCenterlist.Insert(0, new CostCenter() { cc_id = "0", cc_name = "---Select---" });
            CCModel1.costCenter = costCenterlist;

          //  DataSet ds1 = CostCenterSetup_ISERVICES.GetModulDetails(CompID);
            List<ModuleNoList> _moduleno = new List<ModuleNoList>();
            _moduleno.Add(new ModuleNoList { module_name = "---Select---", module_id = "0" });
            foreach (DataRow dr in dt.Tables[1].Rows)
            {
                ModuleNoList numbarList = new ModuleNoList();
                numbarList.module_id = dr["doc_id"].ToString();
                numbarList.module_name = dr["doc_name_eng"].ToString();
                _moduleno.Add(numbarList);
            }
            CCModel1._moduleno = _moduleno;

            List<Branch_list> branch_Lists = new List<Branch_list>();
            foreach (DataRow dr in dt.Tables[6].Rows)
            {
                Branch_list _brlist = new Branch_list();
                _brlist.BranchID = Convert.ToInt32(dr["Comp_id"].ToString());
                _brlist.BranchName = dr["comp_nm"].ToString();
                branch_Lists.Add(_brlist);
            }
            CCModel1._branchList = branch_Lists;

            CCModel1.CostCenterno = dt.Tables[0];
            //ViewBag.CostCenterValue = ds.Tables[1];
            CCModel1.CostCenterValue = dt.Tables[3];
            //ViewBag.CostCenterBranch = ds.Tables[2];
            CCModel1.CostCenterBranch = dt.Tables[4];
            //ViewBag.CostCenterModule = ds.Tables[3];
            CCModel1.CostCenterModule = dt.Tables[5];
        }
        public ActionResult SaveCCSetup(CostCenterSetup_Model CCModel, string Command)
        {
            try
            {
                switch (Command)
                {
                    case "SaveCC_name":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        var cc_id = CCModel.cc_id;
                        return( RedirectToAction("CostCenterSetup", new { cc_id = cc_id}));
                    case "UpdateCC_name":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        cc_id = CCModel.cc_id;
                        return ( RedirectToAction("CostCenterSetup", new { cc_id = cc_id }));
                    case "SaveCCVal_name":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupValueDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        cc_id = CCModel.cc_val_id;
                        return ( RedirectToAction("CostCenterSetup", new { cc_id = cc_id }));
                    case "UpdateCC_val_name":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupValueDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        cc_id = CCModel.cc_val_id;
                        return ( RedirectToAction("CostCenterSetup", new { cc_id = cc_id }));
                    case "SaveCC_branch_name":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupBranchDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        cc_id = CCModel.BranchCC_id;
                        return ( RedirectToAction("CostCenterSetup", new { cc_id = cc_id }));
                    case "SaveCC_Module":
                        //Session["BtnNameCCSetup"] = Command;
                        CCModel.BtnName = Command;
                        SaveCCSetupModuleDetails(CCModel, Command);
                        TempData["Modeldata"] = CCModel;
                        cc_id = CCModel.Modulecc_id;
                        return ( RedirectToAction("CostCenterSetup", new { cc_id = cc_id }));
                }
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveCCSetupDetails(CostCenterSetup_Model CCModel, string Command)
        {
            try
            {
                string user_id = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    user_id = Session["userid"].ToString();
                }
                string SaveMessage = CostCenterSetup_ISERVICES.SaveCCSetupData(Command,CompID, user_id, CCModel.cc_id, CCModel.cc_name);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
            
                if (Message == "UpdateCC_name" || Message == "SaveCC_name")
                {
                    CCModel.hdnSavebtn = "";
                    //Session["Message"] = "SaveCC_name";
                    CCModel.Message = "SaveCC_name";
                    //Session["TransTypeCCSetup"] = null;
                    CCModel.TransTypeCCSetup = null;
                }
                if (Message == "Duplicate")
                {
                    //Session["TransTypeCCSetup"] = "Duplicate";
                    CCModel.hdnSavebtn = "";
                    CCModel.TransTypeCCSetup = "Duplicate";
                    //Session["Message"] = "DuplicateCCsetup";
                    CCModel.Message = "DuplicateCCsetup";
                    CCModel.cc_id = IsNull(CCModel.cc_id, "");
                    //Session["cc_id"] = IsNull(CCModel.cc_id, "");
                    CCModel.cc_name = CCModel.cc_name;
                    //Session["cc_name"] = CCModel.cc_name;
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }           
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private ActionResult SaveCCSetupValueDetails(CostCenterSetup_Model CCModel, string Command)
        {
            try
            {
                string user_id = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    user_id = Session["userid"].ToString();
                }              
                string SaveMessage = CostCenterSetup_ISERVICES.SaveCCSetupValueData(Command, CompID, user_id, CCModel.DDLcc_id,CCModel.cc_val_id, CCModel.cc_val_name);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "UpdateCC_val_name" || Message == "SaveCCVal_name")
                    {
                        //Session["Message"] = "SaveCCVal_name";
                    CCModel.Message = "SaveCCVal_name";
                    CCModel.TransTypeCCSetup = null;
                    //Session["TransTypeCCSetup"] = null;

                }
                if (Message == "Duplicate")
                {
                    //Session["TransTypeCCSetup"] = "Duplicate";
                    CCModel.TransTypeCCSetup = "Duplicate";
                    //Session["Message"] = "DuplicateValue";
                    CCModel.Message = "DuplicateValue";
                    //Session["cc_id"] = CCModel.DDLcc_id;
                    CCModel.cc_id = CCModel.DDLcc_id;
                    //Session["cc_val_id"] = IsNull(CCModel.cc_val_id, "");
                    CCModel.cc_val_id = IsNull(CCModel.cc_val_id, "");
                    //Session["cc_val_name"] = CCModel.cc_val_name;
                    CCModel.cc_val_name = CCModel.cc_val_name;
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private ActionResult SaveCCSetupBranchDetails(CostCenterSetup_Model CCModel, string Command)
        {
            try
            {
                string user_id = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    user_id = Session["userid"].ToString();
                }
                string SaveMessage = CostCenterSetup_ISERVICES.SaveCCSetup_branch_ueData(Command, CompID, user_id, CCModel.BranchCC_id, CCModel.Branch_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if( Message == "SaveCC_branch_name")
                {
                    //Session["Message"] = "SaveCC_branch_name";
                    CCModel.Message = "SaveCC_branch_name";
                    //Session["TransTypeCCSetup"] = null;
                    CCModel.TransTypeCCSetup = null;
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private ActionResult SaveCCSetupModuleDetails(CostCenterSetup_Model CCModel, string Command)
        {
            try
            {
                string user_id = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    user_id = Session["userid"].ToString();
                }
                string SaveMessage = CostCenterSetup_ISERVICES.SaveCCSetup_Module(Command, CompID, user_id, CCModel.Modulecc_id, CCModel.module_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "SaveCC_Module")
                {
                    //Session["Message"] = "SaveCC_Module";
                    CCModel.Message = "SaveCC_Module";
                    //Session["TransTypeCCSetup"] = null;
                    CCModel.TransTypeCCSetup = null;
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult deleteCCSetup( string cc_id)
        {
            try
            {
                CostCenterSetup_Model CCModel = new CostCenterSetup_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (cc_id != "")
                {
                    string SaveMessage = CostCenterSetup_ISERVICES.DeleteCCSetup("Delete",cc_id , CompID);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        //Session["Message"] = "Deleted_CCname";
                        CCModel.Message = "Deleted_CCname";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "Used";
                        CCModel.Message = "Used";
                    }
                    if (Message == "Used_br")
                    {
                        //Session["Message"] = "Used_br";
                        CCModel.Message = "Used_br";
                    }
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult deleteCCSetup_val(string cc_val_id)
        {
            try
            {
                CostCenterSetup_Model CCModel = new CostCenterSetup_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (cc_val_id != "")
                {
                    string SaveMessage = CostCenterSetup_ISERVICES.DeleteCC_Setup_val("Delete", cc_val_id, CompID);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        //Session["Message"] = "Deleted_valuse";
                        CCModel.Message = "Deleted_valuse";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "Used";
                        CCModel.Message = "UsedVal";
                    }
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult deleteCC_branc(string cc_id,string br_id)
        {
            try
            {
                CostCenterSetup_Model CCModel = new CostCenterSetup_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (cc_id != "" && br_id != "")
                {
                    string SaveMessage = CostCenterSetup_ISERVICES.deleteCC_branchSetup_val("Delete", cc_id, br_id, CompID);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        //Session["Message"] = "Deleted_Branch_ccsetup";
                        CCModel.Message = "Deleted_Branch_ccsetup";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "Used";
                        CCModel.Message = "Used";
                    }
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult deleteCC_Module(string cc_id, string module_id)
        {
            try
            {
                CostCenterSetup_Model CCModel = new CostCenterSetup_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (cc_id != "" && module_id != "")
                {
                    string SaveMessage = CostCenterSetup_ISERVICES.deleteCC_module("Delete", cc_id, module_id, CompID);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        //Session["Message"] = "delete_module";
                        CCModel.Message = "delete_module";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "Used";
                        CCModel.Message = "Used";
                    }
                }
                TempData["Modeldata"] = CCModel;
                return RedirectToAction("CostCenterSetup");
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataSet getAppDocDetails()
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    create_id = Session["userid"].ToString();
                }
                string flag = "ddlbranch";
                DataSet DocumentName = CostCenterSetup_ISERVICES.getAppDocDetails(flag, CompID, create_id);
                return DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetCC_nameDropdown()
        {
            try
            {
                string CompID = string.Empty;
                
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                
                DataTable dt = CostCenterSetup_ISERVICES.GetCostCenterList(CompID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
        }
        public JsonResult GetBranchOnchangeCC(string ddlcc_id)
        {
            try
            {
                JsonResult result = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    create_id = Session["userid"].ToString();
                }
                DataSet Ds = CostCenterSetup_ISERVICES.GetBranchOnchangeCC(ddlcc_id, CompID, create_id);
                result = Json(JsonConvert.SerializeObject(Ds));
                return result;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public JsonResult GetModuleOnchangeCC(string DDLModulecc_id)
        {
            try
            {
                JsonResult result = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    create_id = Session["userid"].ToString();
                }
                DataSet Ds = CostCenterSetup_ISERVICES.GetModuleOnchangeCC(DDLModulecc_id, CompID, create_id);
                result = Json(JsonConvert.SerializeObject(Ds));
                return result;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
    }

}






















