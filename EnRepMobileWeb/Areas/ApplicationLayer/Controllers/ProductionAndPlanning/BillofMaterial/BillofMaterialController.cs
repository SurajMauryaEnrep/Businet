using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.BillofMaterial;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.BillofMaterial;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.BillofMaterial
{
    public class BillofMaterialController : Controller
    {
        string DocumentMenuId = "105105115";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, Br_ID, userid, language = String.Empty, title;
        Common_IServices _Common_IServices;
        BillofMaterial_ISERVICES _BillofMaterial_ISERVICES;
        public BillofMaterialController(Common_IServices _Common_IServices, BillofMaterial_ISERVICES _BillofMaterial_ISERVICES)
        {
            this._BillofMaterial_ISERVICES = _BillofMaterial_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/BillofMaterial
        BillofMaterialModel _BOMModel = new BillofMaterialModel();
        public ActionResult BillofMaterial(BillofMaterialModel _BOMModel)
        {//Billof Material List Page
            try
            {
                //BillofMaterialModel _BOMModel = new BillofMaterialModel();
                if (Session["CompId"] != null && Session["BranchId"] != null)
                {
                    CommonPageDetails();
                    CompID = Session["CompId"].ToString();
                    string Br_Id = Session["BranchId"].ToString();
                    //ViewBag.MenuPageName = getDocumentName();
                    _BOMModel.Title = title;
                    List<Status> statusLists = new List<Status>();
                    foreach (DataRow dr in ViewBag.StatusList.Rows)
                    {
                        Status list = new Status();
                        list.status_id = dr["status_code"].ToString();
                        list.status_name = dr["status_name"].ToString();
                        statusLists.Add(list);
                    }
                    _BOMModel.statusLists = statusLists;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        if (TempData["ListFilterData"].ToString() != "")
                        {
                            var PRData = TempData["ListFilterData"].ToString();
                            var a = PRData.Split(',');
                            _BOMModel.Active = a[0].Trim();
                            _BOMModel.Status = a[1].Trim();
                            _BOMModel.ListFilterData = TempData["ListFilterData"].ToString();
                        }
                    }

                    ViewBag.GetBOMList = GetBOMList(_BOMModel);
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_Id, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                //Session["BOMSearch"] = "0";
                _BOMModel.BOMSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterialList.cshtml", _BOMModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchDataBOM(string Act,string Status)
        {
            BillofMaterialModel _BOMModel = new BillofMaterialModel();
            CommonPageDetails();
            string wfstatus = "";
            string CompID = Session["CompId"].ToString();
            string Br_Id = Session["BranchId"].ToString();
            string UserID = Session["UserID"].ToString();
            DataTable SearchData= _BillofMaterial_ISERVICES.GetBOMList(Convert.ToInt32(CompID), Convert.ToInt32(Br_Id), UserID, wfstatus, DocumentMenuId, Act, Status);
            ViewBag.GetBOMList = SearchData;
            ViewBag.SearchData = "SearchDataBOM";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBillofMaterialList.cshtml", _BOMModel);
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, userid, DocumentMenuId, language);
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
        public ActionResult dbClickEdit(string product_id, string rev_no, string WF_status,string ListFilterData)
        {
            try
            {
              
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
                BillofMaterialModel _BOMModel = new BillofMaterialModel();
                UrlModelData UrlData = new UrlModelData();
                if (Session["CompId"] != null)
                {
                    TempData["ListFilterData"] = ListFilterData;
                    _BOMModel.MessageBOM = "";
                    _BOMModel.CommandBOM = "View";
                    _BOMModel.TransTypeBOM = "EditNew";
                    _BOMModel.BtnName = "BtnToDetailPage";
                    _BOMModel.TransTypeBOM = "Update";
                    _BOMModel.product_id = product_id;
                    _BOMModel.rev_no = Convert.ToInt32(rev_no);
                    _BOMModel.SaveUpd = "0";
                    _BOMModel.dbclick = "dbclick";
                    _BOMModel.WF_status1 = WF_status;
                    UrlData.TransTypeBOM = _BOMModel.TransTypeBOM;
                    UrlData.CommandBOM = _BOMModel.CommandBOM;
                    UrlData.BtnName = _BOMModel.BtnName;
                    UrlData.rev_no = _BOMModel.rev_no;
                    UrlData.product_id = _BOMModel.product_id;
                    UrlData.dbclick = "dbclick";
                    TempData["ModelData"] = _BOMModel;
                    //Session["MessageBOM"] = "";
                    //Session["CommandBOM"] = "View";
                    //Session["TransTypeBOM"] = "EditNew";
                    //Session["BtnName"] = "BtnToDetailPage";
                    //Session["TransTypeBOM"] = "Update";                    
                    //Session["product_id"] = product_id;
                    //_BOMModel.product_id = product_id;
                    //Session["rev_no"] = rev_no;
                    //_BOMModel.rev_no =Convert.ToInt32(rev_no);
                    //Session["SaveUpd"] = "0";
                    //Session["dbclick"] = "dbclick";
                    string Comp_ID = Session["CompId"].ToString();
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                return RedirectToAction("AddBillofMaterialDetail", UrlData);
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private DataTable GetBOMList(BillofMaterialModel _BOMModel)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _BOMModel.WF_status = TempData["WF_status"].ToString();
                    if (_BOMModel.WF_status != null)
                    {
                        wfstatus = _BOMModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                else
                {
                    if (_BOMModel.WF_status != null)
                    {
                        wfstatus = _BOMModel.WF_status;
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    string UserID = Session["UserID"].ToString();
                    dt = _BillofMaterial_ISERVICES.GetBOMList(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), UserID, wfstatus, DocumentMenuId, _BOMModel.Active, _BOMModel.Status);
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
        private string todecimalplace(int QtyDigit)
        {
            string dec = "0.";
            for (int i = 0; i < QtyDigit; i++)
            {
                dec += "0";
            }
            return dec;
        }
        public ActionResult AddNewBillofMaterialDetail()
        {
            BillofMaterialModel _BOMModel = new BillofMaterialModel();
            TempData["ListFilterData"] = null;
            _BOMModel.MessageBOM = "New";
            _BOMModel.CommandBOM = "Add";
            _BOMModel.AppStatus = "D";
            _BOMModel.DocumentStatus = "D";
            _BOMModel.bom_status = "D";
            _BOMModel.TransTypeBOM = "Save";
            //Session.Remove("dbclick");
            _BOMModel.BtnName = "BtnAddNew";
            _BOMModel.bom_status = "bom_status";
            TempData["ModelData"] = _BOMModel;
            //Session["MessageBOM"] = "New";
            //Session["CommandBOM"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["DocumentStatus"] = "D";
            //_BOMModel.bom_status = "D";
            //Session["TransTypeBOM"] = "Save";
            //Session.Remove("dbclick");
            //Session["BtnName"] = "BtnAddNew";
            //Session["bom_status"] = "bom_status";
            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("BillofMaterial");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("AddBillofMaterialDetail", "BillofMaterial", _BOMModel);
        }
        public ActionResult AddBillofMaterialDetail(UrlModelData UrlData)//Page Load
        {
            try
            {
                var _BOMModel = TempData["ModelData"] as BillofMaterialModel;
                if (_BOMModel != null)
                {
                    CommonPageDetails();
                    //ViewBag.MenuPageName = getDocumentName();
                    _BOMModel.Title = title;
                    BindOperationNameList(_BOMModel);
                    BindItemNameDDL(_BOMModel, "IR", "Y", "N");
                    _BOMModel.act_status = true;
                    _BOMModel.def_status = false;
                    _BOMModel.qty = "1";
                    BindShopFloorList(_BOMModel);      // Added by Nitesh 26102023 for bind shopfloore
                    BindReplicateWithlist(_BOMModel);  // Added by Nitesh 26102023 for bind replicate with dropdown list
                    if (Session["CompId"] != null && Session["BranchId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                        string Br_Id = Session["BranchId"].ToString();
                        //var other = new CommonController(_Common_IServices);
                        //ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_Id, DocumentMenuId);                       
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _BOMModel.WF_status1 = TempData["WF_status1"].ToString();
                    }  
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _BOMModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["dbclick"] != null && Session["TransTypeBOM"] != null)
                    if (_BOMModel.dbclick != null && _BOMModel.TransTypeBOM != null)
                    {
                        CompID = Session["CompId"].ToString();
                        string Br_Id = Session["BranchId"].ToString();
                        //string Pr_ID = Session["product_id"].ToString();
                        string Pr_ID = _BOMModel.product_id;
                        // For edit
                        string UserID = Session["UserID"].ToString();

                        //_BOMModel.rev_no=Convert.ToInt32(Session["rev_no"].ToString());
                        //_BOMModel.rev_no=Convert.ToInt32(Session["rev_no"].ToString());
                        Int32 rev_no = _BOMModel.rev_no;
                        DataSet ds = new DataSet();
                        ds = _BillofMaterial_ISERVICES.BindBOMdbEdit(Convert.ToInt32(CompID), Convert.ToInt32(Br_Id), Pr_ID, rev_no, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            _BOMModel.product_id = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                            _BOMModel.product_name = Convert.ToString(ds.Tables[0].Rows[0]["item_name"]);
                            _BOMModel.SO_ItemName = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                            _BOMModel.qty = ds.Tables[0].Rows[0]["qty"].ToString();
                            _BOMModel.rev_no = Convert.ToInt32(ds.Tables[0].Rows[0]["rev_no"]);
                            _BOMModel.bom_remarks = Convert.ToString(ds.Tables[0].Rows[0]["bom_remarks"]);
                            string act_status = Convert.ToString(ds.Tables[0].Rows[0]["act_status"]);

                            _BOMModel.create_name = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                            _BOMModel.create_dt = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                            _BOMModel.app_name = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                            _BOMModel.app_dt = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                            _BOMModel.mod_name = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                            _BOMModel.mod_dt = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                            _BOMModel.bom_status_Name = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                            _BOMModel.uom_id = Convert.ToInt32(ds.Tables[0].Rows[0]["uom_id"]);
                            _BOMModel.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);

                            _BOMModel.bom_status = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();
                            _BOMModel.ddl_ShopfloorName = ds.Tables[0].Rows[0]["shfl_id"].ToString().Trim();
                            if (act_status == "Y")
                            {
                                _BOMModel.act_status = true;
                            }
                            else if (act_status == "N")
                            {
                                _BOMModel.act_status = false;
                            }
                            string def_status = Convert.ToString(ds.Tables[0].Rows[0]["def_status"]);
                            //if (Session["TransTypeBOM"] != null)
                            if (_BOMModel.TransTypeBOM != null)
                            {
                                //if (Session["TransTypeBOM"].ToString() == "Revision") {
                                if (_BOMModel.TransTypeBOM == "Revision")
                                {
                                    def_status = "N";
                                }
                                //Session["TransTypeBOM"] = null;
                            }
                            if (def_status == "Y")
                            {
                                _BOMModel.def_status = true;
                            }
                            if (def_status == "N")
                            {
                                _BOMModel.def_status = false;
                            }
                            List<BOM_Item_Detail> ArrItem = new List<BOM_Item_Detail>();
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                BOM_Item_Detail bid = new BOM_Item_Detail();
                                bid.op_product_id = Convert.ToString(ds.Tables[1].Rows[i]["product_id"]);
                                bid.op_product_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                bid.op_rev_no = Convert.ToInt32(ds.Tables[1].Rows[i]["rev_no"]);
                                bid.op_op_id = Convert.ToInt32(ds.Tables[1].Rows[i]["op_id"]);
                                bid.op_op_name = Convert.ToString(ds.Tables[1].Rows[i]["op_name"]);
                                bid.op_Item_type = Convert.ToString(ds.Tables[1].Rows[i]["item_type_id"]);
                                bid.op_Item_type_name = Convert.ToString(ds.Tables[1].Rows[i]["Item_type"]);
                                bid.op_item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                bid.op_item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                bid.op_uom_id = Convert.ToInt32(ds.Tables[1].Rows[i]["uom_id"]);
                                bid.op_uom_name = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                bid.op_qty = Convert.ToString(ds.Tables[1].Rows[i]["qty"]);
                                bid.op_item_cost = Convert.ToString(ds.Tables[1].Rows[i]["item_cost"]);
                                bid.op_item_value = Convert.ToString(ds.Tables[1].Rows[i]["item_value"]);
                                bid.alt_fill = Convert.ToString(ds.Tables[1].Rows[i]["alt_fill"]);
                                bid.op_contain_row = Convert.ToInt32(ds.Tables[1].Rows[i]["containrow"]);
                                bid.seq_no = Convert.ToInt32(ds.Tables[1].Rows[i]["item_type_seq_no"]);
                                ArrItem.Add(bid);
                            }
                            string Statuscode = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();


                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }

                            //if (ViewBag.AppLevel != null && Session["CommandBOM"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _BOMModel.CommandBOM != "Edit")
                            {


                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                //UserID = Session["UserID"].ToString();
                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                _BOMModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                _BOMModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                                _BOMModel.DocDate = ds.Tables[0].Rows[0]["DocDate"].ToString();
                                _BOMModel.create_id = Convert.ToInt32(create_id);
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

                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _BOMModel.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //if(Session["TransTypeBOM"].ToString() == "Revision")
                                        if (_BOMModel.TransTypeBOM == "Revision")
                                        {
                                            //Session["BtnName"] = "BtnAddNew";
                                            _BOMModel.BtnName = "BtnAddNew";
                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    else
                                    {
                                        //if (Session["TransTypeBOM"].ToString() == "Revision")
                                        if (_BOMModel.TransTypeBOM == "Revision")
                                        {
                                            //Session["BtnName"] = "BtnAddNew";
                                            _BOMModel.BtnName = "BtnAddNew";
                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnRefresh";
                                            _BOMModel.BtnName = "BtnRefresh";
                                        }
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            _BOMModel.BOM_Item_Detail_List = ArrItem;
                            // GetAutoCompleteSupplierName(_BOMModel);
                            //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();
                            _BOMModel.DocumentStatus = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();
                            //ViewBag.MenuPageName = getDocumentName();
                            _BOMModel.Title = title;
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.AltenateItemDetail = ds.Tables[5];
                            //CommonPageDetails();
                            //Session.Remove("dbclick");
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel);
                        }
                        //Session.Remove("dbclick");
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        _BOMModel.Title = title;
                        //Session["DocumentStatus"] = "D";
                        _BOMModel.DocumentStatus = "D";
                        //CommonPageDetails();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel);
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _BOMModel.Title = title;
                    //Session["DocumentStatus"] = "D";
                    _BOMModel.DocumentStatus = "D";
                    //CommonPageDetails();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel);
                }
                else
                {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
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
                    BillofMaterialModel _BOMModel1 = new BillofMaterialModel();
                    CommonPageDetails();
                    if (UrlData.TransTypeBOM != null)
                    {
                        _BOMModel1.TransTypeBOM = UrlData.TransTypeBOM;
                    }
                    else
                    {
                        _BOMModel1.TransTypeBOM = "Refresh";
                    }
                    if (UrlData.CommandBOM != null)
                    {
                        _BOMModel1.CommandBOM = UrlData.CommandBOM;
                    }
                    else
                    {
                        _BOMModel1.CommandBOM = "Refresh";
                    }
                    if (UrlData.BtnName != null)
                    {
                        _BOMModel1.BtnName = UrlData.BtnName;
                    }
                    else
                    {
                        _BOMModel1.BtnName = "BtnRefresh";
                    }
                    if (UrlData.product_id != null)
                    {
                        _BOMModel1.product_id = UrlData.product_id;
                    }
                    if (UrlData.rev_no != 0)
                    {
                        _BOMModel1.rev_no = UrlData.rev_no;
                    }
                    if (UrlData.dbclick != null)
                    {
                        _BOMModel1.dbclick = UrlData.dbclick;
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _BOMModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _BOMModel1.Title = title;
                    BindOperationNameList(_BOMModel1);
                    BindItemNameDDL(_BOMModel1, "IR", "Y", "N");
                    _BOMModel1.act_status = true;
                    _BOMModel1.def_status = false;
                    _BOMModel1.qty = "1";
                    if (Session["CompId"] != null && Session["BranchId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                        string Br_Id = Session["BranchId"].ToString();
                        //var other = new CommonController(_Common_IServices);
                        //ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_Id, DocumentMenuId);                       
                    }
                    BindShopFloorList(_BOMModel1); // Added By Nitesh 26-10-2023 for bind  shopfloore dropdownlist
                    BindReplicateWithlist(_BOMModel1); // Added By Nitesh 26-10-2023 for bind  shopfloore dropdownlist
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _BOMModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["dbclick"] != null && Session["TransTypeBOM"] != null)
                    if (_BOMModel1.dbclick != null && _BOMModel1.TransTypeBOM != null)
                    {
                        CompID = Session["CompId"].ToString();
                        string Br_Id = Session["BranchId"].ToString();
                        //string Pr_ID = Session["product_id"].ToString();
                        string Pr_ID = _BOMModel1.product_id;
                        // For edit
                        string UserID = Session["UserID"].ToString();

                        //_BOMModel1.rev_no=Convert.ToInt32(Session["rev_no"].ToString());
                        //_BOMModel1.rev_no=Convert.ToInt32(Session["rev_no"].ToString());
                        Int32 rev_no = _BOMModel1.rev_no;
                        DataSet ds = new DataSet();
                        ds = _BillofMaterial_ISERVICES.BindBOMdbEdit(Convert.ToInt32(CompID), Convert.ToInt32(Br_Id), Pr_ID, rev_no, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            _BOMModel1.product_id = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                            _BOMModel1.product_name = Convert.ToString(ds.Tables[0].Rows[0]["item_name"]);
                            _BOMModel1.SO_ItemName = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                            _BOMModel1.qty = ds.Tables[0].Rows[0]["qty"].ToString();
                            _BOMModel1.rev_no = Convert.ToInt32(ds.Tables[0].Rows[0]["rev_no"]);
                            _BOMModel1.bom_remarks = Convert.ToString(ds.Tables[0].Rows[0]["bom_remarks"]);
                            string act_status = Convert.ToString(ds.Tables[0].Rows[0]["act_status"]);

                            _BOMModel1.create_name = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                            _BOMModel1.create_dt = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                            _BOMModel1.app_name = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                            _BOMModel1.app_dt = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                            _BOMModel1.mod_name = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                            _BOMModel1.mod_dt = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                            _BOMModel1.bom_status_Name = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                            _BOMModel1.uom_id = Convert.ToInt32(ds.Tables[0].Rows[0]["uom_id"]);
                            _BOMModel1.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);

                            _BOMModel1.bom_status = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();

                            _BOMModel1.ddl_ShopfloorName = ds.Tables[0].Rows[0]["shfl_id"].ToString().Trim();
                            if (act_status == "Y")
                            {
                                _BOMModel1.act_status = true;
                            }
                            else if (act_status == "N")
                            {
                                _BOMModel1.act_status = false;
                            }
                            string def_status = Convert.ToString(ds.Tables[0].Rows[0]["def_status"]);
                            //if (Session["TransTypeBOM"] != null)
                            if (_BOMModel1.TransTypeBOM != null)
                            {
                                //if (Session["TransTypeBOM"].ToString() == "Revision") {
                                if (_BOMModel1.TransTypeBOM == "Revision")
                                {
                                    def_status = "N";
                                }
                                //Session["TransTypeBOM"] = null;
                            }
                            if (def_status == "Y")
                            {
                                _BOMModel1.def_status = true;
                            }
                            if (def_status == "N")
                            {
                                _BOMModel1.def_status = false;
                            }
                            List<BOM_Item_Detail> ArrItem = new List<BOM_Item_Detail>();
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                BOM_Item_Detail bid = new BOM_Item_Detail();
                                bid.op_product_id = Convert.ToString(ds.Tables[1].Rows[i]["product_id"]);
                                bid.op_product_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                bid.op_rev_no = Convert.ToInt32(ds.Tables[1].Rows[i]["rev_no"]);
                                bid.op_op_id = Convert.ToInt32(ds.Tables[1].Rows[i]["op_id"]);
                                bid.op_op_name = Convert.ToString(ds.Tables[1].Rows[i]["op_name"]);
                                bid.op_Item_type = Convert.ToString(ds.Tables[1].Rows[i]["item_type_id"]);
                                bid.op_Item_type_name = Convert.ToString(ds.Tables[1].Rows[i]["Item_type"]);
                                bid.op_item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                bid.op_item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                bid.op_uom_id = Convert.ToInt32(ds.Tables[1].Rows[i]["uom_id"]);
                                bid.op_uom_name = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                bid.op_qty = Convert.ToString(ds.Tables[1].Rows[i]["qty"]);
                                bid.op_item_cost = Convert.ToString(ds.Tables[1].Rows[i]["item_cost"]);
                                bid.op_item_value = Convert.ToString(ds.Tables[1].Rows[i]["item_value"]);
                                bid.alt_fill = Convert.ToString(ds.Tables[1].Rows[i]["alt_fill"]);
                                bid.op_contain_row = Convert.ToInt32(ds.Tables[1].Rows[i]["containrow"]);
                                bid.seq_no = Convert.ToInt32(ds.Tables[1].Rows[i]["item_type_seq_no"]);
                                ArrItem.Add(bid);
                            }
                            string Statuscode = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();


                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }

                            //if (ViewBag.AppLevel != null && Session["CommandBOM"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _BOMModel1.CommandBOM != "Edit")
                            {


                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                //UserID = Session["UserID"].ToString();
                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                _BOMModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                _BOMModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                                _BOMModel1.DocDate = ds.Tables[0].Rows[0]["DocDate"].ToString();
                                _BOMModel1.create_id = Convert.ToInt32(create_id);
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

                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _BOMModel1.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _BOMModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //if(Session["TransTypeBOM"].ToString() == "Revision")
                                        if (_BOMModel1.TransTypeBOM == "Revision")
                                        {
                                            //Session["BtnName"] = "BtnAddNew";
                                            _BOMModel1.BtnName = "BtnAddNew";
                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _BOMModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    else
                                    {
                                        //if (Session["TransTypeBOM"].ToString() == "Revision")
                                        if (_BOMModel1.TransTypeBOM == "Revision")
                                        {
                                            //Session["BtnName"] = "BtnAddNew";
                                            _BOMModel1.BtnName = "BtnAddNew";
                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnRefresh";
                                            _BOMModel1.BtnName = "BtnRefresh";
                                        }
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            _BOMModel1.BOM_Item_Detail_List = ArrItem;
                            // GetAutoCompleteSupplierName(_BOMModel1);
                            //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();
                            _BOMModel1.DocumentStatus = ds.Tables[0].Rows[0]["bom_status"].ToString().Trim();
                            //ViewBag.MenuPageName = getDocumentName();
                            _BOMModel1.Title = title;
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.AltenateItemDetail = ds.Tables[5];
                            //CommonPageDetails();
                            //Session.Remove("dbclick");
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel1);
                        }
                        //Session.Remove("dbclick");
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        _BOMModel1.Title = title;
                        //Session["DocumentStatus"] = "D";
                        _BOMModel1.DocumentStatus = "D";
                        //CommonPageDetails();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel1);
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _BOMModel1.Title = title;
                    //Session["DocumentStatus"] = "D";
                    _BOMModel1.DocumentStatus = "D";
                    //CommonPageDetails();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterial.cshtml", _BOMModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BillofMaterialSave(BillofMaterialModel _BOMModel, string command)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    _BOMModel.comp_id = Convert.ToInt32(Session["compid"].ToString());
                    _BOMModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    _BOMModel.create_id = Convert.ToInt32(Session["UserId"].ToString());
                    string product_id = _BOMModel.product_id;
                    Int32 uom_id = _BOMModel.uom_id;
                    Int32 rev_no = _BOMModel.rev_no;
                    string bom_remarks = _BOMModel.bom_remarks;
                    Boolean act_status = _BOMModel.act_status;

                    Boolean def_status = _BOMModel.def_status;
                    Int32 create_id = _BOMModel.create_id;
                    Int32 app_id = _BOMModel.app_id;
                    string bom_status = _BOMModel.bom_status;
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    _BOMModel.mac_id = SystemDetail;// Session["UserMacaddress"].ToString();
                    string mac_id = _BOMModel.mac_id;
                    DataTable AlternateItemDt = new DataTable();

                    string action = "";
                    if (_BOMModel.DeleteCommand == "Delete")
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
                                Br_ID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                return RedirectToAction("dbClickEdit", new { product_id = _BOMModel.product_id, rev_no = _BOMModel.rev_no, WF_status = _BOMModel.WFStatus });
                            }
                           /*End to chk Financial year exist or not*/
                           UrlModelData UrlData = new UrlModelData();
                            _BOMModel.MessageBOM = "";
                            _BOMModel.CommandBOM = "Edit";
                            _BOMModel.BtnName = "BtnAddNew";
                            _BOMModel.TransTypeBOM = "Update";
                            _BOMModel.dbclick = "dbclick";
                            UrlData.TransTypeBOM = _BOMModel.TransTypeBOM;
                            UrlData.CommandBOM = _BOMModel.CommandBOM;
                            UrlData.BtnName = _BOMModel.BtnName;
                            UrlData.product_id = _BOMModel.product_id;
                            UrlData.rev_no = _BOMModel.rev_no;
                            UrlData.dbclick = "dbclick";
                            TempData["ModelData"] = _BOMModel;
                            //Session["MessageBOM"] = "";
                            //Session["CommandBOM"] = "Edit";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["TransTypeBOM"] = "Update";
                            //Session["dbclick"] = "dbclick";
                            return RedirectToAction("AddBillofMaterialDetail", UrlData);

                        case "Revision":
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                Br_ID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                return RedirectToAction("dbClickEdit", new { product_id = _BOMModel.product_id, rev_no = _BOMModel.rev_no, WF_status = _BOMModel.WFStatus });
                            }
                            /*End to chk Financial year exist or not*/
                            UrlModelData UrlDataRevision = new UrlModelData();
                            _BOMModel.MessageBOM = "";
                            _BOMModel.CommandBOM = "Revision";
                            _BOMModel.BtnName = "BtnAddNew";// "btn_rev";
                            _BOMModel.TransTypeBOM = "Revision";
                            _BOMModel.dbclick = "dbclick";
                            UrlDataRevision.TransTypeBOM = _BOMModel.TransTypeBOM;
                            UrlDataRevision.CommandBOM = _BOMModel.CommandBOM;
                            UrlDataRevision.BtnName = _BOMModel.BtnName;
                            UrlDataRevision.product_id = _BOMModel.product_id;
                            UrlDataRevision.rev_no = _BOMModel.rev_no;
                            TempData["ModelData"] = _BOMModel;
                            TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                            return RedirectToAction("AddBillofMaterialDetail", UrlDataRevision);

                        case "AddNew":
                            BillofMaterialModel _BOMModelAddNew = new BillofMaterialModel();
                            _BOMModelAddNew.MessageBOM = "";
                            _BOMModelAddNew.CommandBOM = "Add";
                            _BOMModelAddNew.TransTypeBOM = "Save";
                            _BOMModelAddNew.BtnName = "BtnAddNew";
                            _BOMModelAddNew.bom_status = "D";
                            _BOMModelAddNew.bom_status = "bom_status";
                            TempData["ListFilterData"] = null;
                            TempData["ModelData"] = _BOMModelAddNew;
                            //Session["MessageBOM"] = "";
                            //Session["CommandBOM"] = "Add";
                            //Session["TransTypeBOM"] = "Save";
                            //Session["BtnName"] = "BtnAddNew";
                            //_BOMModel.bom_status = "D";
                            //Session["bom_status"] = "bom_status";
                            //Session.Remove("dbclick");
                            //Session.Remove("SaveUpd");
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                Br_ID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                if (!string.IsNullOrEmpty(_BOMModel.product_id))
                                    return RedirectToAction("dbClickEdit", new { product_id = _BOMModel.product_id, rev_no = _BOMModel.rev_no, WF_status = _BOMModel.WFStatus });
                                else
                                    _BOMModelAddNew.CommandBOM = "Refresh";
                                _BOMModelAddNew.TransTypeBOM = "Refresh";
                                _BOMModelAddNew.BtnName = "Refresh";
                                _BOMModelAddNew.DocumentStatus = null;
                                TempData["ListFilterData"] = null;
                                TempData["ModelData"] = _BOMModelAddNew;
                                return RedirectToAction("AddBillofMaterialDetail", _BOMModelAddNew);
                            }
                            /*End to chk Financial year exist or not*/
                            return RedirectToAction("AddBillofMaterialDetail");

                        case "Approve":
                            //Session["CommandBOM"] = command;
                            //Session["TransTypeBOM"] = command;
                            UrlModelData UrlDataApprove = new UrlModelData();
                            _BOMModel.CommandBOM = command;
                            TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                            _BOMModel.TransTypeBOM = command;
                            int brid = Convert.ToInt32(Session["BranchId"].ToString());
                            BillOfMeterialApprove(_BOMModel);//_BillofMaterial_ISERVICES.insertBOMDetail(dtBOMDetail2, dtBomItemDetail2);
                            UrlDataApprove.TransTypeBOM = _BOMModel.TransTypeBOM;
                            UrlDataApprove.CommandBOM = _BOMModel.CommandBOM;
                            UrlDataApprove.BtnName = _BOMModel.BtnName;
                            UrlDataApprove.product_id = _BOMModel.product_id;
                            UrlDataApprove.rev_no = _BOMModel.rev_no;
                            UrlDataApprove.dbclick = _BOMModel.dbclick;
                            TempData["ModelData"] = _BOMModel;
                            return RedirectToAction("AddBillofMaterialDetail", UrlDataApprove);

                        case "Delete":
                            BillofMaterialModel _BOMModelDelete = new BillofMaterialModel();
                            //Session["CommandBOM"] = command;
                            //Session["TransTypeBOM"] = "Delete";
                            TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                            _BOMModel.CommandBOM = command;
                            _BOMModel.TransTypeBOM = "Delete";
                            //var Trtype1 = Session["TransTypeBOM"].ToString();
                            //var Trtype = Session["TransTypeBOM"].ToString();
                            var Trtype1 = _BOMModel.TransTypeBOM;
                            var Trtype = _BOMModel.TransTypeBOM;
                            _BOMModel.TransType = Trtype;
                            DataTable dtBOMDetail1 = new DataTable();
                            dtBOMDetail1.Columns.Add("comp_id", typeof(Int32));
                            dtBOMDetail1.Columns.Add("br_id", typeof(Int32));
                            dtBOMDetail1.Columns.Add("product_id", typeof(string));
                            dtBOMDetail1.Columns.Add("uom_id", typeof(string));
                            dtBOMDetail1.Columns.Add("qty", typeof(int));
                            dtBOMDetail1.Columns.Add("rev_no", typeof(Int32));
                            dtBOMDetail1.Columns.Add("bom_remarks", typeof(string));
                            dtBOMDetail1.Columns.Add("act_status", typeof(string));
                            dtBOMDetail1.Columns.Add("def_status", typeof(string));
                            dtBOMDetail1.Columns.Add("create_id", typeof(Int32));
                            dtBOMDetail1.Columns.Add("bom_status", typeof(string));
                            dtBOMDetail1.Columns.Add("mac_id", typeof(string));
                            dtBOMDetail1.Columns.Add("TransType", typeof(string));
                            dtBOMDetail1.Columns.Add("shfl_id", typeof(string));
                            DataRow drBOMDetail1 = dtBOMDetail1.NewRow();

                            drBOMDetail1["comp_id"] = _BOMModel.comp_id;
                            drBOMDetail1["br_id"] = _BOMModel.br_id;
                            drBOMDetail1["product_id"] = _BOMModel.product_id;
                            drBOMDetail1["uom_id"] = _BOMModel.uom_id;
                            drBOMDetail1["qty"] = IsNull(_BOMModel.qty, "0");
                            drBOMDetail1["rev_no"] = _BOMModel.rev_no;

                            drBOMDetail1["bom_remarks"] = _BOMModel.bom_remarks;
                            drBOMDetail1["act_status"] = "Y";
                            drBOMDetail1["def_status"] = "Y";
                            //drBOMDetail["def_status"] = _BOMModel.def_status;
                            drBOMDetail1["create_id"] = _BOMModel.create_id;
                            drBOMDetail1["bom_status"] = _BOMModel.bom_status;
                            drBOMDetail1["mac_id"] = _BOMModel.mac_id;

                            drBOMDetail1["TransType"] = _BOMModel.TransType;
                            if (Trtype == "Update")
                            {
                                drBOMDetail1["product_id"] = _BOMModel.product_id;
                            }
                            drBOMDetail1["shfl_id"] = _BOMModel.ddl_ShopfloorName;
                            dtBOMDetail1.Rows.Add(drBOMDetail1);


                            DataTable dtBomItemDetail1 = new DataTable();

                            dtBomItemDetail1.Columns.Add("product_id", typeof(string));
                            dtBomItemDetail1.Columns.Add("rev_no", typeof(Int32));
                            dtBomItemDetail1.Columns.Add("op_id", typeof(float));
                            dtBomItemDetail1.Columns.Add("Item_type", typeof(char));
                            dtBomItemDetail1.Columns.Add("item_id", typeof(string));
                            dtBomItemDetail1.Columns.Add("uom_id", typeof(Int32));
                            dtBomItemDetail1.Columns.Add("qty", typeof(float));
                            dtBomItemDetail1.Columns.Add("item_cost", typeof(float));
                            dtBomItemDetail1.Columns.Add("item_value", typeof(float));
                            dtBomItemDetail1.Columns.Add("seq_no", typeof(string));
                            brid = Convert.ToInt32(Session["BranchId"].ToString());

                            AlternateItemDt = DtAltItemDetail(_BOMModel.hdnBomAltItemDetail);

                            var result = _BillofMaterial_ISERVICES.insertBOMDetail(dtBOMDetail1, dtBomItemDetail1, AlternateItemDt, "", "", "", DocumentMenuId);
                            string Message = result.Item1;
                            string[] splitmsg = Message.Split('-');
                            if (splitmsg[0].ToString().Trim() == "Delete")
                            {
                                _BOMModelDelete.MessageBOM = "Deleted";
                                _BOMModelDelete.CommandBOM = "Refresh";
                                _BOMModelDelete.TransTypeBOM = "Refresh";
                                _BOMModelDelete.BtnName = "BtnDelete";
                                ViewBag.Message = _BOMModelDelete.MessageBOM;
                                TempData["ModelData"] = _BOMModelDelete;
                                //Session["MessageBOM"] = "Deleted";
                                //Session["CommandBOM"] = "Refresh";
                                //Session["TransTypeBOM"] = "Refresh";
                                //Session["BtnName"] = "BtnDelete";
                                //Session.Remove("dbclick");
                                //ViewBag.Message = Session["MessageBOM"].ToString();

                                //_BOMModel = null;
                                return RedirectToAction("AddBillofMaterialDetail");
                            }
                            else
                            {
                                TempData["ModelData"] = _BOMModel;
                                return RedirectToAction("AddBillofMaterialDetail");
                            }
                        case "Save":
                            //Session["CommandBOM"] = command;
                            UrlModelData UrlDataSave = new UrlModelData();
                            _BOMModel.CommandBOM = command;
                            //Trtype = Session["TransTypeBOM"].ToString();
                            if (_BOMModel.TransTypeBOM == null)
                            {
                                _BOMModel.TransTypeBOM = command;
                            }
                            Trtype = _BOMModel.TransTypeBOM;
                            //if (ModelState.IsValid)
                            //{
                            // _BOMModel.bom_status = 'D';
                            _BOMModel.TransType = Trtype;
                            DataTable dtBOMDetail = new DataTable();
                            dtBOMDetail.Columns.Add("comp_id", typeof(Int32));
                            dtBOMDetail.Columns.Add("br_id", typeof(Int32));
                            dtBOMDetail.Columns.Add("product_id", typeof(string));
                            dtBOMDetail.Columns.Add("uom_id", typeof(string));
                            dtBOMDetail.Columns.Add("qty", typeof(int));
                            dtBOMDetail.Columns.Add("rev_no", typeof(Int32));
                            dtBOMDetail.Columns.Add("bom_remarks", typeof(string));
                            dtBOMDetail.Columns.Add("act_status", typeof(string));
                            dtBOMDetail.Columns.Add("def_status", typeof(string));
                            dtBOMDetail.Columns.Add("create_id", typeof(Int32));
                            dtBOMDetail.Columns.Add("bom_status", typeof(string));
                            dtBOMDetail.Columns.Add("mac_id", typeof(string));
                            dtBOMDetail.Columns.Add("TransType", typeof(string));
                            dtBOMDetail.Columns.Add("shfl_id", typeof(string));


                            DataRow drBOMDetail = dtBOMDetail.NewRow();

                            drBOMDetail["comp_id"] = _BOMModel.comp_id;
                            drBOMDetail["br_id"] = _BOMModel.br_id;
                            drBOMDetail["product_id"] = _BOMModel.product_id;
                            drBOMDetail["uom_id"] = _BOMModel.uom_id;
                            drBOMDetail["qty"] = IsNull(_BOMModel.qty, "0");
                            drBOMDetail["rev_no"] = _BOMModel.rev_no;

                            drBOMDetail["bom_remarks"] = _BOMModel.bom_remarks;
                            //if (Session["bom_status"] != null)
                            //if (_BOMModel.bom_status != null)
                            //{
                            //    //if (Session["bom_status"].ToString() == "bom_status")
                            //    if (_BOMModel.bom_status == "bom_status")
                            //    {
                            //        _BOMModel.act_status = true;
                            //        _BOMModel.def_status = true;
                            //        _BOMModel.bom_status = "D";
                            //        //Session.Remove("bom_status");
                            //    }
                            //}
                            //if (Session["DocumentStatus"].ToString() == "D")
                            if (_BOMModel.bom_status != null)
                            {
                                if (_BOMModel.bom_status == "bom_status" || Trtype == "Save")
                                {
                                    _BOMModel.bom_status = "D";
                                }
                            }
                            if (Trtype == "Save")
                            {
                                _BOMModel.bom_status = "D";
                            }
                            if (_BOMModel.DocumentStatus == "D")
                            {
                                drBOMDetail["act_status"] = "Y";
                                _BOMModel.def_status = false;
                            }
                            else
                            {
                                if (_BOMModel.act_status == true)
                                {
                                    drBOMDetail["act_status"] = "Y";
                                }
                                if (_BOMModel.act_status == false)
                                {
                                    drBOMDetail["act_status"] = "N";
                                }
                            }
                            if (_BOMModel.def_status == true)
                            {
                                drBOMDetail["def_status"] = "Y";
                            }
                            if (_BOMModel.def_status == false)
                            {
                                drBOMDetail["def_status"] = "N";
                            }
                            drBOMDetail["create_id"] = _BOMModel.create_id;
                            //drBOMDetail["bom_status"] = _BOMModel.bom_status;
                            drBOMDetail["bom_status"] = _BOMModel.bom_status;
                            drBOMDetail["mac_id"] = _BOMModel.mac_id;

                            drBOMDetail["TransType"] = _BOMModel.TransType;
                            if (Trtype == "Update")
                            {
                                drBOMDetail["product_id"] = _BOMModel.product_id;
                            }
                          
                            drBOMDetail["shfl_id"] = _BOMModel.ddl_ShopfloorName; //added By NItesh 26-10-2023 for shopfloore 
                            dtBOMDetail.Rows.Add(drBOMDetail);

                            DataTable dtBomItemDetail = new DataTable();

                            dtBomItemDetail.Columns.Add("product_id", typeof(string));
                            dtBomItemDetail.Columns.Add("rev_no", typeof(Int32));
                            dtBomItemDetail.Columns.Add("op_id", typeof(float));
                            dtBomItemDetail.Columns.Add("Item_type", typeof(string));
                            dtBomItemDetail.Columns.Add("item_id", typeof(string));
                            dtBomItemDetail.Columns.Add("uom_id", typeof(int));
                            dtBomItemDetail.Columns.Add("qty", typeof(float));
                            dtBomItemDetail.Columns.Add("item_cost", typeof(float));
                            dtBomItemDetail.Columns.Add("item_value", typeof(float));
                            dtBomItemDetail.Columns.Add("seq_no", typeof(string));

                            JArray jObject = JArray.Parse(_BOMModel.bomitemattr);
                            for (int i = 0; i < jObject.Count; i++)
                            {
                                DataRow drBomItemDetail = dtBomItemDetail.NewRow();

                                drBomItemDetail["product_id"] = jObject[i]["op_product_id"];
                                drBomItemDetail["rev_no"] = jObject[i]["op_rev_no"];
                                drBomItemDetail["op_id"] = jObject[i]["op_op_id"];
                                drBomItemDetail["Item_type"] = jObject[i]["op_Item_type"];
                                drBomItemDetail["item_id"] = jObject[i]["op_item_id"];
                                drBomItemDetail["uom_id"] = jObject[i]["op_uom_id"];
                                drBomItemDetail["qty"] = jObject[i]["op_qty"];
                                drBomItemDetail["item_cost"] = jObject[i]["op_item_cost"];
                                drBomItemDetail["item_value"] = jObject[i]["op_item_value"];
                                drBomItemDetail["seq_no"] = jObject[i]["seq_no"];

                                dtBomItemDetail.Rows.Add(drBomItemDetail);
                            }

                            AlternateItemDt = DtAltItemDetail(_BOMModel.hdnBomAltItemDetail);

                            var result1 = _BillofMaterial_ISERVICES.insertBOMDetail(dtBOMDetail, dtBomItemDetail, AlternateItemDt, "", "", "", "");
                            Message = result1.Item1;
                            splitmsg = Message.Split('-');
                            if(splitmsg[0].ToString().Trim() == "CanNotActive")//Added by Suraj Maurya on 10-01-2025
                            {
                                _BOMModel.MessageBOM = "CanNotActive";
                                _BOMModel.UsedInProducts = result1.Item2;
                                _BOMModel.CommandBOM = "EditNew";
                                _BOMModel.TransTypeBOM = "Update";
                                _BOMModel.BtnName = "BtnSave";
                                ViewBag.Message = _BOMModel.MessageBOM;
                                _BOMModel.SaveUpd = "AfterSaveUpdate";
                                Session["br_id"] = Session["BranchId"].ToString();
                                _BOMModel.dbclick = "dbclick";
                                _BOMModel.product_id = splitmsg[1].ToString().Trim();
                                _BOMModel.rev_no = Convert.ToInt32(splitmsg[2].ToString().Trim());

                                UrlDataSave.TransTypeBOM = _BOMModel.TransTypeBOM;
                                UrlDataSave.CommandBOM = _BOMModel.CommandBOM;
                                UrlDataSave.BtnName = _BOMModel.BtnName;
                                UrlDataSave.product_id = _BOMModel.product_id;
                                UrlDataSave.rev_no = _BOMModel.rev_no;
                                UrlDataSave.dbclick = "dbclick";
                                TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                                TempData["ModelData"] = _BOMModel;

                                return RedirectToAction("AddBillofMaterialDetail", UrlDataSave);
                            }
                            else if (splitmsg[0].ToString().Trim() == "Update" || splitmsg[0].ToString().Trim() == "Save" 
                                || splitmsg[0].ToString().Trim() == "Revision")
                            {
                                _BOMModel.MessageBOM = "Save";
                                _BOMModel.CommandBOM = "EditNew";
                                _BOMModel.TransTypeBOM = "Update";
                                _BOMModel.BtnName = "BtnSave";
                                ViewBag.Message = _BOMModel.MessageBOM;
                                _BOMModel.SaveUpd = "AfterSaveUpdate";
                                Session["br_id"] = Session["BranchId"].ToString();
                                _BOMModel.dbclick = "dbclick";
                                _BOMModel.product_id = splitmsg[1].ToString().Trim();
                                _BOMModel.rev_no = Convert.ToInt32(splitmsg[2].ToString().Trim());

                                UrlDataSave.TransTypeBOM = _BOMModel.TransTypeBOM;
                                UrlDataSave.CommandBOM = _BOMModel.CommandBOM;
                                UrlDataSave.BtnName = _BOMModel.BtnName;
                                UrlDataSave.product_id = _BOMModel.product_id;
                                UrlDataSave.rev_no = _BOMModel.rev_no;
                                UrlDataSave.dbclick = "dbclick";
                                TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                                TempData["ModelData"] = _BOMModel;
                                //Session["product_id"] = splitmsg[1].ToString().Trim();
                                //Session["rev_no"] = splitmsg[2].ToString().Trim();

                                //Session["MessageBOM"] = "Save";
                                //Session["CommandBOM"] = "EditNew";
                                //Session["TransTypeBOM"] = "Update";
                                //Session["BtnName"] = "BtnSave";
                                //ViewBag.Message = Session["MessageBOM"].ToString();
                                //Session["SaveUpd"] = "AfterSaveUpdate";
                                //Session["br_id"] = Session["BranchId"].ToString();
                                //_BOMModel.product_id = splitmsg[1].ToString().Trim();
                                //Session["product_id"] = splitmsg[1].ToString().Trim();
                                //_BOMModel.rev_no=Convert.ToInt32( splitmsg[2].ToString().Trim());
                                //Session["rev_no"]= splitmsg[2].ToString().Trim();
                                //Session["dbclick"] = "dbclick";
                                return RedirectToAction("AddBillofMaterialDetail", UrlDataSave);
                            }
                            else if (splitmsg[0].ToString().Trim() == "Duplicate")
                            {
                                //Session["MessageBOM"] = "Duplicate";
                                TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                                _BOMModel.MessageBOM = "Duplicate";
                                //ViewBag.Message = Session["MessageBOM"].ToString();
                                ViewBag.Message = _BOMModel.MessageBOM;
                                TempData["ModelData"] = _BOMModel;
                            }
                            else if (splitmsg[0].ToString().Trim() == "Data_Not_Found")
                            {
                                TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                                //var a = SaveMessage.Split(',');
                                ViewBag.MenuPageName = getDocumentName();
                                _BOMModel.Title = title;
                                var msg = splitmsg[0].ToString().Trim().Replace("_", " ") + " " + splitmsg[1].ToString().Trim() + " in " + _BOMModel.Title;//ContraNo is use as table type
                                    string path = Server.MapPath("~");
                                    Errorlog.LogError_customsg(path, msg, "", "");
                                _BOMModel.MessageBOM = splitmsg[0].ToString().Trim().Replace("_", "");
                                return View("~/Views/Shared/Error.cshtml");

                            }
                            return RedirectToAction("AddBillofMaterialDetail");
                        case "Refresh":
                            BillofMaterialModel _BOMModelRefresh = new BillofMaterialModel();
                            _BOMModelRefresh.BtnName = "BtnRefresh";
                            _BOMModelRefresh.CommandBOM = command;
                            _BOMModelRefresh.TransTypeBOM = "Refresh";
                            TempData["ModelData"] = _BOMModelRefresh;
                            TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                            //Session["BtnName"] = "BtnRefresh";
                            //Session["CommandBOM"] = command;
                            //Session["TransTypeBOM"] = "Refresh";
                            //Session["MessageBOM"] = "";
                            //Session["AppStatus"] = "";
                            //Session.Remove("dbclick");
                            //Session.Remove("SaveUpd");
                            //Session.Remove("bom_status");
                            //_BOMModel = null;
                            return RedirectToAction("AddBillofMaterialDetail");
                        case "Print":

                            return GenratePdfFile(_BOMModel);
                        case "BacktoList":
                            //Session.Remove("Message");
                            //Session.Remove("TransType");
                            //Session.Remove("Command");
                            //Session.Remove("BtnName");
                            //Session.Remove("DocumentStatus");
                            //Session.Remove("bom_status");
                            //Session.Remove("dbclick");
                            //_BOMModel = null;
                            TempData["ListFilterData"] = _BOMModel.ListFilterData1;
                            TempData["WF_status"] = _BOMModel.WF_status1;
                            return RedirectToAction("BillofMaterial", "BillofMaterial");
                    }
                }
                else
                {
                    RedirectToAction("");
                }
                return RedirectToAction("AddBillofMaterialDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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

        private DataTable DtAltItemDetail(string AltItemDetail)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("op_id",typeof(string));
            dt.Columns.Add("item_id",typeof(string));
            dt.Columns.Add("alt_item_id",typeof(string));
            dt.Columns.Add("uom_id", typeof(string));
            dt.Columns.Add("qty", typeof(string));
            dt.Columns.Add("item_cost", typeof(string));

            if (AltItemDetail != null)
            {
                JArray jObject = JArray.Parse(AltItemDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow drBomItemDetail = dt.NewRow();

                    drBomItemDetail["op_id"] = jObject[i]["op_id"];
                    drBomItemDetail["item_id"] = jObject[i]["item_id"];
                    drBomItemDetail["alt_item_id"] = jObject[i]["alt_item_id"];
                    drBomItemDetail["uom_id"] = jObject[i]["uom_id"];
                    drBomItemDetail["qty"] = jObject[i]["qty"];
                    drBomItemDetail["item_cost"] = jObject[i]["item_cost"];

                    dt.Rows.Add(drBomItemDetail);
                }
            }
            return dt;
        }
        private ActionResult BillOfMeterialApprove(BillofMaterialModel _BOMModel)
        {
            try
            {
                var userid = Session["UserID"].ToString();
                //var Trtype2 = Session["TransTypeBOM"].ToString();
                if (_BOMModel.TransTypeBOM == null)
                {
                    _BOMModel.TransTypeBOM = "Approve";
                }
                var Trtype2 = _BOMModel.TransTypeBOM;
                _BOMModel.TransType = "Approve";
                DataTable dtBOMDetail2 = new DataTable();
                dtBOMDetail2.Columns.Add("comp_id", typeof(Int32));
                dtBOMDetail2.Columns.Add("br_id", typeof(Int32));
                dtBOMDetail2.Columns.Add("product_id", typeof(string));
                dtBOMDetail2.Columns.Add("uom_id", typeof(string));
                dtBOMDetail2.Columns.Add("qty", typeof(int));
                dtBOMDetail2.Columns.Add("rev_no", typeof(Int32));
                dtBOMDetail2.Columns.Add("bom_remarks", typeof(string));
                dtBOMDetail2.Columns.Add("act_status", typeof(string));
                dtBOMDetail2.Columns.Add("def_status", typeof(string));
                dtBOMDetail2.Columns.Add("create_id", typeof(Int32));
                dtBOMDetail2.Columns.Add("bom_status", typeof(string));
                dtBOMDetail2.Columns.Add("mac_id", typeof(string));
                dtBOMDetail2.Columns.Add("TransType", typeof(string));
                dtBOMDetail2.Columns.Add("shfl_id", typeof(string));
                DataRow drBOMDetail2 = dtBOMDetail2.NewRow();

                drBOMDetail2["comp_id"] = Session["CompId"].ToString();
                drBOMDetail2["br_id"] = Session["BranchId"].ToString();
                drBOMDetail2["product_id"] = _BOMModel.product_id;
                drBOMDetail2["uom_id"] = _BOMModel.uom_id;
                drBOMDetail2["qty"] = IsNull(_BOMModel.qty, "0");
                drBOMDetail2["rev_no"] = _BOMModel.rev_no;
                drBOMDetail2["bom_remarks"] = _BOMModel.bom_remarks;
                drBOMDetail2["act_status"] = "Y";
                drBOMDetail2["def_status"] = "Y";
                drBOMDetail2["create_id"] = Session["UserID"].ToString();
                drBOMDetail2["bom_status"] = IsNull(_BOMModel.bom_status, "");
                drBOMDetail2["shfl_id"] = _BOMModel.ddl_ShopfloorName;
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();

                drBOMDetail2["mac_id"] = SystemDetail;

                drBOMDetail2["TransType"] = _BOMModel.TransType;
                if (Trtype2 == "Approve")
                {
                    drBOMDetail2["product_id"] = _BOMModel.product_id;
                }
                dtBOMDetail2.Rows.Add(drBOMDetail2);

                DataTable dtBomItemDetail2 = new DataTable();

                dtBomItemDetail2.Columns.Add("product_id", typeof(string));
                dtBomItemDetail2.Columns.Add("rev_no", typeof(Int32));
                dtBomItemDetail2.Columns.Add("op_id", typeof(float));
                dtBomItemDetail2.Columns.Add("Item_type", typeof(char));
                dtBomItemDetail2.Columns.Add("item_id", typeof(string));
                dtBomItemDetail2.Columns.Add("uom_id", typeof(Int32));
                dtBomItemDetail2.Columns.Add("qty", typeof(float));
                dtBomItemDetail2.Columns.Add("item_cost", typeof(float));
                dtBomItemDetail2.Columns.Add("item_value", typeof(float));
                dtBomItemDetail2.Columns.Add("seq_no", typeof(string));
                int brid = Convert.ToInt32(Session["BranchId"].ToString());

                string A_Status = _BOMModel.A_Status;
                string A_Level = _BOMModel.A_Level;
                string A_Remarks = _BOMModel.A_Remarks;
                DataTable AlternateItemDt = DtAltItemDetail(_BOMModel.hdnBomAltItemDetail);

                var result = _BillofMaterial_ISERVICES.insertBOMDetail(dtBOMDetail2, dtBomItemDetail2, AlternateItemDt, A_Status, A_Level, A_Remarks, DocumentMenuId);
                string Message = result.Item1;
                string[] splitmsg = Message.Split('-');
                if (splitmsg[0].ToString().Trim() == "Approve")
                {
                    try
                    {
                        //string fileName = "BOM_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "BillOfMaterial_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_BOMModel.product_id, _BOMModel.rev_no.ToString(), fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, _BOMModel.product_id, "AP", userid, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _BOMModel.MessageBOM = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _BOMModel.MessageBOM = _BOMModel.MessageBOM == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    //_BOMModel.MessageBOM = "Approved";
                    _BOMModel.CommandBOM = "EditNew";
                    _BOMModel.TransTypeBOM = "Approve";
                    _BOMModel.BtnName = "BtnToDetailPage";
                    ViewBag.Message = _BOMModel.MessageBOM;
                    _BOMModel.SaveUpd = "AfterSaveUpdate";
                    Session["br_id"] = Session["BranchId"].ToString();
                    _BOMModel.product_id = splitmsg[1].ToString().Trim();
                    _BOMModel.dbclick = "dbclick";

                    //Session["MessageBOM"] = "Approved";
                    //Session["CommandBOM"] = "EditNew";
                    //Session["TransTypeBOM"] = "Approve";
                    //Session["BtnName"] = "BtnToDetailPage";
                    //ViewBag.Message = Session["MessageBOM"].ToString();
                    //Session["SaveUpd"] = "AfterSaveUpdate";
                    //Session["br_id"] = Session["BranchId"].ToString();
                    //_BOMModel.product_id = splitmsg[1].ToString().Trim();
                    //Session["product_id"] = splitmsg[1].ToString().Trim();
                    //Session["dbclick"] = "dbclick";
                }
                else if (splitmsg[0].ToString().Trim() == "Duplicate")
                {
                    _BOMModel.MessageBOM = "Duplicate";
                    ViewBag.Message = _BOMModel.MessageBOM;
                    //Session["MessageBOM"] = "Duplicate";
                    //ViewBag.Message = Session["MessageBOM"].ToString();
                }
                return RedirectToAction("AddBillofMaterialDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        //[HttpPost]
        public ActionResult GetProductNameInDDL(BillofMaterialModel _BOMModel)
        {
            JsonResult DataRows = null;
            string SOItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_BOMModel.SO_ItemName))
                    {
                        SOItmName = "0";
                    }
                    else
                    {
                        SOItmName = _BOMModel.SO_ItemName;
                    }
                    DataSet ProductList = _BillofMaterial_ISERVICES.BindProductNameInDDL(Comp_ID, Br_ID, SOItmName);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        //ItemList.Add("0" + "_" + "H1", "Heading");
                        ItemList.Add("0" + "_" + "0", "---Select---");
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string itemId = ProductList.Tables[0].Rows[i]["Item_id"].ToString();
                            string itemName = ProductList.Tables[0].Rows[i]["Item_name"].ToString();
                            string Uom = ProductList.Tables[0].Rows[i]["uom_name"].ToString();
                            ItemList.Add(itemId + "_" + Uom, itemName);
                        }
                    }
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult BindItemNameDDL(BillofMaterialModel _BOMModel, string ItmType, string wip, string Pack)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            DataSet itemList = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_BOMModel.SO_ItemName))
                    {
                        _BOMModel.SO_ItemName = "0";
                    }

                    itemList = _BillofMaterial_ISERVICES.GeItemNameList(Comp_ID, Br_ID, ItmType, wip, Pack, _BOMModel.SO_ItemName,_BOMModel.product_id);
                    //if (itemList.Tables[0].Rows.Count > 0)
                    //{
                    //    //ItemList.Add("0" + "_" + "H1", "Heading");
                    //    for (int i = 0; i < itemList.Tables[0].Rows.Count; i++)
                    //    {
                    //        string itemId = itemList.Tables[0].Rows[i]["Item_id"].ToString();
                    //        string itemName = itemList.Tables[0].Rows[i]["Item_name"].ToString();
                    //        //string Uom = itemList.Tables[0].Rows[i]["uom_name"].ToString();
                    //        ItemList.Add(itemId/* + "_" + Uom*/, itemName);
                    //    }
                    //}

                    //  DataRows = Json(JsonConvert.SerializeObject(itemList));/*Result convert into Json Format for javasript*/
                }
                return Json(JsonConvert.SerializeObject(itemList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
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
                DataSet result = _BillofMaterial_ISERVICES.GetSOItemUOMDL(Itm_ID, Comp_ID);
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
        public void BindOperationNameList(BillofMaterialModel _BOMModel)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    dt = _BillofMaterial_ISERVICES.GetOperationNameList(Convert.ToInt32(Comp_ID));
                    List<OperationName> _Status = new List<OperationName>();
                    if (dt.Rows.Count > 0)
                    {
                        //OperationName _Statuslist1 = new OperationName();/*commented by Hina on 13-09-2024 to add below out of table*/
                        //_Statuslist1.op_id = "0";
                        //_Statuslist1.op_name = "---Select---";
                        //_Status.Add(_Statuslist1);
                        foreach (DataRow data in dt.Rows)
                        {
                            OperationName _Statuslist = new OperationName();
                            _Statuslist.op_id = data["op_id"].ToString();
                            _Statuslist.op_name = data["op_name"].ToString();
                            _Status.Add(_Statuslist);
                        }
                    }
                    _Status.Insert(0, new OperationName() { op_id = "0", op_name = "---Select---" });/*Add by Hina on 13-09-2024*/

                    _BOMModel.OperationNameList = _Status;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public JsonResult GetBomOpNameList()
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _BillofMaterial_ISERVICES.GetOperationNameList(Convert.ToInt32(Comp_ID));
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetItemCost(Int32 CompId, Int32 BranchId, string ddl_ProductId)
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
                DataSet result = _BillofMaterial_ISERVICES.GetItemCost(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), ddl_ProductId);
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
        public ActionResult ToRefreshByJS(string TrancType,string Mailerror)
        {
            //Session["MessageBOM"] = "";
            BillofMaterialModel _BOMModel = new BillofMaterialModel();
            UrlModelData UrlData = new UrlModelData();
            var a = TrancType.Split(',');
            var prodct = a[0].Trim();
            var rev_no = prodct.Split('_');
            _BOMModel.product_id = rev_no[0].Trim();
            _BOMModel.rev_no = Convert.ToInt32(rev_no[1]);
            _BOMModel.TransTypeBOM = "Update";
            var WF_status1 = a[2].Trim();
            _BOMModel.BtnName = "BtnToDetailPage";
            _BOMModel.MessageBOM  = Mailerror;
            _BOMModel.WF_status1 = WF_status1;
            _BOMModel.dbclick = "dbclick";
            TempData["WF_status1"] = WF_status1;
            TempData["ModelData"] = _BOMModel;
            UrlData.product_id = rev_no[0].Trim();
            UrlData.rev_no = _BOMModel.rev_no;
            UrlData.TransTypeBOM = "Update";
            UrlData.BtnName = "BtnToDetailPage";
            UrlData.CommandBOM = "Refresh";
            UrlData.dbclick = "dbclick";
            return RedirectToAction("AddBillofMaterialDetail", UrlData);
        }

        public ActionResult GetBillofMaterialList(string docid, string status)
        {
            BillofMaterialModel _BOMModel = new BillofMaterialModel();
            //Session["WF_status"] = status;
            _BOMModel.WF_status = status;
            return RedirectToAction("BillofMaterial", _BOMModel);
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string WF_status1)
        {

            BillofMaterialModel BillofMaterial_Model = new BillofMaterialModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    string str = jObjectBatch[i]["DocNo"].ToString();
                    string[] str1 = str.Split('_');

                    BillofMaterial_Model.product_id = str1[0].ToString();// jObjectBatch[i]["DocNo"].ToString();
                    BillofMaterial_Model.rev_no = Convert.ToInt32(str1[1]);// jObjectBatch[i]["DocNo"].ToString();
                    BillofMaterial_Model.DocDate = jObjectBatch[i]["DocDate"].ToString();
                    BillofMaterial_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    BillofMaterial_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    BillofMaterial_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (BillofMaterial_Model.A_Status != "Approve" || BillofMaterial_Model.A_Status == "" || BillofMaterial_Model.A_Status == null)
            {
                BillofMaterial_Model.A_Status = "Approve";
            }
            BillOfMeterialApprove(BillofMaterial_Model);
            UrlModelData UrlDataApprove = new UrlModelData();
            UrlDataApprove.TransTypeBOM = BillofMaterial_Model.TransTypeBOM;
            UrlDataApprove.CommandBOM = BillofMaterial_Model.CommandBOM;
            UrlDataApprove.BtnName = BillofMaterial_Model.BtnName;
            UrlDataApprove.product_id = BillofMaterial_Model.product_id;
            UrlDataApprove.rev_no = BillofMaterial_Model.rev_no;
            UrlDataApprove.dbclick = BillofMaterial_Model.dbclick;
            TempData["ModelData"] = BillofMaterial_Model;
            TempData["WF_status1"] = WF_status1;
            return RedirectToAction("AddBillofMaterialDetail", UrlDataApprove);
        }
        /*--------Print---------*/

        public FileResult GenratePdfFile(BillofMaterialModel _Model)
        {
            // return File("ErrorPage", "application/pdf");
            var pdfData = GetPdfData(_Model.product_id, _Model.rev_no);
            if (pdfData != null)
                return File(pdfData, "application/pdf", "BillOfMaterial.pdf");
            else
                return File("ErrorPage", "application/pdf");
        }
        public byte[] GetPdfData(string productId, int revNo)
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
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _BillofMaterial_ISERVICES.GetBillofMaterialPrintDeatils(CompID, Br_ID, productId, revNo);
                ViewBag.PageName = "BOM";
                ViewBag.Title = "Bill of Material";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/BillofMaterial/BillofMaterialPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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

        /*--------Print End--------*/
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, Br_ID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Convert.ToInt32(Doc_dt));
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "ErrorPage";
            }
            return null;
        }
        /*--------Alternate Items Start-------- */
        // Created by Suraj on 16-10-2023
        public ActionResult GetAlternateItemDetail(string disabled,string ArrListAltItemDetail)
        {
            try
            {
                ViewBag.Disable = disabled;
                DataTable dt = new DataTable();

                dt.Columns.Add("ItemName", typeof(string));
                dt.Columns.Add("ItemNameId", typeof(string));
                dt.Columns.Add("ItemUom", typeof(string));
                dt.Columns.Add("ItemUomId", typeof(string));
                dt.Columns.Add("AltQty", typeof(string));
                dt.Columns.Add("AltItemCost", typeof(string));

                  
                JArray jObj = JArray.Parse(ArrListAltItemDetail);
                for (int i = 0; i < jObj.Count; i++)
                {
                    DataRow dtRow = dt.NewRow();
                    dtRow["ItemName"] = jObj[i]["ItemName"];
                    dtRow["ItemNameId"] = jObj[i]["ItemNameId"];
                    dtRow["ItemUom"] = jObj[i]["ItemUom"];
                    dtRow["ItemUomId"] = jObj[i]["ItemUomId"];
                    dtRow["AltQty"] = jObj[i]["AltQty"];
                    dtRow["AltItemCost"] = jObj[i]["AltItemCost"];
                    dt.Rows.Add(dtRow);
                }

                ViewBag.AltenateItemDetailItemWise = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBOMAlternateItemDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

            
        }
        /*--------Alternate Items End--------*/
        //added By Nitesh 26-10-2023 1050 for  shopfloore bind dropdown 
        public void BindShopFloorList(BillofMaterialModel _BOMModel1)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    dt = _BillofMaterial_ISERVICES.GetShopFloorDetailsDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id));
                    List<ShopFloor> _Status = new List<ShopFloor>();
                    if (dt.Rows.Count > 0)
                    {
                        //ShopFloor _Statuslist1 = new ShopFloor();/*commented by Hina on 13-09-2024 to add below out of table*/
                        //_Statuslist1.shfl_id = "0";
                        //_Statuslist1.shfl_name = "---Select---";
                        //_Status.Add(_Statuslist1);
                        foreach (DataRow data in dt.Rows)
                        {
                            ShopFloor _Statuslist = new ShopFloor();
                            _Statuslist.shfl_id = data["shfl_id"].ToString();
                            _Statuslist.shfl_name = data["shfl_name"].ToString();
                            _Status.Add(_Statuslist);
                        }
                    }
                    _Status.Insert(0, new ShopFloor() { shfl_id = "0", shfl_name = "---Select---" });/*Add by Hina on 13-09-2024*/

                    _BOMModel1.ShopFloorList = _Status;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        //added By Nitesh 26-10-2023 1050 for  repicate with bind dropdown approve and default product
        public ActionResult BindReplicateWithlist(BillofMaterialModel _BOMModel1)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
               
               // JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string SOItmName = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    if (string.IsNullOrEmpty(_BOMModel1.SO_ItemName))
                    {
                        SOItmName = "0";
                    }
                    else
                    {
                        SOItmName = _BOMModel1.SO_ItemName;
                    }
                    DataSet ProductList = _BillofMaterial_ISERVICES.getrepicateitem(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), SOItmName);               
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        //ItemList.Add("0" + "_" + "H1", "Heading");
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string itemId = ProductList.Tables[0].Rows[i]["Item_id"].ToString();
                            string itemName = ProductList.Tables[0].Rows[i]["Item_name"].ToString();
                            string Uom = ProductList.Tables[0].Rows[i]["uom_alias"].ToString();
                            ItemList.Add(itemId + "_" + Uom, itemName);
                        }
                    }
                
                }
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //added By Nitesh 26-10-2023 1050 for onchange repicate data itemdata
        public JsonResult getreplicateitemdata(string replicate_item) 
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
                DataSet result = _BillofMaterial_ISERVICES.getreplicateitemdata( Comp_ID,Br_ID,replicate_item);
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
        public JsonResult getUomConvRate(string ItemId, string UomId)
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
                DataSet result = _BillofMaterial_ISERVICES.getUomConvRate(Comp_ID, Br_ID, ItemId, UomId);
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
    }
}