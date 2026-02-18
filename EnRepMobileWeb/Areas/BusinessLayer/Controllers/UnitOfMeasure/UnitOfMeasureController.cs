using EnRepMobileWeb.MODELS.BusinessLayer.UnitOfMeasure;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.UnitOfMeasure;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.UnitOfMeasure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.UnitOfMeasure
{
    public class UnitOfMeasureController : Controller
    {
        string CompID, language, br_id, user_id, title = String.Empty;
        string DocumentMenuId = "103110";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        UnitOfMeasure_ISERVICE UnitOfMeasure_ISERVICE;
        //UnitOfMeasure_MODEL UOMModel;
        public UnitOfMeasureController(Common_IServices _Common_IServices, UnitOfMeasure_SERVICE unitOfMeasure_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.UnitOfMeasure_ISERVICE = unitOfMeasure_ISERVICE;
        }
        // GET: BusinessLayer/UnitOfMeasure
        public ActionResult UnitOfMeasure()
        {
            try
            {
                CommonPageDetails();
                var UOMModel = TempData["Modeldata"] as UnitOfMeasure_MODEL;
                if (UOMModel != null)
                {
                    if (UOMModel.Message != null)
                    {
                        if (UOMModel.TransTypeUOM != null)
                        {
                            if (UOMModel.TransTypeUOM.ToString() == "Duplicate")
                            {
                                UOMModel.TransTypeUOM = null;
                                //if (UOMModel.uom_id != null && UOMModel.uom_name != null && UOMModel.uom_alias != null)
                                //{
                                //    UOMModel.uom_id = UOMModel.uom_id.ToString();
                                //    UOMModel.uom_name = UOMModel.uom_name.ToString();
                                //    UOMModel.uom_alias = UOMModel.uom_alias.ToString();
                                //}
                                UOMModel.BtnName = UOMModel.BtnNameUOM;
                                UOMModel.BtnNameUOM = null;
                            }
                        }
                        if (UOMModel.Message == "Save")
                        {
                            UOMModel.uom_id = null;
                            UOMModel.uom_name = null;
                            UOMModel.uom_alias = null;                           
                        }
                        if (UOMModel.Message == "SaveConversion")
                        {
                            UOMModel.ItemName = null;
                            UOMModel.Uom = null;
                            UOMModel.UomID = null;
                            UOMModel.ConvertedUnit = null;
                            UOMModel.ConversionRate = null;
                        }
                    }
                    if (UOMModel.Message == null)
                    {
                        UOMModel.Message = "New";
                    }
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }
                    //DataTable dt = new DataTable();
                    //List<ItemNameList> ProductNameLists = new List<ItemNameList>();
                    //dt = GetItemNameLists();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ItemNameList ItemList = new ItemNameList();
                    //    ItemList.ItemId = dr["Item_id"].ToString();
                    //    ItemList.ItemName = dr["Item_name"].ToString();
                    //    ProductNameLists.Add(ItemList);
                    //}
                    //ProductNameLists.Insert(0, new ItemNameList() { ItemId = "0", ItemName = "---Select---" });
                    //UOMModel._ItemNameList = ProductNameLists;

                    //DataTable dt1 = new DataTable();
                    //List<ConvertedUnitList> ConvertedUnitLists = new List<ConvertedUnitList>();
                    ////dt1 = GetUomNameLists();
                    ////foreach (DataRow dr in dt1.Rows)
                    ////{
                    ////    ConvertedUnitList UomList = new ConvertedUnitList();
                    ////    UomList.UnitId = dr["uom_id"].ToString();
                    ////    UomList.UnitName = dr["uom_name"].ToString();
                    ////    ConvertedUnitLists.Add(UomList);
                    ////}
                    //ConvertedUnitLists.Insert(0, new ConvertedUnitList() { UnitId = "0", UnitName = "---Select---" });
                    //UOMModel._ConvertedUnitList = ConvertedUnitLists;
                    //DataSet ds = UnitOfMeasure_ISERVICE.GetUOMTable(CompID, br_id);
                    //ViewBag.UomNo = ds.Tables[0];
                    //ViewBag.ConvrsionDetails= ds.Tables[1];
                    GetAllDropDown(UOMModel);
                    //ViewBag.MenuPageName = getDocumentName();
                    UOMModel.Title = title;
                    return View("~/Areas/BusinessLayer/Views/UnitOfMeasure/UnitOfMeasure.cshtml", UOMModel);
                }
                else
                {
                    UnitOfMeasure_MODEL UOMModel1 = new UnitOfMeasure_MODEL();
                    if (UOMModel1.Message != null)
                    {
                        if (UOMModel1.TransTypeUOM != null)
                        {
                            if (UOMModel1.TransTypeUOM.ToString() == "Duplicate")
                            {
                                UOMModel1.TransTypeUOM = null;
                                //if (UOMModel1.uom_id != null && UOMModel1.uom_name != null && UOMModel1.uom_alias != null)
                                //{
                                //    UOMModel1.uom_id = UOMModel.uom_id.ToString();
                                //    UOMModel1.uom_name = UOMModel.uom_name.ToString();
                                //    UOMModel1.uom_alias = UOMModel.uom_alias.ToString();
                                //}
                                UOMModel1.BtnName = UOMModel.BtnNameUOM;
                                UOMModel1.BtnNameUOM = null;
                            }
                        }
                        if (UOMModel1.Message.ToString() == "Save")
                        {
                            UOMModel1.uom_id = null;
                            UOMModel1.uom_name = null;
                            UOMModel1.uom_alias = null;
                        }
                        if (UOMModel1.Message == "SaveConversion")
                        {
                            UOMModel1.ItemName = null;
                            UOMModel1.Uom = null;
                            UOMModel1.UomID = null;
                            UOMModel1.ConvertedUnit = null;
                            UOMModel1.ConversionRate = null;
                        }
                    }
                    if (UOMModel1.Message == null)
                    {
                        UOMModel1.Message = "New";
                    }
                        if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }
                    GetAllDropDown(UOMModel1);
                    //ViewBag.MenuPageName = getDocumentName();
                    UOMModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/UnitOfMeasure/UnitOfMeasure.cshtml", UOMModel1);
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
                    br_id = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    user_id = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, br_id, user_id, DocumentMenuId, language);
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
        private void GetAllDropDown(UnitOfMeasure_MODEL UOMModel1)  /**Added By NItesh 19-03-2024**/
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                br_id = Session["BranchId"].ToString();
            }
            DataSet dt = UnitOfMeasure_ISERVICE.GetAllDataDropDown(CompID, br_id);
          //  DataTable dt = new DataTable();
            List<ItemNameList> ProductNameLists = new List<ItemNameList>();
           // dt = GetItemNameLists();
            //foreach (DataRow dr in dt.Tables[0].Rows)
            //{
            //    ItemNameList ItemList = new ItemNameList();
            //    ItemList.ItemId = dr["Item_id"].ToString();
            //    ItemList.ItemName = dr["Item_name"].ToString();
            //    ProductNameLists.Add(ItemList);
            //}
            ProductNameLists.Insert(0, new ItemNameList() { ItemId = "0", ItemName = "---Select---" });
            UOMModel1._ItemNameList = ProductNameLists;

            DataTable dt1 = new DataTable();
            List<ConvertedUnitList> ConvertedUnitLists = new List<ConvertedUnitList>();
            //dt1 = GetUomNameLists();
            //foreach (DataRow dr in dt1.Rows)
            //{
            //    ConvertedUnitList UomList = new ConvertedUnitList();
            //    UomList.UnitId = dr["uom_id"].ToString();
            //    UomList.UnitName = dr["uom_name"].ToString();
            //    ConvertedUnitLists.Add(UomList);
            //}
            ConvertedUnitLists.Insert(0, new ConvertedUnitList() { UnitId = "0", UnitName = "---Select---" });
            UOMModel1._ConvertedUnitList = ConvertedUnitLists;
          // DataSet ds1 = UnitOfMeasure_ISERVICE.GetUOMTable(CompID, br_id);
            ViewBag.UomNo = dt.Tables[1];
            ViewBag.ConvrsionDetails = dt.Tables[2];
        }
        public ActionResult SaveUOMSetup(UnitOfMeasure_MODEL UOMModel, string Command)
        {
            try
            {
                switch (Command)
                {
                    case "Save":
                        UOMModel.BtnNameUOM = Command;
                        SaveUOMDetails(UOMModel, Command);
                        TempData["Modeldata"] = UOMModel;
                        //UnitOfMeasure_MODEL Modeldata = (UnitOfMeasure_MODEL)TempData["Modeldata"];
                        return RedirectToAction("UnitOfMeasure");
                    case "Update":
                        UOMModel.BtnNameUOM = Command;
                        SaveUOMDetails(UOMModel, Command);
                        TempData["Modeldata"] = UOMModel;
                        return RedirectToAction("UnitOfMeasure");
                    case "SaveConversion":
                        UOMModel.BtnNameUOM = Command;
                        SaveUOMDetails(UOMModel, Command);
                        TempData["Modeldata"] = UOMModel;
                        return RedirectToAction("UnitOfMeasure");
                    case "UpdateConversion":
                        UOMModel.BtnNameUOM = Command;
                        SaveUOMDetails(UOMModel, Command);
                        TempData["Modeldata"] = UOMModel;
                        return RedirectToAction("UnitOfMeasure");
                    default:
                        return RedirectToAction("UnitOfMeasure");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveUOMDetails(UnitOfMeasure_MODEL UOMModel, string Command )
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
                if(Command== "SaveConversion")
                {
                    UOMModel.uom_id = UOMModel.UomID;
                }
                if (Command == "UpdateConversion")
                {
                    UOMModel.uom_id = UOMModel.UomID;
                }
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                string mac_id = SystemDetail;
                string ShowStock = "N";
                if (UOMModel.ShowStock == true)
                {
                    ShowStock = "Y";
                }
                string SaveMessage = UnitOfMeasure_ISERVICE.SaveUOMData(Command, CompID, user_id, mac_id, UOMModel.uom_id, UOMModel.uom_name, UOMModel.uom_alias,IsNull(UOMModel.ItemName,""), IsNull(UOMModel.ConvertedUnit, ""), IsNull(UOMModel.ConversionRate, ""), ShowStock);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                if (Message == "Update" || Message == "Save")
                {
                    UOMModel.Message= "Save";
                    UOMModel.TransTypeUOM = null;
                }
                if(Message == "SaveConversion" || Message == "UpdateConversion")
                {
                    UOMModel.Message = "SaveConversion";
                    UOMModel.TransTypeUOM = null;
                }
                if (Message == "Duplicate"|| Message == "DuplicateName" || Message == "DuplicateAlias")
                {
                    UOMModel.TransTypeUOM = "Duplicate";
                    UOMModel.Message = Message;
                    UOMModel.uom_id = IsNull(UOMModel.uom_id, "");
                    UOMModel.uom_name = UOMModel.uom_name;
                    UOMModel.uom_alias = UOMModel.uom_alias;
                }    
                return RedirectToAction("UnitOfMeasure", UOMModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [NonAction]
        private DataTable GetItemNameLists()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = UnitOfMeasure_ISERVICE.GetItemNameLists(CompID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetUomNameLists()
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
                DataTable dt = UnitOfMeasure_ISERVICE.GetUomNameLists(CompID, br_id);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetUomNameDict(SearchParams search)
        {
            try
            {
                Dictionary<string, string> UomDictionary = new Dictionary<string, string>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                DataTable dt = UnitOfMeasure_ISERVICE.GetUomNameLists(CompID, br_id, search.SearchValue);
                UomDictionary = GetDict(dt);
                
                return Json(UomDictionary.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        internal Dictionary<string, string> GetDict(DataTable dt)
        {
            var myList = dt.AsEnumerable()
                            .ToDictionary<DataRow, string, string>(row => row[0].ToString(),
                                       row => row[1].ToString());
            return myList;
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
        public ActionResult deleteUOM(UnitOfMeasure_MODEL UOMModel, string uom_id,string TrancType,string Conv_item_id,string conv_uom_id)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (uom_id != "")
                {
                    string SaveMessage = UnitOfMeasure_ISERVICE.DeleteUOM(TrancType, uom_id, CompID, Conv_item_id, conv_uom_id);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        UOMModel.Message= "Deleted";
                    }
                    if (Message == "DeleteConversionTable")
                    {
                        UOMModel.Message = "DeleteConversionTable";
                    }
                }
                TempData["Modeldata"] = UOMModel;
                return RedirectToAction("UnitOfMeasure");
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
        public JsonResult GetItemsList(BindItemList bindItemList)//Added by Suraj maurya on 14-10-2024 to bind item list dynamic
        {

            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                JsonResult DataRows;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(bindItemList.SearchName))
                {
                    bindItemList.SearchName = "";
                }

                DataSet result = _Common_IServices.GetItmListDL(Comp_ID, Br_ID, bindItemList.SearchName, "UOM");
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
    }

}