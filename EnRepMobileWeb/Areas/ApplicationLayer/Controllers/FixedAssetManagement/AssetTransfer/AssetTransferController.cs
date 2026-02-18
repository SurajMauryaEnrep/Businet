using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetTransfer;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetTransfer;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Text;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.AssetTransfer
{
    public class AssetTransferController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105106115", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AssetTransfer_ISERVICES _AssetTransfer_ISERVICES;
        public AssetTransferController(Common_IServices _Common_IServices, AssetTransfer_ISERVICES _AssetTransfer_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AssetTransfer_ISERVICES = _AssetTransfer_ISERVICES;
        }
        #region Common Page Code
        private void CommonPageDetails()
        {
            try
            {
                GetCompDeatil();
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
        public void GetCompDeatil()
        {
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
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        public ActionResult ErrorPage()
        {
            try
            {
                throw new Exception("JsError");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        // GET: ApplicationLayer/AssetTransfer
        public ActionResult AssetTransfer(string wfStatus)
        {
            ViewBag.MenuPageName = getDocumentName();
            GetCompDeatil();
            CommonPageDetails();
            AssetTransferList_Model _model = new AssetTransferList_Model();
            if (wfStatus != null)
            {
                _model.WF_status = wfStatus;
                _model.ListFilterData = "0,0,0,0" + "," + wfStatus;
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string todate = range.ToDate;
            if (_model.AT_FromDate == null)
            {
                _model.FromDate = startDate;
                _model.AT_FromDate = startDate;
                _model.AT_ToDate = todate;
            }
            else
            {
                _model.FromDate = _model.AT_FromDate;
            }
            List<StatusLists> statusLists = new List<StatusLists>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                StatusLists list = new StatusLists();
                list.Status_id = dr["status_code"].ToString();
                list.Status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _model.StatusLists = statusLists;
            _model.Command = "Add";
            _model.TransType = "Save";
            _model.BtnName = "BtnAddNew";
            _model.DocumentMenuId = DocumentMenuId;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                if (ListFilterData != null && ListFilterData != "")
                {
                    var a = ListFilterData.Split(',');
                    var Status = a[2].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    // _model.AssetsGroupId = a[0].Trim();
                    _model.AssetItemsId = a[0].Trim();
                    _model.AssignedRequirementAreaId = a[1].Trim();
                    _model.FromDate = a[3].Trim();
                    _model.ToDate = a[4].Trim();
                    _model.ListFilterData = TempData["ListFilterData"].ToString();
                    _model.AT_status = Status;
                }
            }
            _model.Title = title;
            _model.PQASearch = "0";
            GetAssetDescriptionList(_model, "A");
            List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
            Lists.Add(new AssignedRequirementArea { AssignedRequirementArea_name = "---Select---", AssignedRequirementArea_id = "0" });
            _model.AssignedRequirementAreaList = Lists;
            //GetAssetGroupDetailListView(_model);
            GetAllListData(_model);
            return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetTransfer/AssetTransferList.cshtml", _model);
        }
        private void GetAllListData(AssetTransferList_Model _List_Model)
        {
            try
            {
                string SupplierName = string.Empty;
                CommonPageDetails();
                DataSet CustList = _AssetTransfer_ISERVICES.GetAllData(CompID, BrchID, UserID, _List_Model.AssetItemsId, _List_Model.AssignedRequirementAreaId, _List_Model.AT_status,   _List_Model.FromDate, _List_Model.AT_ToDate,_List_Model.WF_status, DocumentMenuId);
                ViewBag.ListDetail = CustList.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public ActionResult ATListSearch(string AssetId, string RequirementArea, string FromDate,string ToDate, string Status, string wfstatus)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = new DataSet();
                ds = _AssetTransfer_ISERVICES.GetAllData(CompID, BrchID,UserID, AssetId, RequirementArea, Status, FromDate, ToDate,  "0", DocumentMenuId);
                ViewBag.ListDetail = ds.Tables[0];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = AssetId + "," + RequirementArea + "," + RequirementArea + "," + FromDate + "," + "," + ToDate + "," + Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssetTransferList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AddAssetTransferDetail(string DocNo, string DocDt, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                GetCompDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (DocNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDt, ListFilterData);
                        return RedirectToAction("AssetTransferDetail", "AssetTransfer", urlData);
                    }
                }
                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                ViewBag.MenuPageName = getDocumentName();
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDt, ListFilterData);
                return RedirectToAction("AssetTransferDetail", "AssetTransfer", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AssetTransferDetail(UrlData urlData)
        {
            try
            {
                AssetTransfer_Model model = new AssetTransfer_Model();
                CommonPageDetails();
                GetCompDeatil();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Doc_date) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                model.Command = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.ListFilterData = urlData.ListFilterData;
                ViewBag.MenuPageName = getDocumentName();
                model.Title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
              
                GetAssetDescription(model, "A");
                GetSerialNo(model, "A", 0);
                model.DocNo = urlData.Doc_No;
                model.DocDate = urlData.Doc_date;
                model.TransferDate = DateTime.Now.ToString();

                List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                Lists.Add(new AssignedRequirementArea { AssignedRequirementArea_name = "---Select---", AssignedRequirementArea_id = "0" });
                model.AssignedRequirementAreaList = Lists;

                List<DestinationAssignedRequirementArea> DestinationLists = new List<DestinationAssignedRequirementArea>();
                DestinationLists.Add(new DestinationAssignedRequirementArea { DestinationAssignedRequirementArea_name = "---Select---", DestinationAssignedRequirementArea_id = "0" });
                model.DestinationAssignedRequirementAreaList = DestinationLists;
               // GetDestinationAssignedRequirementArea(model);
                if (urlData.Doc_No != null)
                {
                    DataSet ds = _AssetTransfer_ISERVICES.GetAssetTransferDetail(CompID, BrchID, model.DocNo, model.DocDate, UserID, model.DocumentMenuId);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            model.DocNo = ds.Tables[0].Rows[0]["assettrans_No"].ToString();  
                            model.DocDate = ds.Tables[0].Rows[0]["assettrans_date"].ToString();
                            model.TransferDate = ds.Tables[0].Rows[0]["transfer_date"].ToString();
                            model.AssetItemsId = ds.Tables[0].Rows[0]["asset_id"].ToString();
                            GetSerialNo(model, "A", 0);
                            model.SerialNumber = ds.Tables[0].Rows[0]["serial_no"].ToString();
                            model.AssetLabel = ds.Tables[0].Rows[0]["asset_label"].ToString();
                            model.AssignedRequirementAreaId = ds.Tables[0].Rows[0]["assign_req_area"].ToString();
                            model.AssignedRequirementAreaType = ds.Tables[0].Rows[0]["assign_req_area_type"].ToString();
                            model.DestinationAssignedRequirementAreaId = ds.Tables[0].Rows[0]["des_assign_req_area"].ToString();
                            model.DestinationAssignedRequirementAreaType = ds.Tables[0].Rows[0]["des_assign_req_area_type"].ToString();
                            model.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            model.StatusName = ds.Tables[0].Rows[0]["ATStatus"].ToString();
                            model.DocumentStatus = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                            model.Create_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            model.Create_on = ds.Tables[0].Rows[0]["CreatedOn"].ToString();
                            model.Amended_by = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            model.Amended_on = ds.Tables[0].Rows[0]["ModifiedOn"].ToString();
                            model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            model.Approved_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                            model.TransType = "Update";
                            ViewBag.AttechmentDetails = ds.Tables[1];
                            string doc_status = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                            string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                            model.DocumentStatus = doc_status;

                            //if (doc_status == "C")
                            //{
                            //    model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            //    model.CancelFlag = true;
                            //    model.BtnName = "Refresh";
                            //}
                            //else
                            //{
                            //    model.CancelFlag = false;
                            //}
                            model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[4];
                            }
                            if (ViewBag.AppLevel != null && model.Command != "Edit")
                            {
                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[4].Rows.Count > 0)
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
                                        model.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            model.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                        }
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        model.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }
                        else
                        {
                            //BindDDLOnPageLoad(model);
                        }
                    }
                    else
                    {
                        //  BindDDLOnPageLoad(model);
                    }
                }
                else
                {
                    // BindDDLOnPageLoad(model);
                }
                model.Title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = model.TransType;
                ViewBag.DocumentStatus = model.DocumentStatus;
                ViewBag.Command = model.Command;
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetTransfer/AssetTransferDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
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
        public ActionResult GetAssetDescription(AssetTransfer_Model _AssetRegistrationGroupDetail, string ShowFor)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetTransfer_ISERVICES.GetAssetItem(CompID, BrchID, ShowFor);
                List<AssetItemsLists> _COACList = new List<AssetItemsLists>();
                foreach (var data in AGAList)
                {
                    AssetItemsLists _COADetail = new AssetItemsLists();
                    _COADetail.AssetItem_id = data.Key;
                    _COADetail.AssetItem_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetItems = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssetDescriptionList(AssetTransferList_Model _AssetRegistrationGroupDetail, string ShowFor)
        {
            CommonPageDetails();
            Dictionary<string, string> AGAList = new Dictionary<string, string>();
            try
            {
                AGAList = _AssetTransfer_ISERVICES.GetAssetItem(CompID, BrchID, ShowFor);
                List<AssetItemsLists> _COACList = new List<AssetItemsLists>();
                foreach (var data in AGAList)
                {
                    AssetItemsLists _COADetail = new AssetItemsLists();
                    _COADetail.AssetItem_id = data.Key;
                    _COADetail.AssetItem_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetItems = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(AGAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSerialNo(AssetTransfer_Model _AssetRegistrationGroupDetail, string ShowFor, int RegId)
        {
            CommonPageDetails();
            Dictionary<string, string> SerialNoList = new Dictionary<string, string>();
            try
            {
                SerialNoList = _AssetTransfer_ISERVICES.GetSerialNo(CompID, BrchID, _AssetRegistrationGroupDetail.AssetItemsId, "A", RegId);
                List<AssetSerialNoLists> _COACList = new List<AssetSerialNoLists>();
                foreach (var data in SerialNoList)
                {
                    AssetSerialNoLists _COADetail = new AssetSerialNoLists();
                    _COADetail.SerialNo_id = data.Key;
                    _COADetail.SerialNo_name = data.Value;
                    _COACList.Add(_COADetail);
                }
                _AssetRegistrationGroupDetail.AssetSerialNo = _COACList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SerialNoList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSerialNoJS(AssetTransfer_Model _AssetTransfer_Model, string AssetDescriptionId)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                AssetTransferList_Model model = new AssetTransferList_Model();
                DataSet ds = _AssetTransfer_ISERVICES.GetSerialNoJs(CompID, BrchID, AssetDescriptionId, "E");

                List<AssetSerialNoLists> Lists = new List<AssetSerialNoLists>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssetSerialNoLists _COADetail = new AssetSerialNoLists();
                    _COADetail.SerialNo_id = data.Table.Columns["serial_noid"].ToString();
                    _COADetail.SerialNo_name = data.Table.Columns["serial_no"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetTransfer_Model.AssetSerialNo = Lists;
                DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetLabelJS(AssetTransfer_Model _AssetTransfer_Model, string AssetDescriptionId, string SerialNo)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                AssetTransferList_Model model = new AssetTransferList_Model();
                DataSet ds = _AssetTransfer_ISERVICES.GetLabelJs(CompID, BrchID, AssetDescriptionId, SerialNo);
                DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetTransferDetailsActionCommands(AssetTransfer_Model _model, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "Save":
                         SaveAssetTransferDetail(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.DocNo, _model.DocDate, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "Approve":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("AssetTransferDetail", urlData);
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string Invdt1 = _model.DocDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.DocNo, _model.DocDate, _model.ListFilterData);
                            return RedirectToAction("AssetTransferDetail", urlData);
                        }
                        ApproveATDetails(_model, _model.DocNo, _model.DocDate, "", "", "", "", "", "");
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.DocNo, _model.DocDate, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        GetCompDeatil();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _model.DocNo, DocDate = _model.DocDate, ListFilterData = _model.ListFilterData, WF_status = _model.WFStatus });
                        }
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.DocDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.DocNo, _model.DocDate, _model.ListFilterData);
                            return RedirectToAction("AssetTransferDetail", urlData);
                        }
                        /*End to chk Financial year exist or not*/

                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.Message = null;
                        Command = _model.Command;
                        TempData["ModelData"] = _model;
                        // TempData["FilterData"] = _model.FilterData1;
                        SetUrlData(urlData, _model.Command, "Update", _model.BtnName, _model.Message, _model.DocNo, _model.DocDate, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "Print":
                    ///return GenratePdfFile(_model);
                    case "Delete":
                        DeleteATDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetTransferDetail", urlData);
                    case "BacktoList":
                        TempData["WF_status"] = _model.WFStatus;
                        TempData["ListFilterData"] = _model.ListFilterData;
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData);
                        return RedirectToAction("AssetTransfer");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("AssetTransferDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApproveATDetails(AssetTransfer_Model _model, string Inv_No, string Inv_Date, string A_Status, string A_Level, string A_Remarks, string FilterData, string docid, string WF_Status1)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                // string Dn_Nurr = ""; _model.DN_Nurration == null ? Dn_Nurration : _model.DN_Nurration;
                string Result = _AssetTransfer_ISERVICES.ApproveATDetail(Inv_No, Inv_Date, DocumentMenuId, BrchID, CompID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                //ViewBag.PrintFormat = PrintFormatDataTable(_Model);
                //try
                //{
                //    //string fileName = "DPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                //    string fileName = Inv_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                //    var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId, "AP");
                //    _Common_IServices.SendAlertEmail(CompID, BrchID, "105101154", Inv_No, "AP", UserID, "", filePath);
                //}
                //catch (Exception exMail)
                //{
                //    _model.Message = "ErrorInMail";
                //    string path = Server.MapPath("~");
                //    Errorlog.LogError(path, exMail);
                //}
                if (Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                //_model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[4], FilterData);
                return RedirectToAction("AssetTransferDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void DeleteATDetail(AssetTransfer_Model _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _AssetTransfer_ISERVICES.DeleteATDetails(CompID, BrchID, _model.DocNo, _model.DocDate);
                _model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblAttachmentDetail(AssetTransfer_Model _model)
        {
            try
            {
                string PageName = _model.Title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                GetCompDeatil();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as AssetTransferattch;

                if (_model.Attatchmentdetail != null)
                {
                    if (_DirectPurchaseInvoiceattch != null)
                    {
                        if (_DirectPurchaseInvoiceattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _DirectPurchaseInvoiceattch.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_model.Attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_model.SerialNumber))
                            {
                                dtrowAttachment1["id"] = _model.SerialNumber;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_model.TransType == "Update")
                    {
                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_model.SerialNumber))
                            {
                                ItmCode = _model.SerialNumber;
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
                }
                return dtAttachment;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                AssetTransferattch _DirectPurchaseInvoiceattch = new AssetTransferattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _DirectPurchaseInvoiceattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string PageName = title.Replace(" ", "");
                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _DirectPurchaseInvoiceattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _DirectPurchaseInvoiceattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _DirectPurchaseInvoiceattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult SaveAssetTransferDetail(AssetTransfer_Model ObjAddItemGroupSetupBOL)
        {
            try
            {
                CommonPageDetails();
                string PageName = ObjAddItemGroupSetupBOL.Title.Replace(" ", "");
                if (Session["CompId"] != null)
                {
                    ObjAddItemGroupSetupBOL.CompId = CompID;
                    ObjAddItemGroupSetupBOL.BrdId = BrchID;
                    ObjAddItemGroupSetupBOL.Create_id = UserID;
                    var src_ra = ObjAddItemGroupSetupBOL.AssignedRequirementAreaId;
                    string[] parts = src_ra.Split('_');
                    ObjAddItemGroupSetupBOL.AssignedRequirementAreaId = parts[0];
                    ObjAddItemGroupSetupBOL.AssignedRequirementAreaType = parts[1];

                    var src_des = ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaId;
                    string[] parts_des = src_des.Split('_');
                    ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaId = parts_des[0];
                    ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaType = parts_des[1];

                    //var type = ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaType;
                    //ObjAddItemGroupSetupBOL.DestinationAssignedRequirementAreaType = type.ToLower() == "customer" ? "C" :
                    //  type.ToLower() == "supplier" ? "S" :
                    //  type.ToLower() == "employee" ? "E" :
                    //  type.ToLower() == "requirment area" ? "R" : type.ToUpper();
                    //var src_type = ObjAddItemGroupSetupBOL.AssignedRequirementAreaType;
                    //ObjAddItemGroupSetupBOL.AssignedRequirementAreaType = src_type.ToLower() == "customer" ? "C" :
                    //  src_type.ToLower() == "supplier" ? "S" :
                    //  src_type.ToLower() == "employee" ? "E" :
                    //  src_type.ToLower() == "requirment area" ? "R" : src_type.ToUpper();
                    if (ObjAddItemGroupSetupBOL.FormMode == "1")
                    {
                        ObjAddItemGroupSetupBOL.Create_id = UserID;
                    }
                }
                DataTable DtblAttchDetail = new DataTable();
                ObjAddItemGroupSetupBOL.Title = title;
                DtblAttchDetail = ToDtblAttachmentDetail(ObjAddItemGroupSetupBOL);
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as AssetTransferattch;
                // ObjAddItemGroupSetupBOL.AssetLife =(ObjAddItemGroupSetupBOL.AssetLife);
                TempData["ModelDataattch"] = null;
                
                if (ObjAddItemGroupSetupBOL.AssetItemsId.ToString() != "0")
                {
                    string SaveMessage = _AssetTransfer_ISERVICES.InsertAssetRegDetail(ObjAddItemGroupSetupBOL, mac_id, DtblAttchDetail);
                    if (SaveMessage == "DocModify")
                    {
                        ObjAddItemGroupSetupBOL.Message = "DocModify";
                        ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                        ObjAddItemGroupSetupBOL.Command = "Refresh";
                    }
                    else if (SaveMessage == "Cancelled")
                    {
                        try
                        {
                            //string fileName = ObjAddItemGroupSetupBOL.Doc_no.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            //var filePath = SavePdfDocToSendOnEmailAlert(ObjAddItemGroupSetupBOL.Doc_no, ObjAddItemGroupSetupBOL.Doc_date, fileName, DocumentMenuId, "C");
                            //_Common_IServices.SendAlertEmail(CompID, BrchID, "105106105", ObjAddItemGroupSetupBOL.Doc_no, "Cancel", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            ObjAddItemGroupSetupBOL.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        ObjAddItemGroupSetupBOL.Message = ObjAddItemGroupSetupBOL.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_model.Message = "Cancelled";
                        ObjAddItemGroupSetupBOL.Command = "Update";
                        ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                        ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                        ObjAddItemGroupSetupBOL.TransType = "Update";
                        //return RedirectToAction("DepreciationProcessAdd");
                    }
                    else
                    {
                        string[] FDetail = SaveMessage.Split(',');
                        string Message = FDetail[5].ToString();
                        string Inv_no = FDetail[0].ToString();
                        string Inv_DATE = FDetail[6].ToString();
                        string Cancel = FDetail[1].ToString();
                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            ObjAddItemGroupSetupBOL.Message = Message;
                            //return RedirectToAction("DepreciationProcessAdd");
                        }
                        if (Message == "Save")
                        {
                            //string Guid = "";
                            //if (_DirectPurchaseInvoiceattch != null)
                            //{
                            //    if (_DirectPurchaseInvoiceattch.Guid != null)
                            //    {
                            //        Guid = _DirectPurchaseInvoiceattch.Guid;
                            //    }
                            //}
                            string Guid = "";
                            if (_DirectPurchaseInvoiceattch != null)
                            {
                                if (_DirectPurchaseInvoiceattch.Guid != null)
                                {
                                    Guid = _DirectPurchaseInvoiceattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, ObjAddItemGroupSetupBOL.TransType, DtblAttchDetail);
                        }
                        if (Cancel == "C" && Message == "Update")
                        {
                            ObjAddItemGroupSetupBOL.Message = "Cancelled";
                            ObjAddItemGroupSetupBOL.Command = "Update";
                            ObjAddItemGroupSetupBOL.DocNo = Inv_no;
                            ObjAddItemGroupSetupBOL.DocDate = Inv_DATE;
                            ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                            ObjAddItemGroupSetupBOL.BtnName = "Refresh";
                            ObjAddItemGroupSetupBOL.TransType = "Update";
                        }
                        else
                        {
                            if (Message == "Update" || Message == "Save")
                             {
                                ObjAddItemGroupSetupBOL.Message = "Save";
                                ObjAddItemGroupSetupBOL.Command = "Update";
                                ObjAddItemGroupSetupBOL.DocNo = Inv_no;
                                ObjAddItemGroupSetupBOL.DocDate = Inv_DATE;
                                ObjAddItemGroupSetupBOL.DocumentStatus = "D";
                                ObjAddItemGroupSetupBOL.BtnName = "BtnSave";
                                ObjAddItemGroupSetupBOL.TransType = "Update";
                            }
                        }
                    }
                    TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                    return RedirectToAction("AddAssetTransferDetail", "AssetTransfer");
                }
                TempData["ModelData"] = ObjAddItemGroupSetupBOL;
                return RedirectToAction("AddAssetTransferDetail", "AssetTransfer");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string DocNo = null, string Doc_dt = null,
            string ListFilterData = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Doc_No = DocNo;
                urlData.Doc_date = Doc_dt;
                urlData.ListFilterData = ListFilterData;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        [HttpPost]
        public JsonResult GetAssignedRequirementArea(AssetTransfer_Model _AssetTransfer_Model)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet ds = _AssetTransfer_ISERVICES.GetAssignedRequirementArea(CompID, BrchID);

                List<AssignedRequirementArea> Lists = new List<AssignedRequirementArea>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    AssignedRequirementArea _COADetail = new AssignedRequirementArea();
                    _COADetail.AssignedRequirementArea_id = data.Table.Columns["acc_id"].ToString();
                    _COADetail.AssignedRequirementArea_name = data.Table.Columns["acc_name"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetTransfer_Model.AssignedRequirementAreaList = Lists;
                DataRows = Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public JsonResult GetDestinationAssignedRequirementArea(AssetTransfer_Model _AssetTransfer_Model)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet ds = _AssetTransfer_ISERVICES.GetAssignedRequirementArea(CompID, BrchID);
                List<DestinationAssignedRequirementArea> Lists = new List<DestinationAssignedRequirementArea>();
                foreach (DataRow data in ds.Tables[0].Rows)
                {
                    DestinationAssignedRequirementArea _COADetail = new DestinationAssignedRequirementArea();
                    _COADetail.DestinationAssignedRequirementArea_id = data.Table.Columns["acc_id"].ToString();
                    _COADetail.DestinationAssignedRequirementArea_name = data.Table.Columns["acc_name"].ToString();
                    Lists.Add(_COADetail);
                }
                _AssetTransfer_Model.DestinationAssignedRequirementAreaList = Lists;
                DataRows = Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            //Session["Message"] = "";
            AssetTransfer_Model _Model = new AssetTransfer_Model();
            var a = TrancType.Split(',');
            _Model.DocNo = a[0].Trim();
            _Model.DocDate = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WFStatus = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            UrlData URLModel = new UrlData();
            URLModel.Doc_No = a[0].Trim();
            URLModel.Doc_date = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.ListFilterData = ListFilterData1;
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["UrlData"] = URLModel;
            TempData["Message"] = _Model.Message;
            return RedirectToAction("AssetTransferDetail", URLModel);
        }
    }
}