using EnRepMobileWeb.MODELS.BusinessLayer.ItemAttributeSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemAttributeSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.ItemAttributeSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.ItemAttributeSetup
{
    public class ItemAttributeSetupController : Controller
    {
        string DocumentMenuId = "103117", CompID, br_id, user_id, language, title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ItemAttributeSetup_ISERVICES itemAttributeSetup_ISERVICES;
        ItemAttributeSetup_SERVICES itemAttributeSetup_SERVICES;
        public ItemAttributeSetupController(Common_IServices _Common_IServices, ItemAttributeSetup_ISERVICES itemAttributeSetup_ISERVICES, ItemAttributeSetup_SERVICES itemAttributeSetup_SERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.itemAttributeSetup_ISERVICES = itemAttributeSetup_ISERVICES;
            this.itemAttributeSetup_SERVICES = itemAttributeSetup_SERVICES;
        }
        // GET: BusinessLayer/ItemAttributeSetup 
        public ActionResult ItemAttributeSetup()
        {
            try
            {
                CommonPageDetails();
                var _itemAttributeSetupModel = TempData["Modeldata"] as ItemAttributeSetupModel;
                ItemAttributeSetupModel _itemAttributeSetupM = new ItemAttributeSetupModel();
                if (_itemAttributeSetupModel != null)
                {
                    //ViewBag.MenuPageName = getDocumentName();
                    string msg = _itemAttributeSetupModel.Message;
                    //if (_itemAttributeSetupModel.Message != null)
                    ////if (Session["Message"] != null)
                    //{
                    //    //msg = Session["Message"].ToString();
                    //    msg = _itemAttributeSetupModel.Message.ToString();
                    //}

                    //Session["Message"] = "New";
                    _itemAttributeSetupM.Message = "New";
                    if (msg == "Save")
                    {
                        //ViewBag.Message = "Save";
                        _itemAttributeSetupM.Message = "Save";
                    }
                    if (msg == "Used")
                    {
                        _itemAttributeSetupM.Message = "Exist";
                    }
                    if (msg == "DuplicateAttrName")
                    {
                        _itemAttributeSetupM.Message = "DuplicateAttrName";
                        //ViewBag.Message = "DuplicateAttrName";
                        //_itemAttributeSetupModel.attributeName = Session["attrName"].ToString();
                        //_itemAttributeSetupModel.attributeID = Session["attr_id"].ToString();
                        //Session["attrName"] = null;
                        //Session["attr_id"] = null;
                        _itemAttributeSetupM.attributeName = _itemAttributeSetupModel.attrName;
                        _itemAttributeSetupM.attributeID = _itemAttributeSetupModel.attr_id;
                        //_itemAttributeSetupM.attrName = null;
                        //_itemAttributeSetupM.attr_id = null;
                    }
                    if (msg == "DuplicateAttrValName")
                    {
                        //ViewBag.Message = "DuplicateAttrValName";
                        _itemAttributeSetupM.Message = "DuplicateAttrValName";
                        //_itemAttributeSetupModel.attributeValue = Session["attrValName"].ToString();
                        //_itemAttributeSetupModel.L_attribute_Name = Session["attrName_id"].ToString();
                        //_itemAttributeSetupModel.attrVal_Id = Session["attr_val_id"].ToString();
                        _itemAttributeSetupM.attributeValue = _itemAttributeSetupModel.attrValName;
                        _itemAttributeSetupM.L_attribute_Name = _itemAttributeSetupModel.attrName_id;
                        _itemAttributeSetupM.attrVal_Id = _itemAttributeSetupModel.attr_val_id;
                    }
                    if (msg == "Deleted")
                    {
                        //ViewBag.Message = "Deleted";
                        _itemAttributeSetupM.Message = "Deleted";
                        //_itemAttributeSetupModel.hdnAction = null;
                        //_itemAttributeSetupModel.attributeID = null;
                        //_itemAttributeSetupModel.attrVal_Id = null;
                        //_itemAttributeSetupModel.L_attribute_Name = null;
                    }
                    if (msg == "UsedAttr")
                    {
                        //ViewBag.Message = "Exist";
                        _itemAttributeSetupM.Message = "Exist";
                        //_itemAttributeSetupModel.hdnAction = null;
                        //_itemAttributeSetupModel.attributeID = null;
                        //_itemAttributeSetupModel.attrVal_Id = null;
                        //_itemAttributeSetupModel.L_attribute_Name = null;
                    }
                    string CompID = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    _itemAttributeSetupM.collapse = _itemAttributeSetupModel.collapse;
                    DataSet dt = new DataSet();
                    dt = itemAttributeSetup_ISERVICES.getAttrName(CompID);
                    ViewBag.AttributeName = dt.Tables[0];
                    ViewBag.AttributeValues = dt.Tables[1];
                    List<Attributelist> _attrlists = new List<Attributelist>();
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        Attributelist _Attribute = new Attributelist();
                        _Attribute.attri_id = dr["attr_id"].ToString();
                        _Attribute.attri_name = dr["attr_name"].ToString();
                        _attrlists.Add(_Attribute);
                    }
                    //if (msg == "Save")
                    //{
                    //_itemAttributeSetupModel.L_attribute_Name = null;
                    //_itemAttributeSetupModel.attributeValue = null;
                    //_itemAttributeSetupModel.attributeName = null;
                    //_itemAttributeSetupModel.hdnAction = null;
                    //_itemAttributeSetupModel.attributeID = null;
                    //_itemAttributeSetupModel.hdnAction1attrval = null;
                    //_itemAttributeSetupModel.attrVal_Id = null;
                    //}
                    //if (msg != null)
                    //{
                    //    msg = null;
                    //}
                    _itemAttributeSetupM.attributes = _attrlists;
                    _itemAttributeSetupM.Title = title;
                    _itemAttributeSetupM.actionCommand = _itemAttributeSetupModel.actionCommand;
                    return View("~/Areas/BusinessLayer/Views/ItemAttributeSetup/ItemAttributeSetup.cshtml", _itemAttributeSetupM);
                }
                else
                {
                    ItemAttributeSetupModel _itemAttributeSetupModel1 = new ItemAttributeSetupModel();
                    //ViewBag.MenuPageName = getDocumentName();
                    //string msg = _itemAttributeSetupModel1.Message;
                    //if (_itemAttributeSetupModel1.Message != null)
                    ////if (Session["Message"] != null)
                    //{
                    //    //msg = Session["Message"].ToString();
                    //    msg = _itemAttributeSetupModel1.Message.ToString();
                    //}

                    ////Session["Message"] = "New";
                    //_itemAttributeSetupModel1.Message = "New";
                    //if (msg == "Save")
                    //{
                    //    //ViewBag.Message = "Save";
                    //    _itemAttributeSetupModel1.Message = "Save";
                    //}
                    //if (msg == "DuplicateAttrName")
                    //{
                    //    _itemAttributeSetupModel1.Message = "DuplicateAttrName";
                    //    //ViewBag.Message = "DuplicateAttrName";
                    //    //_itemAttributeSetupModel.attributeName = Session["attrName"].ToString();
                    //    //_itemAttributeSetupModel.attributeID = Session["attr_id"].ToString();
                    //    //Session["attrName"] = null;
                    //    //Session["attr_id"] = null;
                    //    _itemAttributeSetupModel1.attributeName = _itemAttributeSetupModel1.attrName.ToString();
                    //    _itemAttributeSetupModel1.attributeID = _itemAttributeSetupModel1.attr_id.ToString();
                    //    _itemAttributeSetupModel1.attrName = null;
                    //    _itemAttributeSetupModel1.attr_id = null;
                    //}
                    //if (msg == "DuplicateAttrValName")
                    //{
                    //    //ViewBag.Message = "DuplicateAttrValName";
                    //    _itemAttributeSetupModel1.Message = "DuplicateAttrValName";
                    //    //_itemAttributeSetupModel.attributeValue = Session["attrValName"].ToString();
                    //    //_itemAttributeSetupModel.L_attribute_Name = Session["attrName_id"].ToString();
                    //    //_itemAttributeSetupModel.attrVal_Id = Session["attr_val_id"].ToString();
                    //    _itemAttributeSetupModel1.attributeValue = _itemAttributeSetupModel1.attrValName.ToString();
                    //    _itemAttributeSetupModel1.L_attribute_Name = _itemAttributeSetupModel1.attrName_id.ToString();
                    //    _itemAttributeSetupModel1.attrVal_Id = _itemAttributeSetupModel1.attr_val_id.ToString();
                    //}
                    //if (msg == "Deleted")
                    //{
                    //    //ViewBag.Message = "Deleted";
                    //    _itemAttributeSetupModel1.Message = "Deleted";
                    //}
                    //if (msg == "UsedAttr")
                    //{
                    //    //ViewBag.Message = "Exist";
                    //    _itemAttributeSetupModel1.Message = "Exist";
                    //}
                    string CompID = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    DataSet dt = new DataSet();
                    dt = itemAttributeSetup_ISERVICES.getAttrName(CompID);
                    ViewBag.AttributeName = dt.Tables[0];
                    ViewBag.AttributeValues = dt.Tables[1];
                    List<Attributelist> _attrlists = new List<Attributelist>();
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        Attributelist _Attribute = new Attributelist();
                        _Attribute.attri_id = dr["attr_id"].ToString();
                        _Attribute.attri_name = dr["attr_name"].ToString();
                        _attrlists.Add(_Attribute);

                    }
                    _itemAttributeSetupModel1.attributes = _attrlists;
                    _itemAttributeSetupModel1.Title = title;
                    return View("~/Areas/BusinessLayer/Views/ItemAttributeSetup/ItemAttributeSetup.cshtml", _itemAttributeSetupModel1);
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
        public ActionResult Save(ItemAttributeSetupModel _itemAttributeSetupModel,string Command)
        {
            try
            {
                string attr_id, attr_val_id;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string TransType = _itemAttributeSetupModel.hdnAction;
                if (TransType == "DeleteAttrName")
                {
                    attr_id = _itemAttributeSetupModel.attributeID;
                    string SaveMessage = itemAttributeSetup_ISERVICES.DeleteAttr(TransType, attr_id, CompID);
                    string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "DeleteAttrName")
                    {
                        //Session["Message"] = "Deleted";
                        _itemAttributeSetupModel.Message = "Deleted";
                        //Session["collapse"] = "1";
                        _itemAttributeSetupModel.collapse = "1";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "UsedAttr";
                        _itemAttributeSetupModel.Message = "UsedAttr";
                        //Session["collapse"] = "1";
                        _itemAttributeSetupModel.collapse = "1";
                    }
                }
                if (TransType == "DeleteAttrVal")
                {
                    attr_val_id = _itemAttributeSetupModel.attrVal_Id;
                    string SaveMessage = itemAttributeSetup_ISERVICES.DeleteAttr(TransType, attr_val_id, CompID);
                    string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "DeleteAttrVal")
                    {
                        //Session["Message"] = "Deleted";
                        _itemAttributeSetupModel.Message = "Deleted";
                        _itemAttributeSetupModel.collapse = "2";
                        //Session["collapse"] = "2";
                    }
                    if (Message == "Used")
                    {
                        //Session["Message"] = "Used";
                        _itemAttributeSetupModel.Message = "Used";
                        //Session["collapse"] = "2";
                        _itemAttributeSetupModel.collapse = "2";
                    }
                }
                switch (Command)
                {
                    case "Save_attr":

                        TransType = Command;
                        if (_itemAttributeSetupModel.hdnAction != null)
                        {
                            TransType = _itemAttributeSetupModel.hdnAction;
                        }
                        string attrName = _itemAttributeSetupModel.attributeName;

                        if (!string.IsNullOrEmpty(_itemAttributeSetupModel.attributeID))
                        {
                            attr_id = _itemAttributeSetupModel.attributeID;
                        }
                        else
                        {
                            attr_id = "0";
                        }
                        string SaveMessage = itemAttributeSetup_ISERVICES.insertAttributeName(CompID, attr_id, attrName, TransType);
                        string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        //Session["actionCommand"] = TransType;
                        _itemAttributeSetupModel.actionCommand = TransType;
                        if (Message == "Save_attr" || Message == "Update_attr")
                        {
                            //Session["Message"] = "Save";
                            _itemAttributeSetupModel.Message = "Save";
                            //Session["Collapse"] = "1";
                            _itemAttributeSetupModel.collapse = "1";
                        }
                        if (Message == "Duplicate")
                        {
                            //Session["Message"] = "DuplicateAttrName";
                            _itemAttributeSetupModel.Message = "DuplicateAttrName";
                            //Session["attrName"] = attrName;
                            _itemAttributeSetupModel.attrName = attrName;
                            //Session["attr_id"] = attr_id;
                            _itemAttributeSetupModel.attr_id = attr_id;

                        }
                        TempData["ModelData"] = _itemAttributeSetupModel;
                        return RedirectToAction("ItemAttributeSetup");
                    case "SaveAttrVAl":
                        TransType = Command;
                        if (_itemAttributeSetupModel.hdnAction1attrval != null)
                        {
                            TransType = _itemAttributeSetupModel.hdnAction1attrval;
                        }
                        string attrName_id = _itemAttributeSetupModel.L_attribute_Name;
                        string attrValName = _itemAttributeSetupModel.attributeValue;

                        if (!string.IsNullOrEmpty(_itemAttributeSetupModel.attrVal_Id))
                        {
                            attr_val_id = _itemAttributeSetupModel.attrVal_Id;
                        }
                        else
                        {
                            attr_val_id = "0";
                        }
                        SaveMessage = itemAttributeSetup_ISERVICES.insertAttributeVal(CompID, attrName_id, attr_val_id, attrValName, TransType);
                        TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        Message = SaveMessage.Substring(0, SaveMessage.IndexOf('-'));
                        //Session["actionCommand"] = TransType;
                        _itemAttributeSetupModel.actionCommand = TransType;
                        if (Message == "SaveAttrVAl" || Message == "Update_attr_val")
                        {
                            //Session["Message"] = "Save";
                            _itemAttributeSetupModel.Message = "Save";
                            //Session["Collapse"] = "2";
                            _itemAttributeSetupModel.collapse = "2";
                        }
                        if (Message == "Duplicate")
                        {
                            //Session["Message"] = "DuplicateAttrValName";
                            _itemAttributeSetupModel.Message = "DuplicateAttrValName";
                            //Session["attrValName"] = attrValName;
                            //Session["attrName_id"] = attrName_id;
                            //Session["attr_val_id"] = attr_val_id;
                            _itemAttributeSetupModel.attrValName = attrValName;
                            _itemAttributeSetupModel.attrName_id = attrName_id;
                            _itemAttributeSetupModel.attr_val_id = attr_val_id;
                        }
                        TempData["ModelData"] = _itemAttributeSetupModel;
                        return RedirectToAction("ItemAttributeSetup");
                }
                TempData["ModelData"] = _itemAttributeSetupModel;
                return RedirectToAction("ItemAttributeSetup");
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
    }
}