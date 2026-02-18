using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

using EnRepMobileWeb.MODELS.Common;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
//***Modifyed by Shubham Maurya on 21-01-2023 11:52 add Other Details Data Save in DataBase***//
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management
{
    public class DeliveryNoteDetailController : Controller
    {
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string comp_id, BrchID, Dn_no, title, UserID, language, userid = String.Empty;
        DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS;
        string DocumentMenuId = "105102110";
        DataTable dt;
      
        string CompID = string.Empty;
        DeliveryNoteDetail_IServices _DeliveryNoteDetail_IServices;
        Common_IServices _Common_IServices;
        CommonController cmn = new CommonController();
        public DeliveryNoteDetailController(Common_IServices _Common_IServices, DeliveryNoteDetail_IServices _DeliveryNoteDetail_IServices)
        {
            this._DeliveryNoteDetail_IServices = _DeliveryNoteDetail_IServices;
            this._Common_IServices = _Common_IServices;
        }
        public ActionResult DeliveryNoteDetail(URlModelData URLModel)
        {

            ViewBag.DocumentMenuId = DocumentMenuId;
           
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.dn_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            CommonPageDetails();
            try
            {
                var _DeliveryNoteDetail_MODELS = TempData["ModelData"] as DeliveryNoteDetail_MODELS;
                if (_DeliveryNoteDetail_MODELS != null)
                {
                    
                    var other = new CommonController(_Common_IServices);
                   // ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    GetAutoCompleteSupplierName(_DeliveryNoteDetail_MODELS);
                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.po_no = "---Select---";
                    _DocumentNumber.po_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);

                    _DeliveryNoteDetail_MODELS.DocumentNumberList = _DocumentNumberList;

                    List<ItemName> _ItemNameList = new List<ItemName>();
                    ItemName _ItemName = new ItemName();
                    _ItemName.Item_Name = "---Select---";
                    _ItemName.Item_ID = "0";
                    _ItemNameList.Add(_ItemName);

                    _DeliveryNoteDetail_MODELS.ItemNameList = _ItemNameList;

                    _DeliveryNoteDetail_MODELS.ItemDetailsList = null;
                    _DeliveryNoteDetail_MODELS.bill_date = null;
                    
                    _DeliveryNoteDetail_MODELS.dn_dt = DateTime.Now.ToString();
                    _DeliveryNoteDetail_MODELS.src_doc_date = null;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DeliveryNoteDetail_MODELS.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _DeliveryNoteDetail_MODELS.WF_status1 = TempData["WF_status1"].ToString();
                    }
                  
                    if (_DeliveryNoteDetail_MODELS.TransType == "Update")
                    {
                       
                        string dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        DataSet ds = _DeliveryNoteDetail_IServices.GetDeliveryNoteDetailByNo(CompID, dn_no, BrchID, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        _DeliveryNoteDetail_MODELS.PO_Type = ds.Tables[1].Rows[0]["order_type"].ToString();
                        _DeliveryNoteDetail_MODELS.dn_no = ds.Tables[0].Rows[0]["dn_no"].ToString();
                        _DeliveryNoteDetail_MODELS.dn_dt = ds.Tables[0].Rows[0]["dn_dt"].ToString();
                        _DeliveryNoteDetail_MODELS.dn_type = ds.Tables[0].Rows[0]["dn_type"].ToString();
                        _DeliveryNoteDetail_MODELS.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _DeliveryNoteDetail_MODELS.bill_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["bill_date"].ToString());
                        //_DeliveryNoteDetail_MODELS.veh_no = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        _DeliveryNoteDetail_MODELS.veh_load = ds.Tables[0].Rows[0]["veh_load"].ToString();
                        _DeliveryNoteDetail_MODELS.dn_rem = ds.Tables[0].Rows[0]["dn_rem"].ToString();
                        _DeliveryNoteDetail_MODELS.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _DeliveryNoteDetail_MODELS.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DeliveryNoteDetail_MODELS.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DeliveryNoteDetail_MODELS.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DeliveryNoteDetail_MODELS.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DeliveryNoteDetail_MODELS.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DeliveryNoteDetail_MODELS.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DeliveryNoteDetail_MODELS.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DeliveryNoteDetail_MODELS.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _DeliveryNoteDetail_MODELS.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                        {
                            //_DeliveryNoteDetail_MODELS.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                        {
                            _DeliveryNoteDetail_MODELS.FreightAmount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(RateDigit);
                        }
                        //_DeliveryNoteDetail_MODELS.FreightAmount = ds.Tables[0].Rows[0]["freight_amt"].ToString();
                        _DeliveryNoteDetail_MODELS.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                        _DeliveryNoteDetail_MODELS.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                        {
                            _DeliveryNoteDetail_MODELS.veh_load = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(RateDigit);
                        }
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _DeliveryNoteDetail_MODELS.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;

                        if (_DeliveryNoteDetail_MODELS.Status == "Cancelled")
                        {
                            _DeliveryNoteDetail_MODELS.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                            _DeliveryNoteDetail_MODELS.CancelledRemarks = ds.Tables[0].Rows[0]["Cancel_remarks"].ToString();
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS.CancelFlag = false;
                        }
                        _DeliveryNoteDetail_MODELS.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _DeliveryNoteDetail_MODELS.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel.Rows.Count > 0 && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel.Rows.Count > 0 && _DeliveryNoteDetail_MODELS.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["src_doc_date"].ToString()))
                        {
                            _DeliveryNoteDetail_MODELS.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]);
                        }

                            
                        //_DeliveryNoteDetail_MODELS.DocumentNumberList = GetDeliveryNoteSourceDocument(_DeliveryNoteDetail_MODELS);
                        _DeliveryNoteDetail_MODELS.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        if (_DeliveryNoteDetail_MODELS.src_doc_no == "0")
                        {
                            objDocumentNumber.po_dt = "0";
                            objDocumentNumber.po_no = "---Select---";
                        }
                        else
                        {
                            objDocumentNumber.po_no = _DeliveryNoteDetail_MODELS.src_doc_no;
                            objDocumentNumber.po_dt = _DeliveryNoteDetail_MODELS.src_doc_no;
                        }
                        
                        if (!_DeliveryNoteDetail_MODELS.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            _DeliveryNoteDetail_MODELS.DocumentNumberList.Add(objDocumentNumber);
                        }


                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DeliveryNoteDetail_MODELS.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                       // ViewBag.MenuPageName = getDocumentName();
                        _DeliveryNoteDetail_MODELS.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString();
                     //   ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS);
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (_DeliveryNoteDetail_MODELS.BtnName == "Refresh")
                    {
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        _DeliveryNoteDetail_MODELS.Title = title;
                        _DeliveryNoteDetail_MODELS.DocumentStatus = "New";
                     //   ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS);

                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                       // ViewBag.MenuPageName = getDocumentName();
                        _DeliveryNoteDetail_MODELS.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _DeliveryNoteDetail_MODELS.DocumentStatus = "New";
                      //  ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS);
                    }
                }
                else
                {/*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/

                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS1 = new DeliveryNoteDetail_MODELS();
                    if (URLModel.dn_no != null)
                    {
                        _DeliveryNoteDetail_MODELS1.dn_no = URLModel.dn_no;
                    }
                    if (URLModel.TransType != null)
                    {
                        _DeliveryNoteDetail_MODELS1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _DeliveryNoteDetail_MODELS1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _DeliveryNoteDetail_MODELS1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _DeliveryNoteDetail_MODELS1.BtnName = "Refresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _DeliveryNoteDetail_MODELS1.Command = URLModel.Command;
                    }
                    else
                    {
                        _DeliveryNoteDetail_MODELS1.Command = "Refresh";
                    }
                    var other = new CommonController(_Common_IServices);
                    //  ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                  
                    GetAutoCompleteSupplierName(_DeliveryNoteDetail_MODELS1);
                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.po_no = "---Select---";
                    _DocumentNumber.po_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);


                    List<ItemName> _ItemNameList = new List<ItemName>();
                    ItemName _ItemName = new ItemName();
                    _ItemName.Item_Name = "---Select---";
                    _ItemName.Item_ID = "0";
                    _ItemNameList.Add(_ItemName);

                    _DeliveryNoteDetail_MODELS1.ItemNameList = _ItemNameList;

                    _DeliveryNoteDetail_MODELS1.DocumentNumberList = _DocumentNumberList;
                    _DeliveryNoteDetail_MODELS1.ItemDetailsList = null;
                    _DeliveryNoteDetail_MODELS1.bill_date = null;
                    //_DeliveryNoteDetail_MODELS1.dn_dt = DateTime.Now;
                    _DeliveryNoteDetail_MODELS1.dn_dt = DateTime.Now.ToString();
                    _DeliveryNoteDetail_MODELS1.src_doc_date = null;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DeliveryNoteDetail_MODELS1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _DeliveryNoteDetail_MODELS1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_DeliveryNoteDetail_MODELS1.TransType == "Update")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //BrchID = Session["BranchId"].ToString();
                        //string dn_no = Session["DeliveryNoteNo"].ToString();
                        string dn_no = _DeliveryNoteDetail_MODELS1.dn_no;
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        DataSet ds = _DeliveryNoteDetail_IServices.GetDeliveryNoteDetailByNo(CompID, dn_no, BrchID, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        _DeliveryNoteDetail_MODELS1.dn_no = ds.Tables[0].Rows[0]["dn_no"].ToString();
                        _DeliveryNoteDetail_MODELS1.PO_Type = ds.Tables[1].Rows[0]["order_type"].ToString();
                        //_DeliveryNoteDetail_MODELS1.dn_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["dn_dt"].ToString());
                        _DeliveryNoteDetail_MODELS1.dn_dt = ds.Tables[0].Rows[0]["dn_dt"].ToString();
                        _DeliveryNoteDetail_MODELS1.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _DeliveryNoteDetail_MODELS1.dn_type = ds.Tables[0].Rows[0]["dn_type"].ToString();
                        _DeliveryNoteDetail_MODELS1.bill_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["bill_date"].ToString());
                        //_DeliveryNoteDetail_MODELS1.veh_no = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        _DeliveryNoteDetail_MODELS1.veh_load = ds.Tables[0].Rows[0]["veh_load"].ToString();
                        _DeliveryNoteDetail_MODELS1.dn_rem = ds.Tables[0].Rows[0]["dn_rem"].ToString();
                        _DeliveryNoteDetail_MODELS1.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _DeliveryNoteDetail_MODELS1.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DeliveryNoteDetail_MODELS1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DeliveryNoteDetail_MODELS1.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DeliveryNoteDetail_MODELS1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DeliveryNoteDetail_MODELS1.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DeliveryNoteDetail_MODELS1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DeliveryNoteDetail_MODELS1.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DeliveryNoteDetail_MODELS1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _DeliveryNoteDetail_MODELS1.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                        {
                            //_DeliveryNoteDetail_MODELS1.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS1.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                        {
                            _DeliveryNoteDetail_MODELS1.FreightAmount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS1.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(RateDigit);
                        }
                        //_DeliveryNoteDetail_MODELS1.FreightAmount = ds.Tables[0].Rows[0]["freight_amt"].ToString();
                        _DeliveryNoteDetail_MODELS1.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                        _DeliveryNoteDetail_MODELS1.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                        {
                            _DeliveryNoteDetail_MODELS1.veh_load = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS1.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(RateDigit);
                        }
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _DeliveryNoteDetail_MODELS1.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;

                        if (_DeliveryNoteDetail_MODELS1.Status == "Cancelled")
                        {
                            _DeliveryNoteDetail_MODELS1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _DeliveryNoteDetail_MODELS1.BtnName = "Refresh";
                            _DeliveryNoteDetail_MODELS1.CancelledRemarks = ds.Tables[0].Rows[0]["Cancel_remarks"].ToString();
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS1.CancelFlag = false;
                        }
                        _DeliveryNoteDetail_MODELS1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _DeliveryNoteDetail_MODELS1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel.Rows.Count > 0 && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel.Rows.Count > 0 && _DeliveryNoteDetail_MODELS1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DeliveryNoteDetail_MODELS1.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["src_doc_date"].ToString()))
                        {
                            _DeliveryNoteDetail_MODELS1.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]);
                        }
                        
                        //_DeliveryNoteDetail_MODELS1.DocumentNumberList = GetDeliveryNoteSourceDocument(_DeliveryNoteDetail_MODELS1);
                        _DeliveryNoteDetail_MODELS1.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        if (_DeliveryNoteDetail_MODELS1.src_doc_no == "0")
                        {
                            objDocumentNumber.po_dt = "0";
                            objDocumentNumber.po_no = "---Select---";
                        }
                        else
                        {
                            objDocumentNumber.po_no = _DeliveryNoteDetail_MODELS1.src_doc_no;
                            objDocumentNumber.po_dt = _DeliveryNoteDetail_MODELS1.src_doc_no;
                        }
                        
                        if (!_DeliveryNoteDetail_MODELS1.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            _DeliveryNoteDetail_MODELS1.DocumentNumberList.Add(objDocumentNumber);
                        }


                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DeliveryNoteDetail_MODELS1.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                     //   ViewBag.MenuPageName = getDocumentName();
                        _DeliveryNoteDetail_MODELS1.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS1);
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (_DeliveryNoteDetail_MODELS1.BtnName == "Refresh")
                    {
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        _DeliveryNoteDetail_MODELS1.DocumentStatus = "New";
                        _DeliveryNoteDetail_MODELS1.Title = title;
                        // ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS1);

                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                      //  ViewBag.MenuPageName = getDocumentName();
                        _DeliveryNoteDetail_MODELS1.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _DeliveryNoteDetail_MODELS1.DocumentStatus = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS1);
                    }
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
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeliverynoteSave(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string dn_no, string command, HttpPostedFileBase[] DnFiles)
        {
            try
            {/*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_DeliveryNoteDetail_MODELS.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNewDeliveryNote":
                        DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELSAddNew = new DeliveryNoteDetail_MODELS();
                        _DeliveryNoteDetail_MODELSAddNew.AppStatus = "D";
                        _DeliveryNoteDetail_MODELSAddNew.BtnName = "BtnAddNew";
                        _DeliveryNoteDetail_MODELSAddNew.TransType = "Save";
                        _DeliveryNoteDetail_MODELSAddNew.Command = "New";
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELSAddNew;
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                              if (!string.IsNullOrEmpty(_DeliveryNoteDetail_MODELS.dn_no))
                                return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                            else
                                _DeliveryNoteDetail_MODELSAddNew.Command = "Refresh";
                            _DeliveryNoteDetail_MODELSAddNew.TransType = "Refresh";
                            _DeliveryNoteDetail_MODELSAddNew.BtnName = "Refresh";
                            _DeliveryNoteDetail_MODELSAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _DeliveryNoteDetail_MODELSAddNew;
                            return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail", _DeliveryNoteDetail_MODELSAddNew);

                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail");

                    case "Edit":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no,DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string DnDt = _DeliveryNoteDetail_MODELS.dn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, DnDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        checkDependency(_DeliveryNoteDetail_MODELS);
                        if (_DeliveryNoteDetail_MODELS.Message == "Used")
                        {
                            _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                            _DeliveryNoteDetail_MODELS.TransType = "Update";
                            _DeliveryNoteDetail_MODELS.Command = "Refresh";
                        }
                        else
                        {
                            _DeliveryNoteDetail_MODELS.TransType = "Update";
                            _DeliveryNoteDetail_MODELS.Command = command;
                            _DeliveryNoteDetail_MODELS.BtnName = "BtnEdit";
                            _DeliveryNoteDetail_MODELS.Message = null;
                        }
                        URlModelData URLModelEdit = new URlModelData();
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
                        URLModelEdit.TransType = _DeliveryNoteDetail_MODELS.TransType;
                        URLModelEdit.Command = _DeliveryNoteDetail_MODELS.Command;
                        URLModelEdit.BtnName = _DeliveryNoteDetail_MODELS.BtnName;
                        URLModelEdit.dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                        URLModelEdit.dn_dt = _DeliveryNoteDetail_MODELS.dn_dt;
                        TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                        return RedirectToAction("DeliveryNoteDetail", URLModelEdit);

                    case "Delete":
                        DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELSDelete = new DeliveryNoteDetail_MODELS();
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        //_DeliveryNoteDetail_MODELSDelete.Command = command;
                        //_DeliveryNoteDetail_MODELSDelete.BtnName = "Refresh";
                        Dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                        DeliveryNoteDelete(_DeliveryNoteDetail_MODELS, command, _DeliveryNoteDetail_MODELS.Title);
                        _DeliveryNoteDetail_MODELSDelete.Message = "Deleted";
                        _DeliveryNoteDetail_MODELSDelete.Command = "Refresh";
                        _DeliveryNoteDetail_MODELSDelete.TransType = command;
                        _DeliveryNoteDetail_MODELSDelete.AppStatus = "D";
                        _DeliveryNoteDetail_MODELSDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELSDelete;
                        TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                        return RedirectToAction("DeliveryNoteDetail");

                    case "Save":
                        //Session["Command"] = command;                     
                        _DeliveryNoteDetail_MODELS.Command = command;
                        SaveDeliveryNote(_DeliveryNoteDetail_MODELS, DnFiles, _DeliveryNoteDetail_MODELS.Title);
                        if (_DeliveryNoteDetail_MODELS.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_DeliveryNoteDetail_MODELS.Message == "DocModify")
                        {
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
                            ViewBag.DocumentCode = "D";

                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _DeliveryNoteDetail_MODELS.supp_id, supp_name = _DeliveryNoteDetail_MODELS.SupplierName });
                            _DeliveryNoteDetail_MODELS.SupplierNameList = suppLists;

                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.po_no = _DeliveryNoteDetail_MODELS.src_doc_no;
                            _DocumentNumber.po_dt = Convert.ToString(_DeliveryNoteDetail_MODELS.src_doc_date);
                            _DocumentNumberList.Add(_DocumentNumber);

                            _DeliveryNoteDetail_MODELS.DocumentNumberList = _DocumentNumberList;
                           
                           
                            _DeliveryNoteDetail_MODELS.dn_dt = DateTime.Now.ToString();
                            _DeliveryNoteDetail_MODELS.bill_no = _DeliveryNoteDetail_MODELS.bill_no;
                            _DeliveryNoteDetail_MODELS.bill_date = _DeliveryNoteDetail_MODELS.bill_date;
                            _DeliveryNoteDetail_MODELS.SupplierName = _DeliveryNoteDetail_MODELS.SupplierName;
                            _DeliveryNoteDetail_MODELS.src_doc_no = _DeliveryNoteDetail_MODELS.src_doc_no;
                            _DeliveryNoteDetail_MODELS.src_doc_date = _DeliveryNoteDetail_MODELS.src_doc_date;
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                            _DeliveryNoteDetail_MODELS.Command = "Refresh";
                            _DeliveryNoteDetail_MODELS.DocumentStatus = "D";

                            //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            //ViewBag.ValDigit = ValDigit;
                            //ViewBag.QtyDigit = QtyDigit;
                            //ViewBag.RateDigit = RateDigit;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNote.cshtml", _DeliveryNoteDetail_MODELS);
                        }
                        else
                        {
                            //Session["DeliveryNoteNo"] = Session["DeliveryNoteNo"].ToString();
                            TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
                            URlModelData URLModelSave = new URlModelData();
                            URLModelSave.dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                            URLModelSave.dn_dt = _DeliveryNoteDetail_MODELS.dn_dt;
                            URLModelSave.TransType = _DeliveryNoteDetail_MODELS.TransType;
                            URLModelSave.BtnName = _DeliveryNoteDetail_MODELS.BtnName;
                            URLModelSave.Command = _DeliveryNoteDetail_MODELS.Command;
                            TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                            return RedirectToAction("DeliveryNoteDetail", URLModelSave);
                        }
                       

                    case "Forward":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string DnDt1 = _DeliveryNoteDetail_MODELS.dn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, DnDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string DnDt2 = _DeliveryNoteDetail_MODELS.dn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, DnDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDeliveryNote", "DeliveryNoteList", new { DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no, DeliveryNoteDt = _DeliveryNoteDetail_MODELS.dn_dt, ListFilterData = _DeliveryNoteDetail_MODELS.ListFilterData1, WF_status = _DeliveryNoteDetail_MODELS.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _DeliveryNoteDetail_MODELS.Command = command;
                        Dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                        string Dn_dt = _DeliveryNoteDetail_MODELS.dn_dt.ToString();
                        //Session["DeliveryNoteNo"] = Dn_no;
                        _DeliveryNoteDetail_MODELS.dn_no = Dn_no;
                        DeliveryNoteApprove(Dn_no, Dn_dt, "", "", "", "", "");
                        _DeliveryNoteDetail_MODELS.TransType = "Update";
                        _DeliveryNoteDetail_MODELS.Command = "Approve";
                        _DeliveryNoteDetail_MODELS.Message = "Approved";
                        _DeliveryNoteDetail_MODELS.AppStatus = "D";
                        _DeliveryNoteDetail_MODELS.BtnName = "BtnEdit";
                        URlModelData URLModelApprove = new URlModelData();
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
                        URLModelApprove.dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                        URLModelApprove.dn_dt = _DeliveryNoteDetail_MODELS.dn_dt;
                        URLModelApprove.TransType = "Update";
                        URLModelApprove.BtnName = "BtnEdit";
                        URLModelApprove.Command = "Approve";
                        TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                        return RedirectToAction("DeliveryNoteDetail", URLModelApprove);

                    case "Refresh":
                        DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELSRefresh = new DeliveryNoteDetail_MODELS();
                        _DeliveryNoteDetail_MODELSRefresh.BtnName = "Refresh";
                        _DeliveryNoteDetail_MODELSRefresh.Command = command;
                        _DeliveryNoteDetail_MODELSRefresh.TransType = "Save";
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELSRefresh;
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                        return RedirectToAction("DeliveryNoteDetail");

                    case "Print":
                        return GenratePdfFile(_DeliveryNoteDetail_MODELS);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["WF_status"] = _DeliveryNoteDetail_MODELS.WF_status1;
                        TempData["ListFilterData"] = _DeliveryNoteDetail_MODELS.ListFilterData1;
                        return RedirectToAction("DeliveryNoteList", "DeliveryNoteList");

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
        public ActionResult checkDependency(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                var dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                var dn_dt = _DeliveryNoteDetail_MODELS.dn_dt;

                DataSet ds = _DeliveryNoteDetail_IServices.checkDependency(CompID, BrchID, dn_no, dn_dt);
                if (ds.Tables[0].Rows.Count > 0 )
                {
                    _DeliveryNoteDetail_MODELS.Message = "Used";
                }
                return RedirectToAction("DeliveryNoteDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveDeliveryNote(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, HttpPostedFileBase[] DnFiles, string title)
        {
            //if (Session["CompId"] != null)
            //    CompID = Session["CompId"].ToString();
            //if (Session["BranchId"] != null)
            //    BrchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message"] = "Financial Year not Exist";
            //    return RedirectToAction("DeliveryNoteList", "DeliveryNoteList");
            //}
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            try
            {

                if (_DeliveryNoteDetail_MODELS.CancelFlag == false)
                {
                    DataTable DeliveryNoteHeader = new DataTable();
                    DataTable DeliveryNoteItemDetails = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("dn_type", typeof(string));
                    dt.Columns.Add("CreateBy", typeof(string));
                    dt.Columns.Add("dn_status", typeof(string));
                    dt.Columns.Add("dn_no", typeof(string));
                    dt.Columns.Add("bill_date", typeof(DateTime));
                    dt.Columns.Add("bill_no", typeof(string));
                    dt.Columns.Add("veh_no", typeof(string));
                    dt.Columns.Add("veh_load", typeof(string));
                    dt.Columns.Add("dn_rem", typeof(string));
                    dt.Columns.Add("supp_id", typeof(int));
                    dt.Columns.Add("UserName", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));
                    dt.Columns.Add("dn_dt", typeof(DateTime));
                    dt.Columns.Add("src_doc_no", typeof(string));
                    dt.Columns.Add("src_doc_date", typeof(string));
                    dt.Columns.Add("imp_file_no", typeof(string));
                    dt.Columns.Add("cntry_origin", typeof(string));

                    dt.Columns.Add("gr_no", typeof(string));
                    if (_DeliveryNoteDetail_MODELS.GRDate != null)
                    {
                        dt.Columns.Add("gr_date", typeof(DateTime));
                    }
                    else
                    {
                        dt.Columns.Add("gr_date", typeof(string));
                    }
                    dt.Columns.Add("freight_amt", typeof(string));
                    dt.Columns.Add("trans_name", typeof(string));


                    DataRow dtrow = dt.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["dn_type"] = _DeliveryNoteDetail_MODELS.dn_type;
                    dtrow["CreateBy"] = Session["UserId"].ToString();
                    //dtrow["dn_status"] = Session["AppStatus"].ToString();
                    dtrow["dn_status"] = IsNull(_DeliveryNoteDetail_MODELS.AppStatus, "D");
                    dtrow["dn_no"] = _DeliveryNoteDetail_MODELS.dn_no;
                    dtrow["bill_date"] = _DeliveryNoteDetail_MODELS.bill_date;
                    dtrow["bill_no"] = _DeliveryNoteDetail_MODELS.bill_no;
                    //dtrow["veh_no"] = _DeliveryNoteDetail_MODELS.veh_no;
                    if (!string.IsNullOrEmpty(_DeliveryNoteDetail_MODELS.veh_load))
                    {
                        dtrow["veh_load"] = _DeliveryNoteDetail_MODELS.veh_load;
                    }
                    else
                    {
                        dtrow["veh_load"] = "0";
                    }
                    dtrow["dn_rem"] = _DeliveryNoteDetail_MODELS.dn_rem;
                    if (_DeliveryNoteDetail_MODELS.SupplierID == null)
                    {
                        dtrow["supp_id"] = DBNull.Value;
                    }
                    else
                    {
                        dtrow["supp_id"] = _DeliveryNoteDetail_MODELS.SupplierID;
                    }
                    dtrow["UserName"] = Session["UserName"].ToString();
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();
                    //dtrow["TransType"] = Session["TransType"].ToString();
                    dtrow["TransType"] = _DeliveryNoteDetail_MODELS.TransType;
                    dtrow["dn_dt"] = _DeliveryNoteDetail_MODELS.dn_dt;// DateTime.Now;
                    dtrow["src_doc_no"] = _DeliveryNoteDetail_MODELS.src_doc_no;
                    dtrow["src_doc_date"] = _DeliveryNoteDetail_MODELS.src_doc_date;
                    dtrow["imp_file_no"] = _DeliveryNoteDetail_MODELS.imp_file_no;
                    dtrow["cntry_origin"] = _DeliveryNoteDetail_MODELS.cntry_origin;

                    dtrow["gr_no"] = _DeliveryNoteDetail_MODELS.GRNumber;
                    dtrow["gr_date"] = _DeliveryNoteDetail_MODELS.GRDate;
                    dtrow["freight_amt"] = _DeliveryNoteDetail_MODELS.FreightAmount;
                    dtrow["trans_name"] = _DeliveryNoteDetail_MODELS.TransporterName;
                    dtrow["veh_no"] = _DeliveryNoteDetail_MODELS.VehicleNumber;

                    dt.Rows.Add(dtrow);

                    DeliveryNoteHeader = dt;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("dn_type", typeof(string));
                    dtItem.Columns.Add("dn_no", typeof(string));
                    dtItem.Columns.Add("doc_no", typeof(string));
                    dtItem.Columns.Add("doc_date", typeof(DateTime));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("ord_qty", typeof(string));
                    dtItem.Columns.Add("bill_qty", typeof(string));
                    dtItem.Columns.Add("recd_qty", typeof(string));
                    dtItem.Columns.Add("qc_check", typeof(string));
                    dtItem.Columns.Add("accept_qty", typeof(string));
                    dtItem.Columns.Add("reject_qty", typeof(string));
                    dtItem.Columns.Add("rework_qty", typeof(string));
                    dtItem.Columns.Add("sam_rec", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("pend_qty", typeof(string));
                    dtItem.Columns.Add("diff_qty", typeof(string));

                    JArray jObject = JArray.Parse(_DeliveryNoteDetail_MODELS.DnItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Ord_qty, Bill_qty, Recd_qty, Accept_qty, Reject_qty, Rework_qty;
                        if (jObject[i]["OrderQty"].ToString() == "")
                            Ord_qty = 0;
                        else
                            Ord_qty = Convert.ToDecimal(jObject[i]["OrderQty"].ToString());

                        if (jObject[i]["BillledQty"].ToString() == "")
                            Bill_qty = 0;
                        else
                            Bill_qty = Convert.ToDecimal(jObject[i]["BillledQty"].ToString());
                        
                        if (jObject[i]["RecievedQty"].ToString() == "")
                            Recd_qty = 0;
                        else
                            Recd_qty = Convert.ToDecimal(jObject[i]["RecievedQty"].ToString());

                        if (jObject[i]["AcceptedQty"].ToString() == "")
                            Accept_qty = 0;
                        else
                            Accept_qty = Convert.ToDecimal(jObject[i]["AcceptedQty"].ToString());

                        if (jObject[i]["RejectedQty"].ToString() == "")
                            Reject_qty = 0;
                        else
                            Reject_qty = Convert.ToDecimal(jObject[i]["RejectedQty"].ToString());

                        if (jObject[i]["ReworkableQty"].ToString() == "")
                            Rework_qty = 0;
                        else
                            Rework_qty = Convert.ToDecimal(jObject[i]["ReworkableQty"].ToString());

                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                        dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                        dtrowItemdetails["dn_type"] = _DeliveryNoteDetail_MODELS.dn_type;
                        dtrowItemdetails["dn_no"] = _DeliveryNoteDetail_MODELS.dn_no;
                        dtrowItemdetails["doc_no"] = jObject[i]["SourceDocumentNo"].ToString();
                        dtrowItemdetails["doc_date"] = jObject[i]["SourceDocumentDate"].ToString();
                        dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                        string str = Convert.ToInt32(jObject[i]["UOMId"]).ToString();
                        dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                        dtrowItemdetails["ord_qty"] = Ord_qty;
                        dtrowItemdetails["bill_qty"] = Bill_qty;
                        dtrowItemdetails["recd_qty"] = Recd_qty;
                        dtrowItemdetails["qc_check"] = jObject[i]["QCRequired"].ToString();
                        dtrowItemdetails["accept_qty"] = Accept_qty;
                        dtrowItemdetails["reject_qty"] = Reject_qty;
                        dtrowItemdetails["rework_qty"] = Rework_qty;
                        dtrowItemdetails["sam_rec"] = jObject[i]["SampleRecieved"].ToString();
                        dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                        dtrowItemdetails["pend_qty"] = jObject[i]["PendingQuantity"].ToString();
                        dtrowItemdetails["diff_qty"] = jObject[i]["DifferenceQuantity"].ToString();
                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    DeliveryNoteItemDetails = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("Ord_qty", typeof(string));
                    dtSubItem.Columns.Add("Pend_qty", typeof(string));
                    dtSubItem.Columns.Add("Bill_qty", typeof(string));
                    dtSubItem.Columns.Add("Rec_qty", typeof(string));
                    dtSubItem.Columns.Add("src_doc_no", typeof(string));
                    dtSubItem.Columns.Add("src_doc_date", typeof(string));

                    if (_DeliveryNoteDetail_MODELS.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_DeliveryNoteDetail_MODELS.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["Ord_qty"] = jObject2[i]["OrderQty"].ToString();
                            dtrowItemdetails["Pend_qty"] = jObject2[i]["PendingQuantity"].ToString();
                            dtrowItemdetails["Bill_qty"] = jObject2[i]["BilledQty"].ToString();
                            dtrowItemdetails["Rec_qty"] = jObject2[i]["ReceivedQuantity"].ToString();
                            dtrowItemdetails["src_doc_no"] = jObject2[i]["src_doc_no"].ToString();
                            dtrowItemdetails["src_doc_date"] = jObject2[i]["src_doc_date"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/

                    DataTable dtAttachment = new DataTable();
                    var _DeliveryNoteModelAttch = TempData["ModelDataattch"] as DeliveryNoteModelAttch;
                    TempData["ModelDataattch"] = null;
                    if (_DeliveryNoteDetail_MODELS.attatchmentdetail != null)
                    {
                        if (_DeliveryNoteModelAttch != null)
                        {
                            if (_DeliveryNoteModelAttch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _DeliveryNoteModelAttch.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                dtAttachment.Columns.Add("id", typeof(string));
                                dtAttachment.Columns.Add("file_name", typeof(string));
                                dtAttachment.Columns.Add("file_path", typeof(string));
                                dtAttachment.Columns.Add("file_def", typeof(char));
                                dtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        else
                        {
                            if (_DeliveryNoteDetail_MODELS.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _DeliveryNoteDetail_MODELS.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                dtAttachment.Columns.Add("id", typeof(string));
                                dtAttachment.Columns.Add("file_name", typeof(string));
                                dtAttachment.Columns.Add("file_path", typeof(string));
                                dtAttachment.Columns.Add("file_def", typeof(char));
                                dtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        JArray jObject1 = JArray.Parse(_DeliveryNoteDetail_MODELS.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in dtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = dtAttachment.NewRow();
                                if (!string.IsNullOrEmpty(_DeliveryNoteDetail_MODELS.dn_no))
                                {
                                    dtrowAttachment1["id"] = _DeliveryNoteDetail_MODELS.dn_no;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                dtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_DeliveryNoteDetail_MODELS.TransType == "Update")
                        {

                            string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_DeliveryNoteDetail_MODELS.dn_no))
                                {
                                    ItmCode = _DeliveryNoteDetail_MODELS.dn_no;
                                }
                                else
                                {
                                    ItmCode = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + ItmCode.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in dtAttachment.Rows)
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
                        Attachments = dtAttachment;
                    }
                    SaveMessage = _DeliveryNoteDetail_IServices.insertDeliveryNoteDetails(DeliveryNoteHeader, DeliveryNoteItemDetails, Attachments, dtSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _DeliveryNoteDetail_MODELS.Message = "DocModify";
                        _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                        _DeliveryNoteDetail_MODELS.Command = "Refresh";
                        TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
                        return RedirectToAction("DeliveryNoteDetail");
                    }
                    else
                    {
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        string DeliveryNoteNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                        if (Message == "Data_Not_Found")
                        {
                            var a = DeliveryNoteNo.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim()+" in "+PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _DeliveryNoteDetail_MODELS.Message = Message.Replace("_", "");
                            return RedirectToAction("DeliveryNoteDetail");
                        }
                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_DeliveryNoteModelAttch != null)
                            {
                                //if (Session["Guid"] != null)
                                if (_DeliveryNoteModelAttch.Guid != null)
                                {
                                    //Guid = Session["Guid"].ToString();
                                    Guid = _DeliveryNoteModelAttch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, DeliveryNoteNo, _DeliveryNoteDetail_MODELS.TransType, Attachments);

                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
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
                            //                string DeliveryNoteNo1 = DeliveryNoteNo.Replace("/", "");
                            //                string img_nm = CompID + BrchID + DeliveryNoteNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        if (Message == "Update" || Message == "Save")
                            _DeliveryNoteDetail_MODELS.Message = "Save";
                        _DeliveryNoteDetail_MODELS.Command = "Update";
                        _DeliveryNoteDetail_MODELS.dn_no = DeliveryNoteNo;
                        _DeliveryNoteDetail_MODELS.TransType = "Update";
                        _DeliveryNoteDetail_MODELS.AppStatus = "D";
                        _DeliveryNoteDetail_MODELS.BtnName = "BtnEdit";
                        _DeliveryNoteDetail_MODELS.AttachMentDetailItmStp = null;
                        _DeliveryNoteDetail_MODELS.Guid = null;
                        //Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["DeliveryNoteNo"] = DeliveryNoteNo;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnEdit";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                        return RedirectToAction("DeliveryNoteDetail");
                    }
                    
                }
                else
                {
                    checkDependency(_DeliveryNoteDetail_MODELS);//Dependency check Added by Suraj Maurya on 04-11-2025
                    if (_DeliveryNoteDetail_MODELS.Message != "Used")
                    {
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        SaveMessage = _DeliveryNoteDetail_IServices.DeliveryNoteCancel(_DeliveryNoteDetail_MODELS, CompID, userid, br_id, mac_id);
                        try
                        {
                            // string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "GateEntry_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_DeliveryNoteDetail_MODELS.dn_no, _DeliveryNoteDetail_MODELS.dn_dt, fileName, DocumentMenuId, "C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _DeliveryNoteDetail_MODELS.dn_no, "C", userid, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _DeliveryNoteDetail_MODELS.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }

                        // _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _DeliveryNoteDetail_MODELS.dn_no, "C", userid, "");
                        string DeliveryNoteNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        _DeliveryNoteDetail_MODELS.Message = _DeliveryNoteDetail_MODELS.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_DeliveryNoteDetail_MODELS.Message = "Cancelled";
                        _DeliveryNoteDetail_MODELS.Command = "Update";
                        _DeliveryNoteDetail_MODELS.dn_no = DeliveryNoteNo;
                        _DeliveryNoteDetail_MODELS.TransType = "Update";
                        _DeliveryNoteDetail_MODELS.AppStatus = "D";
                        _DeliveryNoteDetail_MODELS.BtnName = "Refresh";
                    }
                    else
                    {
                        _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
                        _DeliveryNoteDetail_MODELS.TransType = "Update";
                        _DeliveryNoteDetail_MODELS.Command = "Refresh";
                    }

                    return RedirectToAction("DeliveryNoteDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_DeliveryNoteDetail_MODELS.TransType.ToString() == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_DeliveryNoteDetail_MODELS.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _DeliveryNoteDetail_MODELS.Guid.ToString();
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public DataTable dtitemdetail(JArray jObject)
        {
            
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("src_doc_no", typeof(string));
            //dtItem.Columns.Add("src_doc_date", typeof(string));
            dtItem.Columns.Add("Hdn_SrcDocDate", typeof(string));
            dtItem.Columns.Add("src_doc_date", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("base_uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("ord_qty", typeof(string));
            dtItem.Columns.Add("pending_qty", typeof(string));
            dtItem.Columns.Add("bill_qty", typeof(string));
            dtItem.Columns.Add("recd_qty", typeof(string));
            dtItem.Columns.Add("qc_check", typeof(string));
            dtItem.Columns.Add("accept_qty", typeof(string));
            dtItem.Columns.Add("reject_qty", typeof(string));
            dtItem.Columns.Add("rework_qty", typeof(string));
            dtItem.Columns.Add("sam_rec", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("diff_qty", typeof(string));
   
            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowItemdetails = dtItem.NewRow();
               
                dtrowItemdetails["src_doc_no"] = jObject[i]["SourceDocumentNo"].ToString();
                //dtrowItemdetails["src_doc_date"] = jObject[i]["SourceDocumentDate"].ToString();
                dtrowItemdetails["Hdn_SrcDocDate"] = Convert.ToString(jObject[i]["SourceDocumentDate"].ToString());
                dtrowItemdetails["src_doc_date"] = Convert.ToDateTime(jObject[i]["SourceDocumentDate"].ToString()).ToString("dd-MM-yyyy");
                dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowItemdetails["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowItemdetails["base_uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                dtrowItemdetails["uom_name"] = jObject[i]["uom_name"].ToString();

                decimal Ordqty, Billqty, Recdqty, Acceptqty, Rejectqty, Reworkqty;
                if (jObject[i]["OrderQty"].ToString() == "")
                { Ordqty =0;}
                else
                {
                    Ordqty = Convert.ToDecimal(jObject[i]["OrderQty"].ToString());
                }

                if (jObject[i]["BillledQty"].ToString() == "")
                { Billqty = 0;}
                else
                {
                    Billqty = Convert.ToDecimal(jObject[i]["BillledQty"].ToString());
                }
                if (jObject[i]["RecievedQty"].ToString() == "")
                { Recdqty = 0;}
                else
                {
                    Recdqty = Convert.ToDecimal(jObject[i]["RecievedQty"].ToString());
                }
                if (jObject[i]["AcceptedQty"].ToString() == "")
                { Acceptqty = 0; }
                else
                {
                    Acceptqty = Convert.ToDecimal(jObject[i]["AcceptedQty"].ToString());
                }
                if (jObject[i]["RejectedQty"].ToString() == "")
                { Rejectqty = 0; }
                else
                {
                    Rejectqty = Convert.ToDecimal(jObject[i]["RejectedQty"].ToString());
                }
                if (jObject[i]["ReworkableQty"].ToString() == "")
                { Reworkqty = 0; }
                else
                {
                    Reworkqty = Convert.ToDecimal(jObject[i]["ReworkableQty"].ToString());
                }
                dtrowItemdetails["ord_qty"] = Ordqty;
                dtrowItemdetails["pending_qty"] = jObject[i]["PendingQuantity"].ToString();
                dtrowItemdetails["bill_qty"] = Billqty;
                dtrowItemdetails["recd_qty"] = Recdqty;
                dtrowItemdetails["qc_check"] = jObject[i]["QCRequired"].ToString();
                dtrowItemdetails["accept_qty"] = Acceptqty;
                dtrowItemdetails["reject_qty"] = Rejectqty;
                dtrowItemdetails["rework_qty"] = Reworkqty;
                dtrowItemdetails["sam_rec"] = jObject[i]["SampleRecieved"].ToString();
                dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                dtrowItemdetails["diff_qty"] = jObject[i]["DifferenceQuantity"].ToString();
                
                dtItem.Rows.Add(dtrowItemdetails);
            }

            return dtItem;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("Order_Qty", typeof(string));
            dtSubItem.Columns.Add("Pending_Qty", typeof(string));
            dtSubItem.Columns.Add("BilledQty", typeof(string));
            dtSubItem.Columns.Add("RecievedQty", typeof(string));
            dtSubItem.Columns.Add("Accept_Qty", typeof(string));
            dtSubItem.Columns.Add("Reject_Qty", typeof(string));
            dtSubItem.Columns.Add("Rework_Qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["Order_Qty"] = jObject2[i]["OrderQty"].ToString();
                dtrowItemdetails["Pending_Qty"] = jObject2[i]["PendingQuantity"].ToString();
                dtrowItemdetails["BilledQty"] = jObject2[i]["BilledQty"].ToString();
                dtrowItemdetails["RecievedQty"] = jObject2[i]["ReceivedQuantity"].ToString();
                dtrowItemdetails["Accept_Qty"] = jObject2[i]["AcceptedQty"].ToString();
                dtrowItemdetails["Reject_Qty"] = jObject2[i]["RejectedQty"].ToString();
                dtrowItemdetails["Rework_Qty"] = jObject2[i]["ReworkableQty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                DeliveryNoteModelAttch _DeliveryNoteModelAttch = new DeliveryNoteModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string DeliveryNoteNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["DeliveryNoteNo"] != null)
                //{
                //    DeliveryNoteNo = Session["DeliveryNoteNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _DeliveryNoteModelAttch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _DeliveryNoteModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _DeliveryNoteModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _DeliveryNoteModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        private List<SupplierName> getSupplierListall(string SupplierName)
        {
            try
            {
                List<SupplierName> _SupplierNameList = new List<SupplierName>();
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                string SuppName = SupplierName;
                dt = _DeliveryNoteDetail_IServices.GetSupplierListALl(CompID, SuppName, BrchID);
                foreach (DataRow dr in dt.Rows)
                {
                    SupplierName _SupplierName = new SupplierName();
                    _SupplierName.supp_id = dr["supp_id"].ToString();
                    _SupplierName.supp_name = dr["supp_name"].ToString();
                    _SupplierNameList.Add(_SupplierName);
                }
                return _SupplierNameList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult getDetailBySourceDocumentNo(string SourDocumentNo, string SourDocumentDate,string Item_id)
        {
            try
            {
                JsonResult DataRows = null;
                _DeliveryNoteDetail_MODELS = new DeliveryNoteDetail_MODELS();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _DeliveryNoteDetail_IServices.getDetailBySourceDocumentNo(CompID, BrchID, SourDocumentNo, SourDocumentDate, Item_id);

                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult DeliveryNoteDelete(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string command, string title)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string DeliveryNoteNo = _DeliveryNoteDetail_MODELS.dn_no;

                DataSet Message = _DeliveryNoteDetail_IServices.DeliveryNoteDelete(_DeliveryNoteDetail_MODELS, comp_id, br_id, DeliveryNoteNo);

                if (!string.IsNullOrEmpty(DeliveryNoteNo))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string DeliveryNoteNo1 = DeliveryNoteNo.Replace("/", "");
                    other.DeleteTempFile(comp_id + br_id, PageName, DeliveryNoteNo1, Server);
                }
                _DeliveryNoteDetail_MODELS.Message = "Deleted";
                _DeliveryNoteDetail_MODELS.Command = "Refresh";
                _DeliveryNoteDetail_MODELS.dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                _DeliveryNoteDetail_MODELS.TransType = command;
                _DeliveryNoteDetail_MODELS.AppStatus = "D";
                _DeliveryNoteDetail_MODELS.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["DeliveryNoteNo"] = _DeliveryNoteDetail_MODELS.dn_no;
                //Session["TransType"] = command;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult DeliveryNoteApprove(string DN_no, string Dn_date, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_status1)
        {
            try
            {
                URlModelData URLModel = new URlModelData();
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //     DocumentMenuId= Session["MenuDocumentId"].ToString();
                //}
                DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS = new DeliveryNoteDetail_MODELS();
                _DeliveryNoteDetail_MODELS.CreatedBy = Session["UserId"].ToString();
                //_DeliveryNoteDetail_MODELS.CreatedBy = UserID;
                _DeliveryNoteDetail_MODELS.dn_no = DN_no;
                //_DeliveryNoteDetail_MODELS.dn_dt =(Convert.ToDateTime(Dn_date));
                _DeliveryNoteDetail_MODELS.dn_dt = Dn_date;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _DeliveryNoteDetail_IServices.DeliveryNoteApprove(_DeliveryNoteDetail_MODELS, comp_id, br_id, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                try
                {
                    //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "GateEntry_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(DN_no, Dn_date, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(comp_id, br_id, DocumentMenuId, DN_no, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _DeliveryNoteDetail_MODELS.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _DeliveryNoteDetail_MODELS.TransType = "Update";
                _DeliveryNoteDetail_MODELS.Command = "Approve";
                //_DeliveryNoteDetail_MODELS.Message = "Approved";
                _DeliveryNoteDetail_MODELS.Message = _DeliveryNoteDetail_MODELS.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _DeliveryNoteDetail_MODELS.AppStatus = "D";
                _DeliveryNoteDetail_MODELS.BtnName = "BtnEdit";
                TempData["WF_status1"] = WF_status1;
                TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
                URLModel.dn_no = _DeliveryNoteDetail_MODELS.dn_no;
                URLModel.TransType = "Update";
                URLModel.BtnName = "BtnEdit";
                URLModel.Command = "Approve";
                
                // _Common_IServices.SendAlertEmail(comp_id, br_id, DocumentMenuId, DN_no, "AP", UserID, "");
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve"; 
                //Session["DeliveryNoteNo"] = _DeliveryNoteDetail_MODELS.dn_no;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail", URLModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult GetAutoCompleteSupplierName(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS)
        {

            string Spp_Name = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_DeliveryNoteDetail_MODELS.SupplierName))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _DeliveryNoteDetail_MODELS.SupplierName;
                }
                BrchID = Session["BranchId"].ToString();
                SuppList = _DeliveryNoteDetail_IServices.AutoGetSupplierListALl(Comp_ID, Spp_Name, BrchID);

                List<SupplierName> _SupplierNameList = new List<SupplierName>();
                foreach (var dr in SuppList)
                {
                    SupplierName _SupplierName = new SupplierName();
                    _SupplierName.supp_id = dr.Key;
                    _SupplierName.supp_name = dr.Value;
                    _SupplierNameList.Add(_SupplierName);
                }
                _DeliveryNoteDetail_MODELS.SupplierNameList = _SupplierNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetDeliveryNoteSourceDocumentNoList(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string Item_id,string Dn_type)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Spp_ID = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Spp_ID = _DeliveryNoteDetail_MODELS.SupplierID;
                DocumentNumber = _DeliveryNoteDetail_MODELS.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _DeliveryNoteDetail_IServices.getDeliveryNoteSourceDocumentNo(CompID, BrchID, Spp_ID, DocumentNumber, Item_id, Dn_type);
                DataRow dr1;
                dr1 = DocumentNumberList.Tables[0].NewRow();
                dr1[0] = "---Select---";
                dr1[1] = "0";
              
                DocumentNumberList.Tables[0].Rows.InsertAt(dr1, 0);
                foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                {
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.po_no = dr["app_po_no"].ToString();
                    _DocumentNumber.po_dt = dr["po_dt"].ToString();
                   
                    _DocumentNumberList.Add(_DocumentNumber);
                }

                return Json(_DocumentNumberList.Select(c => new { Name = c.po_no, ID = c.po_dt}).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        public List<DocumentNumber> GetDeliveryNoteSourceDocument(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string Item_id, string Dn_type)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Spp_ID = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Spp_ID = _DeliveryNoteDetail_MODELS.SupplierID;
                Spp_ID = _DeliveryNoteDetail_MODELS.supp_id;
                DocumentNumber = _DeliveryNoteDetail_MODELS.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _DeliveryNoteDetail_IServices.getDeliveryNoteSourceDocumentNo(CompID, BrchID, Spp_ID, DocumentNumber,  Item_id,  Dn_type);
                foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                {
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.po_no = dr["app_po_no"].ToString();
                    _DocumentNumber.po_dt = dr["po_dt"].ToString();
                    _DocumentNumberList.Add(_DocumentNumber);
                }


                return _DocumentNumberList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private string getNextDocumentNumber()
        {
            try
            {
                string MenuDocumentId = DocumentMenuId;
                string CompId = Session["CompId"].ToString();
                string BranchId = Session["BranchId"].ToString();
                string Prefix = "DN";
                string NextDocumentNumber = _DeliveryNoteDetail_IServices.getNextDocumentNumber(CompId, BranchId, MenuDocumentId, Prefix);
                return NextDocumentNumber;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            //Session["Message"] = "";
            DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS = new DeliveryNoteDetail_MODELS();
            var a = TrancType.Split(',');
            _DeliveryNoteDetail_MODELS.dn_no = a[0].Trim();
            _DeliveryNoteDetail_MODELS.TransType = a[1].Trim();
            var WF_status1 = a[2].Trim();
            _DeliveryNoteDetail_MODELS.Message = Mailerror;
            //_SPODetailModel.WF_status1 = WF_status1;
            _DeliveryNoteDetail_MODELS.BtnName = "BtnToDetailPage";
            URlModelData URLModel = new URlModelData();
            URLModel.dn_no = a[0].Trim();
            URLModel.TransType = a[1].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("DeliveryNoteDetail", URLModel);
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string Doc_no, string Doc_dt,string Srcdoc_no,string Srcdoc_dt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataTable dt = new DataTable();
                
                if (Status == "D" || Status == "F" || Status == ""||Status == "0")
                {
                    dt = _DeliveryNoteDetail_IServices.GetSubItemDetailsFromPO(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, Srcdoc_no).Tables[0];
                    if (Flag== "DNOrdQty"|| Flag == "DNPendQty" || Flag == "DNAcceptQty" 
                        || Flag == "DNRejctQty" || Flag == "DNRewrkQty" || Flag == "QCShortQty" || Flag == "QCSampleQty")
                    {
                        Flag = "DNAllSubQtyShow";

                    }
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                   
                    int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                       foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("Item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["BilledQty"] = cmn.ConvertToDecimal(item.GetValue("BilledQty").ToString(), DecDgt);
                                dt.Rows[i]["RecievedQty"] = cmn.ConvertToDecimal(item.GetValue("ReceivedQuantity").ToString(), DecDgt);
                                dt.Rows[i]["Order_Qty"] = cmn.ConvertToDecimal(item.GetValue("OrderQty").ToString(), DecDgt);
                                dt.Rows[i]["Pending_Qty"] = cmn.ConvertToDecimal(item.GetValue("PendingQuantity").ToString(), DecDgt);
                                dt.Rows[i]["Accept_Qty"] = cmn.ConvertToDecimal(item.GetValue("AcceptedQty").ToString(), DecDgt);
                                dt.Rows[i]["Reject_Qty"] = cmn.ConvertToDecimal(item.GetValue("RejectedQty").ToString(), DecDgt);
                                dt.Rows[i]["Rework_Qty"] = cmn.ConvertToDecimal(item.GetValue("ReworkableQty").ToString(), DecDgt);
                            }
                        }
                       
                    }
                   
                }

                else
                {
                   dt = _DeliveryNoteDetail_IServices.DN_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag,Srcdoc_no).Tables[0];
                    if (Flag == "DNOrdQty" || Flag == "DNPendQty" || Flag == "DNAcceptQty"
                        || Flag == "DNRejctQty" || Flag == "DNRewrkQty" || Flag == "QCShortQty"|| Flag == "QCSampleQty")
                    {
                        Flag = "DNAllSubQtyShow";

                    }

                }
                ViewBag.src_doc_no = Srcdoc_no;
                ViewBag.src_doc_dt = Convert.ToDateTime(Srcdoc_dt).ToString("yyyy-MM-dd");
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag ,
                    dt_SubItemDetails = dt,
                    _subitemPageName = "DN",
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
                    show_SrcDocNo="Y",
                    show_SrcDocDt="Y"

                };

                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(DeliveryNoteDetail_MODELS _model)
        {
            return File(GetPdfData(_model.dn_no, _model.dn_dt), "application/pdf", "DeliveryNote.pdf");
        }
        public byte[] GetPdfData(string dnNo, string dnDate)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                Byte[] bytes;
                DataSet Details = _DeliveryNoteDetail_IServices.GetDeliveryNoteDeatilsForPrint(CompID, BrchID, dnNo, Convert.ToDateTime(dnDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.Title = "Delivery Note";/*comment and change name of title by Hina shrama on 16-10-2024*/
                ViewBag.Title = "Gate Entry";
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["dn_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNotePrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                }
                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        /*--------------------------For PDF Print End--------------------------*/
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
            return null;
        }
    }

}