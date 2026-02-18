using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.GatePassInward;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassInward;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.GatePassInward
{
    public class GatePassInwardController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105102101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        GatePassInward_IServices _GatePassInward_IServices;

        public GatePassInwardController(Common_IServices _Common_IServices, GatePassInward_IServices _GatePassInward_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._GatePassInward_IServices = _GatePassInward_IServices;
        }

        public ActionResult DashBordtoList(string docid, string status)
        {
            var WF_status = status;
            return RedirectToAction("GatePassInward", new { WF_status });
        }

        // GET: ApplicationLayer/GatePassInward
        public ActionResult GatePassInward(string WF_status)
        {
            try
            {
                GatePassInwardList_Model ListModel = new GatePassInwardList_Model();
                CommonPageDetails();
                BindAllDropDownList(ListModel);
                ListModel.WF_Status = WF_status;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                GatePassInwardList_Model Gpass_model = new GatePassInwardList_Model();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var Entity_type = a[0].Trim();
                    var Entity_name = a[1].Trim();
                    var Fromdate = a[2].Trim();
                    var Todate = a[3].Trim();
                    var Status = a[4].Trim();
                    var Source_type = a[5].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    ListModel.EntityName = Entity_name;
                    Gpass_model.entity_type = Entity_type;
                    GetSupp_CustList(Gpass_model);
                    ListModel.EntityNameList = Gpass_model.EntityNameList;
                    ListModel.EntityName = a[1].Trim();
                    ListModel.FromDate = Fromdate;
                    ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    GetSupp_CustList(Gpass_model);
                    DataTable dt = _GatePassInward_IServices.SearchDataFilter(Source_type, Entity_type, Entity_name, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                    ViewBag.GatePassReciptList = dt;
                    ListModel.EntityType = Entity_type;
                    ListModel.EntityName = Entity_name;
                    ListModel.FromDate = Fromdate;
                    ListModel.ToDate = Todate;
                    ListModel.Status = Status;
                    ListModel.SourceType = Source_type;
                }
                else
                {
                    DataTable GpassList = new DataTable();

                  
                    ListModel.FromDate = startDate;
                    ListModel.ToDate = CurrentDate;
                    GpassList = GetGatePassDetails(ListModel);
                    ViewBag.GatePassReciptList = GpassList;
                }
                ListModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/GatePassInward/GatePassInwardList.cshtml", ListModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            
        }
        private DataTable GetGatePassDetails(GatePassInwardList_Model _listModel)
        {
            CompDataWithID();
            string wfstatus = "";

            if (_listModel.WF_Status != null)
            {
                wfstatus = _listModel.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            //DataSet dt = _GatePassInward_IServices.GetAllDropDownList(CompID, BrchID, UserID, wfstatus, DocumentMenuId);
            DataSet dt = _GatePassInward_IServices.GetAllDropDownList(CompID, BrchID, UserID, wfstatus, DocumentMenuId, _listModel.FromDate, _listModel.ToDate);
            return dt.Tables[0];
        }
        public void BindAllDropDownList(GatePassInwardList_Model ListModel)
        {
            List<EntityNameList> _EntityName = new List<EntityNameList>();
            EntityNameList _EntityNameList = new EntityNameList();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            ListModel.EntityNameList = _EntityName;
            List<Status> list2 = new List<Status>();
            foreach (var dr in ViewBag.StatusList.Rows)
            {
                Status Status = new Status();
                Status.status_id = dr["status_code"].ToString();
                Status.status_name = dr["status_name"].ToString();
                list2.Add(Status);
            }
            //   list2.Insert(0, new Status() { status_id = "0", status_name = "All" });
            ListModel.StatusList = list2;
            //DataSet dt = _GatePassOutwardList_IServices.GetAllDropDownList(CompID, BrchID);
        }
        private void CommonPageDetails()
        {
            try
            {

                CompDataWithID();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
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
        public ActionResult AddGatePassInwardDetail()
        {
            CompDataWithID();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("GatePassInward");
            }
            else
            {
                GatePassInwardDetail_Model AddNew_Model = new GatePassInwardDetail_Model();
                AddNew_Model.Massage = null;
                AddNew_Model.TransType = "Save";
                AddNew_Model.Command = "AddNew";
                AddNew_Model.BtnName = "BtnAddNew";
                TempData["ModelData"] = AddNew_Model;
                UrlModel NewModel = new UrlModel();
                NewModel.Cmd = "AddNew";
                NewModel.tp = "Save";
                NewModel.bt = "BtnAddNew";
                return RedirectToAction("GatePassInwardDetail", NewModel);
            }
           
        }
        public void CompDataWithID()
        {
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
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        public ActionResult GatePassInwardDetail(UrlModel _urlModel)
        {
            try
            {
                CommonPageDetails();
                CompDataWithID();
                var DetailModel = TempData["ModelData"] as GatePassInwardDetail_Model;
                if (DetailModel != null)
                {
                    SetData(DetailModel);
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/GatePassInward/GatePassInwardDetail.cshtml", DetailModel);
                }
                else
                {
                     GatePassInwardDetail_Model DetailModel1 = new GatePassInwardDetail_Model();
                    SetUrlData(_urlModel, DetailModel1);
                    SetData(DetailModel1);
                      return View("~/Areas/ApplicationLayer/Views/InventoryManagement/GatePassInward/GatePassInwardDetail.cshtml", DetailModel1);
                } 
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public void SetData(GatePassInwardDetail_Model DetailModel)
        {
            if (DetailModel.GatePassDate == null)
            {
                DetailModel.GatePassDate = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
            DetailModel.docNoLists = srcDocNoLists;

            List<EntityNameList1> _EntityName = new List<EntityNameList1>();
            EntityNameList1 _EntityNameList = new EntityNameList1();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            DetailModel.EntityNameList = _EntityName;
            DetailModel.Title = title;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                DetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
            }
            if (DetailModel.TransType == "Update")
            {
                SetAllDataInView(DetailModel);
            }
            else
            {
                ViewBag.DocumentCode = "0";
                DetailModel.DocumentStatus = "New";
             
            }
            DetailModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;
            DetailModel.DocumentID = DocumentMenuId;
            ViewBag.Command = DetailModel.Command;
            ViewBag.TransType = DetailModel.TransType;
            ViewBag.DocumentStatus = DetailModel.DocumentStatus;
            ViewBag.DocumentCode = DetailModel.DocumentStatus;
            DetailModel.Qty_pari_Command = DetailModel.Command;
        }
        public void SetAllDataInView(GatePassInwardDetail_Model _GatePassModel)
        {
            try
            {
                GatePassInwardDetail_Model Gpass_model=new GatePassInwardDetail_Model();
                GetSupp_CustListDeatil(Gpass_model);
                Gpass_model.EntityNameList = Gpass_model.EntityNameList;
                CompDataWithID();
                string Gpass_no = _GatePassModel.GatePassNumber;
                string Gpass_dt = _GatePassModel.GatePassDate;
                DataSet ds = new DataSet();
                ds= _GatePassInward_IServices.GetFGRDeatilData(CompID, BrchID, Gpass_no, Gpass_dt, UserID, DocumentMenuId);
                ViewBag.AttechmentDetails = ds.Tables[2];
                ViewBag.ItemDetailData = ds.Tables[1];
                ViewBag.ItemOrderQtyDetail = ds.Tables[6];

                _GatePassModel.GatePassNumber = ds.Tables[0].Rows[0]["gpass_no"].ToString();
                _GatePassModel.GatePassDate = ds.Tables[0].Rows[0]["gpass_dt"].ToString();
                _GatePassModel.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                _GatePassModel.EntityTypeID = ds.Tables[0].Rows[0]["entity_type"].ToString();
                _GatePassModel.Source_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                _GatePassModel.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();

                _GatePassModel.EntityID = ds.Tables[0].Rows[0]["entity_id"].ToString();
                _GatePassModel.EntityName = ds.Tables[0].Rows[0]["Entityname"].ToString();             
                _GatePassModel.ReceivedBy = ds.Tables[0].Rows[0]["rec_by"].ToString();
                _GatePassModel.remarks = ds.Tables[0].Rows[0]["gpass_rem"].ToString();
                _GatePassModel.Created_by = ds.Tables[0].Rows[0]["createname"].ToString();
                _GatePassModel.Created_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                _GatePassModel.Approved_by = ds.Tables[0].Rows[0]["appname"].ToString();
                _GatePassModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                _GatePassModel.Amended_by = ds.Tables[0].Rows[0]["modname"].ToString();
                _GatePassModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                _GatePassModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                _GatePassModel.Status_Code = ds.Tables[0].Rows[0]["gpass_status"].ToString();
                _GatePassModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                _GatePassModel.Address = ds.Tables[0].Rows[0]["EntityAddress"].ToString();
                _GatePassModel.bill_add_id = ds.Tables[0].Rows[0]["billing_Address"].ToString();

                string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                _GatePassModel.StatusCode = doc_status;
                _GatePassModel.doc_status = doc_status;
                _GatePassModel.DocumentStatus = doc_status;
                ViewBag.DocumentCode = doc_status;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                EntityNameList1 _EntityNameList = new EntityNameList1();
                _EntityNameList.entity_name = _GatePassModel.EntityName;
                _EntityNameList.entity_id = _GatePassModel.EntityID;
                _EntityName.Add(_EntityNameList);
                _GatePassModel.EntityNameList = _EntityName;

                if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                {
                    _GatePassModel.CancelFlag = true;

                    _GatePassModel.BtnName = "Refresh";
                }
                else
                {
                    _GatePassModel.CancelFlag = false;
                }
                _GatePassModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                _GatePassModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                if (doc_status != "D" && doc_status != "F")
                {
                    ViewBag.AppLevel = ds.Tables[5];
                }
                if (ViewBag.AppLevel != null && _GatePassModel.Command != "Edit")
                {

                    var sent_to = "";
                    var nextLevel = "";
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                    }

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                    }

                    if (doc_status == "D")
                    {
                        if (create_id != UserID)
                        {
                            _GatePassModel.BtnName = "Refresh";
                        }
                        else
                        {
                            if (nextLevel == "0")
                            {
                                if (create_id == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                _GatePassModel.BtnName = "BtnEdit";
                            }
                            else
                            {
                                ViewBag.Approve = "N";
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                _GatePassModel.BtnName = "BtnEdit";
                            }

                        }
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message1"] != null)
                            {
                                ViewBag.Message = TempData["Message1"];
                            }
                            _GatePassModel.BtnName = "BtnEdit";
                        }


                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                _GatePassModel.BtnName = "BtnEdit";
                            }


                        }
                    }
                    if (doc_status == "F")
                    {
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message1"] != null)
                            {
                                ViewBag.Message = TempData["Message1"];
                            }
                            /*End to chk Financial year exist or not*/
                            _GatePassModel.BtnName = "BtnEdit";
                        }
                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                            }
                            _GatePassModel.BtnName = "BtnEdit";
                        }
                    }
                    if (doc_status == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _GatePassModel.BtnName = "BtnEdit";

                        }
                        else
                        {
                            _GatePassModel.BtnName = "Refresh";
                        }
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private void SetUrlData(UrlModel _urlModel, GatePassInwardDetail_Model _Model)
        {
            if (_urlModel.tp != null)
            {
                _Model.TransType = _urlModel.tp;
            }
            else
            {
                _Model.TransType = "Refresh";
            }
            if (_urlModel.bt != null)
            {
                _Model.BtnName = _urlModel.bt;
            }
            else
            {
                _Model.BtnName = "BtnRefresh";
            }
            if (_urlModel.Cmd != null)
            {
                _Model.Command = _urlModel.Cmd;
            }
            else
            {
                _Model.Command = "Save";
            }
            if (_urlModel.DMS != null)
            {
                _Model.DocumentStatus = _urlModel.DMS;
            }
            else
            {
                _Model.DocumentStatus = "";
            }
            if (_urlModel.GP_no != null && _urlModel.GP_dt != "")
            {
                _Model.GatePassNumber = _urlModel.GP_no;
                _Model.GatePassDate = _urlModel.GP_dt;
            }

        }
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}

        public ActionResult GetSupp_CustList(GatePassInwardList_Model ListModel)
        {
            try
            {
                CompDataWithID();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList> _EntityName = new List<EntityNameList>();
                if (string.IsNullOrEmpty(ListModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = ListModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(ListModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = ListModel.entity_type.ToString();
                }
                DataSet SuppCustList = _GatePassInward_IServices.getSuppCustList(CompID, BrchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                ListModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetSupp_CustListDeatil(GatePassInwardDetail_Model DetailModel)
        {
            try
            {
                CompDataWithID();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                if (string.IsNullOrEmpty(DetailModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = DetailModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(DetailModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = DetailModel.entity_type.ToString();
                }
                DataSet SuppCustList = _GatePassInward_IServices.getSuppCustList(CompID, BrchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                DetailModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpPost]
        public ActionResult DataSearch_Search(string Source_type, string Entity_type, string Entity_id, string Fromdate, string Todate, string Status)
        {
            try
            {
                CompDataWithID();
                GatePassInwardList_Model SearchModel = new GatePassInwardList_Model();
                SearchModel.WF_Status = null;
                DataTable dt = _GatePassInward_IServices.SearchDataFilter(Source_type, Entity_type, Entity_id, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                SearchModel.GpassSearch = "Search";
                ViewBag.GatePassReciptList = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGatePassInward.cshtml", SearchModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public JsonResult GetSourceDocList(string Itm_ID, string SuppID, string entity_type, string sr_number)
        {
            JsonResult DataRows = null;
            try
            {
                CompDataWithID();
                DataSet Deatils = _GatePassInward_IServices.GetSourceDocList(CompID, BrchID, SuppID, entity_type);

                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult BindItemTable(string entity_type, string entity_Name, string Doc_no, string Doc_dt,string hdn_Status,string GatePassNumber,string ddlGatePassDate)
        {
            try
            {
                JsonResult DataRows = null;
                CompDataWithID();
                DataSet Deatils = _GatePassInward_IServices.GetItemDeatilData(CompID, BrchID, entity_Name, entity_type, Doc_no, Doc_dt, GatePassNumber);
                DataRows = Json(JsonConvert.SerializeObject(Deatils));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
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
        public ActionResult SaveGPassInDeatilData(GatePassInwardDetail_Model DetailSaveModel, string command)
        {
            try
            {
                CompDataWithID();
                var commCont = new CommonController(_Common_IServices);
                if (DetailSaveModel.DeleteCommand == "Delete")
                {
                    if (true)
                    {
                        command = "Delete";
                    }
                }
                switch (command)
                {
                    case "AddNew":
                        GatePassInwardDetail_Model AddNewModel = new GatePassInwardDetail_Model();
                        AddNewModel.Command = "AddNew";
                        AddNewModel.TransType = "Save";
                        AddNewModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = AddNewModel;
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "AddNew";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";

                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(DetailSaveModel.GatePassNumber))
                            {
                                return RedirectToAction("DblClick", new { rcpt_no = DetailSaveModel.GatePassNumber, rcpt_dt = DetailSaveModel.GatePassDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                            }
                            else
                            {
                                NewModel.Cmd = "Refresh";
                                NewModel.tp = "Refresh";
                                NewModel.bt = "BtnRefresh";
                                NewModel.DMS = null;
                                TempData["ModelData"] = NewModel;
                                return RedirectToAction("GatePassInwardDetail", NewModel);
                            }
                        }
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("GatePassInwardDetail", NewModel);
                    case "Edit":
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DblClick", new { Gate_no = DetailSaveModel.GatePassNumber, Gate_dt = DetailSaveModel.GatePassDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        UrlModel EditModel = new UrlModel();
                        EditModel.Cmd = command;
                        DetailSaveModel.Command = command;
                        DetailSaveModel.BtnName = "BtnEdit";
                        DetailSaveModel.TransType = "Update";
                        DetailSaveModel.GatePassNumber = DetailSaveModel.GatePassNumber;
                        DetailSaveModel.GatePassDate = DetailSaveModel.GatePassDate;
                        TempData["ModelData"] = DetailSaveModel;
                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";
                        EditModel.GP_no = DetailSaveModel.GatePassNumber;
                        EditModel.GP_dt = DetailSaveModel.GatePassDate;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("GatePassInwardDetail", EditModel);
                    case "Refresh":
                        GatePassInwardDetail_Model RefreshModel = new GatePassInwardDetail_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel Refresh_Model = new UrlModel();
                        Refresh_Model.tp = "Save";
                        Refresh_Model.bt = "BtnRefresh";
                        Refresh_Model.Cmd = command;
                        TempData["ListFilterData"] = RefreshModel.ListFilterData1;
                        return RedirectToAction("GatePassInwardDetail", Refresh_Model);
                    case "Save":
                        GatePass_SaveUpdate(DetailSaveModel);
                        if (DetailSaveModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = DetailSaveModel;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = DetailSaveModel.BtnName;
                        SaveModel.GP_no = DetailSaveModel.GatePassNumber;
                        SaveModel.GP_dt = DetailSaveModel.GatePassDate;
                        SaveModel.tp = DetailSaveModel.TransType;
                        SaveModel.Cmd = DetailSaveModel.Command;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("GatePassInwardDetail", SaveModel);
                    case "Delete":
                        Delete(DetailSaveModel, command);
                        GatePassInwardDetail_Model DeleteModel = new GatePassInwardDetail_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("GatePassInwardDetail", Delete_Model);
                    case "Forward":
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DblClick", new { Gate_no = DetailSaveModel.GatePassNumber, Gate_dt = DetailSaveModel.GatePassDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        return new EmptyResult();

                    case "Approve":
                        ApproveData(DetailSaveModel, "", "");
                        TempData["ModelData"] = DetailSaveModel;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.GP_no = DetailSaveModel.GatePassNumber;
                        Approve.GP_dt = DetailSaveModel.GatePassDate;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("GatePassInwardDetail", Approve);
                       
                    case "Print":
                        //return new EmptyResult();
                        return GenratePdfFile(DetailSaveModel);
                    case "BacktoList":
                        var WF_status = DetailSaveModel.WF_Status1;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("GatePassInward", new { WF_status });

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
       
        private ActionResult Delete(GatePassInwardDetail_Model _model, string command)
        {
            try
            {
                CompDataWithID();
                string doc_no = _model.GatePassNumber;
                DataSet Message = _GatePassInward_IServices.DeleteData(CompID, BrchID, _model.GatePassNumber, _model.GatePassDate);
                if (!string.IsNullOrEmpty(doc_no))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, doc_no1, Server);
                }
                return RedirectToAction("GatePassInwardDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult GatePass_SaveUpdate(GatePassInwardDetail_Model _model)
        {
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_model.CancelFlag == false /*&& _model.ReturnableFlag == false*/)
                {
                    CompDataWithID();
                    DataTable FGRHeader = new DataTable();
                    DataTable ItemDetails = new DataTable();
                    DataTable srcItemDetails = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("gpass_no", typeof(string));
                    dtheader.Columns.Add("gpass_dt", typeof(string));
                    dtheader.Columns.Add("src_type", typeof(char));

                    dtheader.Columns.Add("entity_type", typeof(string));
                    dtheader.Columns.Add("entity_id", typeof(string));                  
                    dtheader.Columns.Add("rec_by", typeof(string));
                    dtheader.Columns.Add("gpass_rem", typeof(string));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("gpass_status", typeof(char));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("add_id", typeof(int));
                 


                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;

                    if (_model.GatePassNumber != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = CompID;

                    dtrowHeader["br_id"] = BrchID;
                    dtrowHeader["gpass_no"] = _model.GatePassNumber;
                    dtrowHeader["gpass_dt"] = _model.GatePassDate;
                    dtrowHeader["src_type"] = _model.Source_Type;

                    dtrowHeader["entity_type"] = _model.EntityTypeID;
                    dtrowHeader["entity_id"] = _model.EntityID;
               
                    dtrowHeader["rec_by"] = _model.ReceivedBy;
                    dtrowHeader["gpass_rem"] = _model.remarks;
                    dtrowHeader["create_id"] = UserID;
                    dtrowHeader["gpass_status"] = "D";
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtrowHeader["mac_id"] = SystemDetail;
                    dtrowHeader["add_id"] = Convert.ToInt32(_model.bill_add_id);
                    dtheader.Rows.Add(dtrowHeader);
                    FGRHeader = dtheader;
                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("issue_qty", typeof(string));
                    dtItem.Columns.Add("gpass_qty", typeof(string));
                    dtItem.Columns.Add("remarks", typeof(string));
                    dtItem.Columns.Add("sr_no", typeof(int));

                    JArray jObject = JArray.Parse(_model.HDNItemDeatilDataSave);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["issue_qty"] = jObject[i]["IssuedQuantity"].ToString();
                        dtrowLines["gpass_qty"] = jObject[i]["ReceivedQuantity"].ToString();
                        dtrowLines["remarks"] = jObject[i]["ItemRemarks"].ToString();
                        dtrowLines["sr_no"] = Convert.ToInt32(jObject[i]["Srno"].ToString());
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ItemDetails = dtItem;
                    DataTable srcdtItem = new DataTable();
                    srcdtItem.Columns.Add("item_id", typeof(string));
                    srcdtItem.Columns.Add("uom_id", typeof(int));
                    srcdtItem.Columns.Add("Document_no", typeof(string));
                    srcdtItem.Columns.Add("Document_dt", typeof(string));
                    srcdtItem.Columns.Add("issue_qty", typeof(string));
                    srcdtItem.Columns.Add("rec_qty", typeof(string));                 

                    JArray Data1 = JArray.Parse(_model.SaveSrcDocDeatil);
                    for (int i = 0; i < Data1.Count; i++)
                    {
                        DataRow dtrowLines1 = srcdtItem.NewRow();
                        dtrowLines1["item_id"] = Data1[i]["item_id"].ToString();
                        dtrowLines1["uom_id"] = Data1[i]["UOMID"].ToString();
                        dtrowLines1["Document_no"] = Data1[i]["Document_no"].ToString();
                        dtrowLines1["Document_dt"] =Data1[i]["Documentdt"].ToString();
                        dtrowLines1["issue_qty"] = Data1[i]["IssuedQuantity"].ToString();
                        dtrowLines1["rec_qty"] = Data1[i]["ReceivedQuantity"].ToString();
                        srcdtItem.Rows.Add(dtrowLines1);
                    }
                    srcItemDetails = srcdtItem; 

                    DataTable dtAttachment = new DataTable();
                    var _GatePassattch = TempData["ModelDataattch"] as GatePassattchment;
                    TempData["ModelDataattch"] = null;
                    if (_model.attatchmentdetail != null)
                    {
                        if (_GatePassattch != null)
                        {
                            if (_GatePassattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _GatePassattch.AttachMentDetailItmStp as DataTable;
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
                            if (_model.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _model.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_model.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_model.GatePassNumber))
                                {
                                    dtrowAttachment1["id"] = _model.GatePassNumber;
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

                        if (_model.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_model.GatePassNumber))
                                {
                                    ItmCode = _model.GatePassNumber;
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
                                        if (drImgPath == fielpath.Replace("/", @"\"))
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

                    SaveMessage = _GatePassInward_IServices.InsertUpdateData(FGRHeader, ItemDetails, Attachments, srcItemDetails);
                    string[] Data = SaveMessage.Split(',');

                    string GatePassNo = Data[1];
                    string Message = Data[0];
                    string GatePassDate = Data[2];
                    if (Message == "DataNotFound")
                    {

                        var a = GatePassNo.Split(',');
                        var msg = "Data Not found" + " " + a[0].Trim() + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _model.Message = Message;
                        return RedirectToAction("GatePassInwardDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_GatePassattch != null)
                        {
                            if (_GatePassattch.Guid != null)
                            {
                                Guid = _GatePassattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, GatePassNo, _model.TransType, Attachments);
                    }
                    if (Message == "Update" || Message == "Save")
                    {
                        _model.Message = "Save";
                        _model.Command = "Update";
                        _model.GatePassNumber = GatePassNo;
                        _model.GatePassDate = GatePassDate;
                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.AttachMentDetailItmStp = null;
                        _model.Guid = null;
                    }
                }
                else
                {                
                    CompDataWithID();
                    string br_id = BrchID;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet message = _GatePassInward_IServices.CancelAndReturnData(CompID, br_id, _model.GatePassNumber, _model.GatePassDate, UserID, DocumentMenuId, mac_id);

                    _model.Message = message.Tables[0].Rows[0]["result"].ToString();
                    _model.GatePassNumber = _model.GatePassNumber;
                    _model.GatePassDate = _model.GatePassDate;
                    _model.TransType = "Update";
                    _model.BtnName = "Refresh";

                    string fileName = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    // var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptDate, _model.RecieptDate, fileName);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _model.GatePassNumber, "C", UserID, "0" /*filePath*/);

                    return RedirectToAction("GatePassInwardDetail");
                }
                return null;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {

                    if (_model.TransType == "Save")
                    {
                        string Guid = "";

                        if (_model.Guid != null)
                        {

                            Guid = _model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                CompDataWithID();
                CommonPageDetails();
                GatePassattchment _GatePassattch = new GatePassattchment();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();

                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");

                _GatePassattch.Guid = DocNo;
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _GatePassattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _GatePassattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _GatePassattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult DblClick(string Gate_no, string Gate_dt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            CompDataWithID();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            GatePassInwardDetail_Model dblclick = new GatePassInwardDetail_Model();
            UrlModel _url = new UrlModel();
            dblclick.GatePassNumber = Gate_no;
            dblclick.GatePassDate = Gate_dt;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            dblclick.Command = "Refresh";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _url.tp = "Update";
            _url.bt = "BtnEdit";
            _url.GP_no = Gate_no;
            _url.GP_dt = Gate_dt;
            _url.Cmd = "Refresh";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("GatePassInwardDetail", _url);
        }
        [HttpPost]
        public ActionResult getItemOrderQuantityDetail(string ItemID, string Status, string SelectedItemdetail,  string TransType, string Command, string docid,string flag)
        {
            try
            {

                DataTable DTableOrderQty = new DataTable();
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    DataTable dtorderqty = new DataTable();
                    dtorderqty.Columns.Add("Doc_no", typeof(string));
                    dtorderqty.Columns.Add("Doc_dt", typeof(string));
                    dtorderqty.Columns.Add("Docdt", typeof(string));
                    dtorderqty.Columns.Add("item_id", typeof(string));
                    dtorderqty.Columns.Add("uom_id", typeof(string));
                    dtorderqty.Columns.Add("issue_qty", typeof(string));
                    dtorderqty.Columns.Add("Pending_qty", typeof(string));
                    dtorderqty.Columns.Add("Rec_qty", typeof(string));

                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);

                    foreach (JObject item in jObjectBatch.Children())
                    {
                        if (item.GetValue("ItemID").ToString() == ItemID.ToString())
                        {
                            DataRow dtorderqtyrow = dtorderqty.NewRow();
                            dtorderqtyrow["Doc_no"] = item.GetValue("DocNo").ToString();
                            dtorderqtyrow["Doc_dt"] = item.GetValue("DocDate").ToString();
                            dtorderqtyrow["Docdt"] = item.GetValue("Doc_Date").ToString();
                            dtorderqtyrow["item_id"] = item.GetValue("ItemID").ToString();
                            dtorderqtyrow["uom_id"] = item.GetValue("UomID").ToString();
                            dtorderqtyrow["issue_qty"] = item.GetValue("Issue_qty").ToString();
                            dtorderqtyrow["Pending_qty"] = item.GetValue("PendingQty").ToString();
                            dtorderqtyrow["Rec_qty"] = item.GetValue("rec_qty").ToString();

                            dtorderqty.Rows.Add(dtorderqtyrow);
                        }
                    }
                    DTableOrderQty = dtorderqty;
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = docid;              
                ViewBag.Command = Command;
                ViewBag.TransType = TransType;
                ViewBag.flag = flag;
                if (DTableOrderQty.Rows.Count > 0)
                {
                    ViewBag.ItemOrderQtyDetail = DTableOrderQty;
                }
                return PartialView("~/Areas/Common/Views/Comn_PartialGatePassInwardDetail.cshtml");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            //Session["Message"] = "";
            GatePassInwardDetail_Model ToRefreshByJS = new GatePassInwardDetail_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.GatePassNumber = a[0].Trim();
            ToRefreshByJS.GatePassDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.GP_no = ToRefreshByJS.GatePassNumber;
            Model.GP_dt = ToRefreshByJS.GatePassDate;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("GatePassInwardDetail", Model);
        }

        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {

            GatePassInwardDetail_Model _model = new GatePassInwardDetail_Model();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _model.GatePassNumber = jObjectBatch[i]["DocNo"].ToString();
                    _model.GatePassDate = jObjectBatch[i]["DocDate"].ToString();
                    _model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_model.A_Status != "Approve")
            {
                _model.A_Status = "Approve";
            }
            ApproveData(_model, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _model;
            ApproveModel.tp = "Update";
            ApproveModel.GP_no = _model.GatePassNumber;
            ApproveModel.GP_dt = _model.GatePassDate;
            ApproveModel.bt = "BtnEdit";
            return RedirectToAction("GatePassInwardDetail", ApproveModel);
        }
        public ActionResult ApproveData(GatePassInwardDetail_Model _model, string ListFilterData1, string WF_Status1)
        {
            try
            {
                CompDataWithID();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string gpass_no = _model.GatePassNumber;
                string gpass_dt = _model.GatePassDate;
                string A_Status = _model.A_Status;
                string A_Level = _model.A_Level;
                string A_Remarks = _model.A_Remarks;
                string Message = _GatePassInward_IServices.Approve_details(CompID, BrchID, DocumentMenuId, gpass_no, gpass_dt, UserID, mac_id, A_Status, A_Level, A_Remarks);

                if (Message == "Approved")
                {
                    //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    ////var filePath = SavePdfDocToSendOnEmailAlert(Cnf_No, Cnf_Date, fileName);
                    //_Common_IServices.SendAlertEmail(CompID, brnchID, DocumentMenuId, Cnf_No, "AP", UserID, "0", filePath);

                    _model.Message = "Approved";
                }
                UrlModel ApproveModel = new UrlModel();
                _model.GatePassNumber = gpass_no;
                _model.GatePassDate = gpass_dt;
                _model.TransType = "Update";
                _model.BtnName = "BtnEdit";
                _model.Command = "Approve";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _model;

                ApproveModel.tp = "Update";
                ApproveModel.GP_no = _model.GatePassNumber;
                ApproveModel.GP_dt = _model.GatePassDate;
                ApproveModel.bt = "BtnEdit";
                ApproveModel.Cmd = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("GatePassInwardDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }

        public FileResult GenratePdfFile(GatePassInwardDetail_Model _GatePassOutward)
        {


            return File(GetPdfData(_GatePassOutward.GatePassNumber, _GatePassOutward.GatePassDate, _GatePassOutward.Status_Code), "application/pdf", "GatePassReceipt.pdf");
        }
        public byte[] GetPdfData(string Doc_No, string Doc_dt, string StatusCode)
        {
            StringReader reader = null;
             Document pdfDoc = null;
            PdfWriter writer = null;

            try
            {
                GatePassInwardDetail_Model _GatePassOutWard_Model = new GatePassInwardDetail_Model();
                CompDataWithID();
                DataSet Details = _GatePassInward_IServices.GetDataForPrint(CompID, BrchID, Doc_No, Convert.ToDateTime(Doc_dt).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                ViewBag.DocStatus = StatusCode.Trim();
                ViewBag.Title = "Gate Pass Receipt";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/GatePassInward/GatePassInwardPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 20f, 100f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 80, 0);
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


        [HttpPost]
        public JsonResult GetCustandSuppAddrDetail(string Entity_Name, string Entity_Type)
        {
            try
            {
                JsonResult DataRows = null;

                CompDataWithID();
                DataSet result = _GatePassInward_IServices.GetCustandSuppAddrDetailDL(Entity_Name, CompID, BrchID, Entity_Type);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
    }

}