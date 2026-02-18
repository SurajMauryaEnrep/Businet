using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;

using System.Globalization;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management
{ 
    public class DeliveryNoteListController : Controller
    {
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string FromDate, title;
        List<DeliveryNote> _DeliveryNoteList;
        string DocumentMenuId = "105102110";
        string CompID, BrchID, UserID = string.Empty, dn_no;
        DataTable dt;
        DeliveryNoteList_IServices _DeliveryNoteList_IServices;
        Common_IServices _Common_IServices;
        public DeliveryNoteListController(Common_IServices _Common_IServices, DeliveryNoteList_IServices _DeliveryNoteList_IServices)
        {
            this._DeliveryNoteList_IServices = _DeliveryNoteList_IServices;
            this._Common_IServices = _Common_IServices;
        }

        public ActionResult DeliveryNoteSearch(string SuppId, string SourceType, string Fromdate, string Todate, string Status)
        {
            try
            {
                //Session.Remove("WF_status");
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _DeliveryNoteList = new List<DeliveryNote>();
                dt = _DeliveryNoteList_IServices.GetDeliveryNoteSearch(SuppId, SourceType, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                 
                DeliveryNoteList_MODELS _DeliveryNoteList_MODELS = new DeliveryNoteList_MODELS();
                //Session["DNSearch"] = "DN_Search";
                _DeliveryNoteList_MODELS.DNSearch = "DN_Search";
                foreach (DataRow dr in dt.Rows)
                {
                    DeliveryNote _DeliveryNote = new DeliveryNote();
                    _DeliveryNote.DeliverNoteNumber = dr["dn_no"].ToString();
                    _DeliveryNote.DeliveryNoteDate = dr["dn_dt"].ToString();
                    _DeliveryNote.Dn_date = dr["Dn_date"].ToString();
                    _DeliveryNote.SupplierName = dr["supp_name"].ToString();
                    _DeliveryNote.CreatedON = dr["created_on"].ToString();
                    _DeliveryNote.ApprovedOn = dr["app_dt"].ToString();
                    _DeliveryNote.ModifiedOn = dr["mod_dt"].ToString();
                    _DeliveryNote.DeliveryNoteStatus = dr["dn_status"].ToString();
                    _DeliveryNote.SourceType = dr["SourceType"].ToString();
                    _DeliveryNote.src_doc_no = dr["src_doc_no"].ToString();
                    _DeliveryNote.src_doc_date = dr["src_doc_date"].ToString();
                    _DeliveryNote.create_by = dr["create_by"].ToString();
                    _DeliveryNote.app_by = dr["app_by"].ToString();
                    _DeliveryNote.mod_by = dr["mod_by"].ToString();
                    _DeliveryNote.curr_name = dr["curr_name"].ToString();
                    _DeliveryNote.BillNumber = dr["bill_no"].ToString();
                    _DeliveryNote.BillDate = dr["bill_date"].ToString();

                    _DeliveryNoteList.Add(_DeliveryNote);
                }
                _DeliveryNoteList_MODELS.DeliveryNoteList = _DeliveryNoteList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDeliveryNoteDetailList.cshtml", _DeliveryNoteList_MODELS);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult EditDeliveryNote(string DeliveryNoteNo, string DeliveryNoteDt, string ListFilterData, string WF_status)
        {
            DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS = new DeliveryNoteDetail_MODELS();
            /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/

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
            URlModelData _URlModelData = new URlModelData();
            _DeliveryNoteDetail_MODELS.Message = "New";
            _DeliveryNoteDetail_MODELS.Command = "Update";
            _DeliveryNoteDetail_MODELS.dn_no = DeliveryNoteNo;
            _DeliveryNoteDetail_MODELS.dn_dt = DeliveryNoteDt;
            _DeliveryNoteDetail_MODELS.TransType = "Update";
            _DeliveryNoteDetail_MODELS.AppStatus = "D";
            _DeliveryNoteDetail_MODELS.BtnName = "BtnEdit";
            _DeliveryNoteDetail_MODELS.WF_status1 = WF_status;
            TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
            TempData["ListFilterData"] = ListFilterData;
            _URlModelData.dn_no= DeliveryNoteNo;
            _URlModelData.dn_dt = DeliveryNoteDt;
            _URlModelData.TransType= "Update";
            _URlModelData.Command= "Update";
            _URlModelData.BtnName= "BtnEdit";
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["DeliveryNoteNo"] = DeliveryNoteNo;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnEdit";
            return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail", _URlModelData);
        }
        public ActionResult DeliveryNoteList(DeliveryNoteList_MODELS _DeliveryNoteList_MODELS)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //DeliveryNoteList_MODELS _DeliveryNoteList_MODELS = new DeliveryNoteList_MODELS();
                // GetAutoCompleteSupplierName(_DeliveryNoteList_MODELS);
                CommonPageDetails();
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                // ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                //_DeliveryNoteList_MODELS.StatusList = listOfStatus;
                List<Status> statusList = new List<Status>();
                if (ViewBag.StatusList.Rows.Count > 0)
                {
                    foreach (DataRow data in ViewBag.StatusList.Rows)
                    {
                        Status _Statuslist = new Status();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusList.Add(_Statuslist);
                    }
                }
                //statusList.Insert(0, new Status() { status_id = "0", status_name = "All" });
                _DeliveryNoteList_MODELS.StatusList = statusList;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                /*Commented By Nitesh 22072025 now from date */
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                ViewBag.DocumentMenuId = DocumentMenuId;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var SuppId = a[0].Trim();
                    var SourceType = a[1].Trim();
                    var Fromdate = a[2].Trim();
                    var Todate = a[3].Trim();
                    var Status = a[4].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                   
                    _DeliveryNoteList = new List<DeliveryNote>();
                  
                   // _DeliveryNoteList_MODELS.supp_id = SuppId;
                    _DeliveryNoteList_MODELS.SupplierID = SuppId;
                    _DeliveryNoteList_MODELS.Status = Status;
                    _DeliveryNoteList_MODELS.ToDate =Todate;
                    _DeliveryNoteList_MODELS.SourceType = SourceType;
                    _DeliveryNoteList_MODELS.FromDate = Fromdate;
                    //Session["DNSearch"] = "DN_Search";
                    _DeliveryNoteList_MODELS.DNSearch = "DN_Search";
                    _DeliveryNoteList_MODELS.ListFilterData = TempData["ListFilterData"].ToString();
                    _DeliveryNoteList_MODELS.flag = "Filter";
                }
                else
                {
                    _DeliveryNoteList_MODELS.FromDate = startDate;
                    _DeliveryNoteList_MODELS.ToDate = CurrentDate;
                    _DeliveryNoteList_MODELS.flag = "NotFilter";
                    // _DeliveryNoteList_MODELS.DeliveryNoteList = getDeliveryNoteListAll(_DeliveryNoteList_MODELS);
                }
                GetAllData(_DeliveryNoteList_MODELS);
                //ViewBag.MenuPageName = getDocumentName();
                _DeliveryNoteList_MODELS.Title = title;
                //Session["DNSearch"] = "0";
                _DeliveryNoteList_MODELS.DNSearch = "0";
              //  ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/DeliveryNote/DeliveryNoteList.cshtml", _DeliveryNoteList_MODELS);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        #region Created By Nitesh 03-04-2024 Function GetAllData For List AllData In One Procedure
        #endregion
        private void GetAllData(DeliveryNoteList_MODELS _DeliveryNoteList_MODELS) 
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
                if (string.IsNullOrEmpty(_DeliveryNoteList_MODELS.Spp_Name))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _DeliveryNoteList_MODELS.Spp_Name;
                }

                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DeliveryNoteList_MODELS.WF_status = TempData["WF_status"].ToString();
                    if (_DeliveryNoteList_MODELS.WF_status != null)
                    {
                        wfstatus = _DeliveryNoteList_MODELS.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    if (_DeliveryNoteList_MODELS.WF_status != null)
                    {
                        wfstatus = _DeliveryNoteList_MODELS.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }


                BrchID = Session["BranchId"].ToString();
                DataSet SuppList1 = _DeliveryNoteList_IServices.GetAllData(Comp_ID, Spp_Name, BrchID, dn_no, UserID, wfstatus, DocumentMenuId
                    , _DeliveryNoteList_MODELS.SupplierID, _DeliveryNoteList_MODELS.SourceType, _DeliveryNoteList_MODELS.FromDate, _DeliveryNoteList_MODELS.ToDate, _DeliveryNoteList_MODELS.Status);

                List<SupplierName> _SupplierNameList = new List<SupplierName>();
                foreach (DataRow dr in SuppList1.Tables[0].Rows)
                {
                    SupplierName _SupplierName = new SupplierName();
                    _SupplierName.supp_id = dr["supp_id"].ToString();
                    _SupplierName.supp_name = dr["supp_name"].ToString();
                    _SupplierNameList.Add(_SupplierName);
                }
                _SupplierNameList.Insert(0, new SupplierName() { supp_id="0",supp_name="All"});
                _DeliveryNoteList_MODELS.SupplierNameList = _SupplierNameList;
                if (_DeliveryNoteList_MODELS.SupplierID != null && _DeliveryNoteList_MODELS.SupplierID != "")
                    _DeliveryNoteList_MODELS.supp_id = _DeliveryNoteList_MODELS.SupplierID;

                    SetAllDataInListTable(_DeliveryNoteList_MODELS, SuppList1);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        private void SetAllDataInListTable(DeliveryNoteList_MODELS _DeliveryNoteList_MODELS,DataSet Data)
        {
            ViewBag.AttechmentDetails = Data.Tables[3];
            if (Data.Tables[2].Rows.Count > 0)
            {
                //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            DataTable dt = new DataTable();
            if(_DeliveryNoteList_MODELS.flag != "Filter" && _DeliveryNoteList_MODELS.flag== "NotFilter")
            {
                dt = Data.Tables[1];
            }
            else
            {
                dt = Data.Tables[4];
            }
            List<DeliveryNote> _DeliveryNoteLists = new List<DeliveryNote>();
            if (dt.Rows.Count > 0)
            {      
              
                foreach (DataRow dr in dt.Rows)
                {
                    DeliveryNote _DeliveryNote = new DeliveryNote();
                    _DeliveryNote.DeliverNoteNumber = dr["dn_no"].ToString();
                    _DeliveryNote.DeliveryNoteDate = dr["dn_dt"].ToString();
                    _DeliveryNote.Dn_date = dr["Dn_date"].ToString();
                    _DeliveryNote.SupplierName = dr["supp_name"].ToString();
                    _DeliveryNote.CreatedON = dr["created_on"].ToString();
                    _DeliveryNote.ApprovedOn = dr["app_dt"].ToString();
                    _DeliveryNote.ModifiedOn = dr["mod_dt"].ToString();
                    _DeliveryNote.DeliveryNoteStatus = dr["dn_status"].ToString();
                    _DeliveryNote.SourceType = dr["SourceType"].ToString();
                    _DeliveryNote.src_doc_no = dr["src_doc_no"].ToString();
                    _DeliveryNote.src_doc_date = dr["src_doc_date"].ToString();
                    _DeliveryNote.create_by = dr["create_by"].ToString();
                    _DeliveryNote.app_by = dr["app_by"].ToString();
                    _DeliveryNote.mod_by = dr["mod_by"].ToString();
                    _DeliveryNote.curr_name = dr["curr_name"].ToString();
                    _DeliveryNote.BillNumber = dr["bill_no"].ToString();
                    _DeliveryNote.BillDate = dr["bill_date"].ToString();

                    _DeliveryNoteLists.Add(_DeliveryNote);
                }
            }
            _DeliveryNoteList_MODELS.DeliveryNoteList = _DeliveryNoteLists;
        }
        private void CommonPageDetails()
        {
            try
            {
                string language = string.Empty;
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
        private DataTable GetRoleList()
        {
            try
            {
                string userid = string.Empty;
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
        public ActionResult AddNewDeliveryNote()
        {
            DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS = new DeliveryNoteDetail_MODELS();
            _DeliveryNoteDetail_MODELS.AppStatus = "D";
            _DeliveryNoteDetail_MODELS.BtnName = "BtnAddNew";
            _DeliveryNoteDetail_MODELS.TransType = "Save";
            _DeliveryNoteDetail_MODELS.Command = "New";
            TempData["ModelData"] = _DeliveryNoteDetail_MODELS;
            /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                //_DeliveryNoteDetail_MODELS.Message = "Financial Year not Exist";
                return RedirectToAction("DeliveryNoteList", _DeliveryNoteDetail_MODELS);
            }
            /*End to chk Financial year exist or not*/
            //Session["Message"] = null;
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnAddNew";
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            return RedirectToAction("DeliveryNoteDetail", "DeliveryNoteDetail");
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult GetAutoCompleteSupplierName(DeliveryNoteList_MODELS _DeliveryNoteList_MODELS)
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
                if (string.IsNullOrEmpty(_DeliveryNoteList_MODELS.Spp_Name))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _DeliveryNoteList_MODELS.Spp_Name;
                }
                BrchID = Session["BranchId"].ToString();
                SuppList = _DeliveryNoteList_IServices.GetSupplierListALl(Comp_ID, Spp_Name, BrchID);

                List<SupplierName> _SupplierNameList = new List<SupplierName>();
                foreach (var dr in SuppList)
                {
                    SupplierName _SupplierName = new SupplierName();
                    _SupplierName.supp_id = dr.Key;
                    _SupplierName.supp_name = dr.Value;
                    _SupplierNameList.Add(_SupplierName);
                }
                _DeliveryNoteList_MODELS.SupplierNameList = _SupplierNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        private List<DeliveryNote> getDeliveryNoteListAll(DeliveryNoteList_MODELS _DeliveryNoteList_MODELS)
        {
            try
            {
                
                _DeliveryNoteList = new List<DeliveryNote>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }               
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _DeliveryNoteList_MODELS.WF_status = TempData["WF_status"].ToString();
                    if (_DeliveryNoteList_MODELS.WF_status != null)
                    {
                        wfstatus = _DeliveryNoteList_MODELS.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    if (_DeliveryNoteList_MODELS.WF_status != null)
                    {
                        wfstatus = _DeliveryNoteList_MODELS.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                DataSet dt = new DataSet();
                dt = _DeliveryNoteList_IServices.GetDeliveryNoteListAll(dn_no,CompID, BrchID, UserID, wfstatus, DocumentMenuId);
                ViewBag.AttechmentDetails = dt.Tables[2];
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {
                   
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        DeliveryNote _DeliveryNote = new DeliveryNote();
                        _DeliveryNote.DeliverNoteNumber = dr["dn_no"].ToString();
                        _DeliveryNote.DeliveryNoteDate = dr["dn_dt"].ToString();
                        _DeliveryNote.Dn_date = dr["Dn_date"].ToString();
                        _DeliveryNote.SupplierName = dr["supp_name"].ToString();
                        _DeliveryNote.CreatedON = dr["created_on"].ToString();
                        _DeliveryNote.ApprovedOn = dr["app_dt"].ToString();
                        _DeliveryNote.ModifiedOn = dr["mod_dt"].ToString();
                        _DeliveryNote.DeliveryNoteStatus = dr["dn_status"].ToString();
                        _DeliveryNote.SourceType = dr["SourceType"].ToString();
                        _DeliveryNote.src_doc_no = dr["src_doc_no"].ToString();
                        _DeliveryNote.src_doc_date = dr["src_doc_date"].ToString();
                        _DeliveryNote.create_by = dr["create_by"].ToString();
                        _DeliveryNote.app_by = dr["app_by"].ToString();
                        _DeliveryNote.mod_by = dr["mod_by"].ToString();
                        _DeliveryNote.curr_name = dr["curr_name"].ToString();
                        _DeliveryNote.BillNumber = dr["bill_no"].ToString();
                        _DeliveryNote.BillDate = dr["bill_date"].ToString();
                        _DeliveryNoteList.Add(_DeliveryNote);
                    }
                }
                return _DeliveryNoteList;
            }
            catch (Exception Ex)
            
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult GetDeliveryNoteDashbrd(string docid, string status)
        {
            DeliveryNoteList_MODELS _DeliveryNoteList_MODELS = new DeliveryNoteList_MODELS();
            //Session["WF_status"] = status;
            _DeliveryNoteList_MODELS.WF_status = status;
            return RedirectToAction("DeliveryNoteList", _DeliveryNoteList_MODELS);
        }

    }
}