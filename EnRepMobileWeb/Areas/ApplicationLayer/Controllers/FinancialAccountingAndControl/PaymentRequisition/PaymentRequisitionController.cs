using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentRequisition;
using System.Data;
using System.Collections.Generic;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentRequisition;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Text;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.PaymentRequisition
{
    public class PaymentRequisitionController : Controller
    {
        string CompID, BrchID, DocumentName, UserID, language = String.Empty;
        string DocumentMenuId = "105104125", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PaymentRequisition_ISERVICES _PaymentRequisition_ISERVICES;
        public PaymentRequisitionController(Common_IServices _Common_IServices, PaymentRequisition_ISERVICES _PaymentRequisition_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._PaymentRequisition_ISERVICES = _PaymentRequisition_ISERVICES;
        }
        // GET: ApplicationLayer/PaymentRequisition
        public ActionResult PaymentRequisition(PaymentRequisitionModel Backtolist)
        {
            try
            {
                CompDataWithID();
                CommonPageDetails();
                PaymentRequisitionModel ListModel = new PaymentRequisitionModel();
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                ListModel.FromDate = startDate;
                ListModel.ToDate = CurrentDate;

                if (Backtolist.FilterDataList != null && Backtolist.FilterDataList.ToString() != "")
                {
                    var PRData = Backtolist.FilterDataList.ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        var req_area = a[0].Trim();
                        ListModel.req_area = req_area;
                        ListModel.FromDate = a[1].Trim();
                        ListModel.PR_status = a[3].Trim();
                        if (ListModel.PR_status == "0")
                        {
                            ListModel.PR_status = null;
                        }
                        ListModel.ToDate = a[2].Trim();
                        ListModel.FilterDataList = Backtolist.FilterDataList.ToString();
                    }
                    else
                    {
                        ListModel.FromDate = startDate;
                    }
                }
                else
                {
                    ListModel.FromDate = startDate;
                }
                ListModel.DashbordPendingStatus = Backtolist.DashbordPendingStatus;

                BindAllDropDownList(ListModel,"List");
             
                ListModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/PaymentRequisition/PaymentRequisitionList.cshtml", ListModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
         
         
           
           
        }
        public ActionResult DashboardPendingDocument(string Wfstatus)
        {
            PaymentRequisitionModel DashboardPending = new PaymentRequisitionModel();
            DashboardPending.DashbordPendingStatus = Wfstatus;
            return (RedirectToAction("PaymentRequisition", DashboardPending));
        }
        public void BindAllDropDownList(PaymentRequisitionModel ListModel,string flag)
        {
            DataSet Data = _PaymentRequisition_ISERVICES.GetAllData(CompID, BrchID, ListModel.req_area, ListModel.FromDate,
                ListModel.ToDate, ListModel.Status, UserID, ListModel.DashbordPendingStatus, DocumentMenuId, flag);
           
            List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
            //  dt = GetRequirmentreaList();
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                RequirementAreaList _RAList = new RequirementAreaList();
                _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                _RAList.req_val = dr["setup_val"].ToString();
                requirementAreaLists.Add(_RAList);
            }
          
            if(flag != "Detail")
            {
                requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "All" });
                ListModel._requirementAreaLists = requirementAreaLists;

                List<Status> list2 = new List<Status>();
                foreach (var dr in ViewBag.StatusList.Rows)
                {
                    Status Status = new Status();
                    Status.status_id = dr["status_code"].ToString();
                    Status.status_name = dr["status_name"].ToString();
                    list2.Add(Status);
                }
                ListModel.StatusList = list2;
                ListModel.SearchFlag = "";
                ViewBag.PaymentRequisitionDetailsList = Data.Tables[2];
            }
            else
            {
                requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                ListModel._requirementAreaLists = requirementAreaLists;


                List<CurrencyNameLIst> currencyNameLIst = new List<CurrencyNameLIst>();
               
                foreach (DataRow dr in Data.Tables[1].Rows)
                {
                    CurrencyNameLIst _CLList = new CurrencyNameLIst();
                    _CLList.curr_id = Convert.ToInt32(dr["curr_id"]);
                    _CLList.curr_name = dr["curr_name"].ToString();
                    currencyNameLIst.Add(_CLList);
                }
                currencyNameLIst.Insert(0, new CurrencyNameLIst() { curr_id = 0, curr_name = "---Select---" });
                ListModel._currencyNameList = currencyNameLIst;            
            }
          
           
        }
    
        public ActionResult AddPaymentRequisitionDetail()
        {
            PaymentRequisitionModel AddNewModel = new PaymentRequisitionModel();
            CompDataWithID();
            SetModelDataSwithchfeature(AddNewModel, "BtnAddNew", "Save", "New", "Add", "", "", "Enable","","");
            return RedirectToAction("PaymentRequisitionDetail", "PaymentRequisition", AddNewModel);
        }
        public ActionResult DblClick(string PRId, string PRDate, string PRData, string WF_status)
        {

            PaymentRequisitionModel Dblclick = new PaymentRequisitionModel();          
            SetModelDataSwithchfeature( Dblclick, "BtnToDetailPage", "Update",
            "New","Add", PRId, PRDate, "Disabled", PRData, WF_status);
          
            return (RedirectToAction("PaymentRequisitionDetail", Dblclick));
        }
        public ActionResult ToRefreshByJS(string PRData, string TrancType, string Mailerror)
        {
            try
            {
                PaymentRequisitionModel ForwardModel = new PaymentRequisitionModel();
                var a = TrancType.Split(',');
                SetModelDataSwithchfeature(ForwardModel, "BtnToDetailPage", a[2].Trim(),
               "New", "Add", a[0].Trim(), a[1].Trim(), "Disabled", PRData, a[3].Trim());

                return RedirectToAction("PaymentRequisitionDetail", ForwardModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult PaymentRequisitionDetail(PaymentRequisitionModel DeatilModel)
        {
            try
            {

                CompDataWithID();
                CommonPageDetails();
               
                BindAllDropDownList(DeatilModel, "Detail");
                if (DeatilModel.Req_date == null || DeatilModel.Req_date == "" && DeatilModel.RequisitionNumber == null || DeatilModel.RequisitionNumber == "")
                {
                    DeatilModel.Req_date = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    SetDataAfterSave(DeatilModel);
                }
               var MSG= TempData["Message"] as string;
                if (string.IsNullOrEmpty(MSG))
                {
                    DeatilModel.Message = "";
                }
                DeatilModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/PaymentRequisition/PaymentRequisitionDetail.cshtml", DeatilModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        private void SetDataAfterSave(PaymentRequisitionModel DeatilModel)
        {
            string pr_no = DeatilModel.RequisitionNumber;
            string pr_dt = DeatilModel.Req_date;

            DataSet ds = new DataSet();
             ds = _PaymentRequisition_ISERVICES.GetDetailData(CompID, BrchID, pr_no, pr_dt, UserID, DocumentMenuId);

            // ---- Safety Check ----
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return ; // or handle accordingly
            }

            // ---- Short References ----
            DataTable dtMain = ds.Tables[0];
            DataRow row = dtMain.Rows[0];

            // ---- Attachment ----
           
                ViewBag.AttechmentDetails = ds.Tables[1];
        

            // ---- Assign Model Values ----
            DeatilModel.RequisitionNumber = row["pr_no"]?.ToString();
            DeatilModel.Req_date = row["pr_dt"]?.ToString();
            DeatilModel.req_area = row["req_area"]?.ToString();
            DeatilModel.Req_By = row["req_by"]?.ToString();
            DeatilModel.remarks = row["remarks"]?.ToString();
            DeatilModel.Purpose = row["purpose"]?.ToString();
            DeatilModel.RequestedAmount = row["req_amt"]?.ToString();
            DeatilModel.Currency_name = row["curr"]?.ToString();
            DeatilModel.Currency_id = row["curr"]?.ToString();
            DeatilModel.CreatedBy = row["create_nm"]?.ToString();
            DeatilModel.CreatedOn = row["create_dt"]?.ToString();
            DeatilModel.ApprovedBy = row["app_nm"]?.ToString();
            DeatilModel.ApprovedOn = row["app_dt"]?.ToString();
            DeatilModel.AmmendedBy = row["mod_nm"]?.ToString();
            DeatilModel.AmmendedOn = row["mod_dt"]?.ToString();
            DeatilModel.Create_id = row["creator_id"]?.ToString();
            DeatilModel.PR_status = row["app_status"]?.ToString();
            DeatilModel.CancelledRemarks = row["cancel_remarks"]?.ToString();

            // ---- Local Variables (Single Access) ----
            string approval_id = row["approval_id"]?.ToString();
            string create_id = row["creator_id"]?.ToString();
            string doc_status = row["status_code"]?.ToString()?.Trim();

            // ---- Document Status ----
            DeatilModel.doc_status = doc_status;
            DeatilModel.DocumentStatus = doc_status;

            // ---- Cancel Flag ----
            if (doc_status == "C")
            {
                DeatilModel.CancelFlag = true;
                DeatilModel.BtnName = "Refresh";
            }
            else
            {
                DeatilModel.CancelFlag = false;
            }


            // ---- Workflow JSON ----
            DeatilModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
            DeatilModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

            // ---- App Level ----
            if (doc_status != "D" && doc_status != "F")
            {
                ViewBag.AppLevel = ds.Tables[4];
            }

            if (ViewBag.AppLevel != null && DeatilModel.Command != "Edit")
            {
                string sent_to = ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["sent_to"].ToString() : "";
                string nextLevel = ds.Tables[3].Rows.Count > 0 ? ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim() : "";

                bool isCreator = create_id == UserID;
                bool isApprover = approval_id == UserID;
                bool isSentToUser = UserID == sent_to;
                bool isFinalLevel = nextLevel == "0";

                switch (doc_status)
                {
                    case "D":

                        if (!isCreator)
                        {
                            DeatilModel.BtnName = "Refresh";
                            break;
                        }

                        if (isFinalLevel)
                        {
                            if (isCreator)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                            }
                        }
                        else
                        {
                            ViewBag.Approve = "N";
                            ViewBag.ForwardEnbl = "Y";
                        }

                        if (isSentToUser)
                        {
                            ViewBag.ForwardEnbl = "Y";
                        }

                        if (isFinalLevel && isSentToUser)
                        {
                            ViewBag.Approve = "Y";
                            ViewBag.ForwardEnbl = "N";
                        }

                        DeatilModel.BtnName = "BtnToDetailPage";
                        break;

                    case "F":

                        if (isSentToUser)
                        {
                            ViewBag.ForwardEnbl = "Y";
                        }

                        if (isFinalLevel)
                        {
                            if (isSentToUser)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                            }
                        }

                        DeatilModel.BtnName = "BtnToDetailPage";
                        break;

                    case "A":

                        if (isCreator || isApprover)
                        {
                            DeatilModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            DeatilModel.BtnName = "Refresh";
                        }

                        break;
                }
            }

            // ---- Safe AppLevel Count Check ----
            if (ViewBag.AppLevel != null && ViewBag.AppLevel.Rows.Count == 0)
            {
                ViewBag.Approve = "Y";
            }


            DeatilModel.UserID = UserID;
            DeatilModel.Title = title;
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
        private void CommonPageDetails()
        {
            try
            {

                CompDataWithID();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
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

        public ActionResult PaymentRiquisitionSave(PaymentRequisitionModel SaveModel, string command)
        {
            try
            {
                CompDataWithID();
                var commCont = new CommonController(_Common_IServices);

                switch (command)
                {
                    case "AddNew":
                        PaymentRequisitionModel AddNewModel = new PaymentRequisitionModel();
                       
                        SetModelDataSwithchfeature(AddNewModel, "BtnAddNew", "Save", "New", "Add", "", "", "Enable","","");
                        return RedirectToAction("PaymentRequisitionDetail", AddNewModel);
                    case "Save":
                        if (SaveModel.TransType == null)
                        {
                            SaveModel.TransType = command;
                        }
                        SaveUpdateData(SaveModel);
                        PaymentRequisitionModel NewSaveModel = new PaymentRequisitionModel();
                        SetModelDataSwithchfeature(NewSaveModel, SaveModel.BtnName, "Update", SaveModel.Message, "Update", SaveModel.RequisitionNumber, 
                            SaveModel.Req_date, "Disabled", SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        
                        return RedirectToAction("PaymentRequisitionDetail", NewSaveModel);
                    case "Delete":
                        DeleteData(SaveModel, command);
                        PaymentRequisitionModel DeleteModel = new PaymentRequisitionModel();
                        SetModelDataSwithchfeature(DeleteModel, "BtnRefresh", "Refresh", "Deleted", command, "", "", "Disabled", 
                            SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        return RedirectToAction("PaymentRequisitionDetail", DeleteModel);
                    case "Edit":
                        SetModelDataSwithchfeature(SaveModel, "BtnEdit", "Update", "New", command, SaveModel.RequisitionNumber,
                            SaveModel.Req_date, "Enabled", SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        return RedirectToAction("PaymentRequisitionDetail", SaveModel);
                    case "Approve":
                        SaveModel.Command = command;                     
                        DocumentApprove(SaveModel, "", "", "", SaveModel.DashbordPendingStatus, SaveModel.FilterDataList);
                        SetModelDataSwithchfeature(SaveModel, "BtnSave", "Approved", "Approved", command, SaveModel.RequisitionNumber,
                            SaveModel.Req_date, "Disabled", SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        return (RedirectToAction("PaymentRequisitionDetail", SaveModel));
                    case "Print":
                        return (RedirectToAction("PaymentRequisitionDetail", SaveModel));
                    case "Refresh":                       
                        PaymentRequisitionModel DisabledModel = new PaymentRequisitionModel();
                        SetModelDataSwithchfeature(DisabledModel, "BtnRefresh", "", "New", command,"","","Disabled", 
                            SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        return RedirectToAction("PaymentRequisitionDetail", DisabledModel);
                    case "BacktoList":
                        PaymentRequisitionModel Backtolist = new PaymentRequisitionModel();                      
                        SetModelDataSwithchfeature(Backtolist, "", "", "", "","","", "", SaveModel.FilterDataList, SaveModel.DashbordPendingStatus);
                        return RedirectToAction("PaymentRequisition", Backtolist);
                    default:
                        return new EmptyResult();
                }
               // return RedirectToAction("PaymentRequisitionDetail", SaveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DocumentApprove(PaymentRequisitionModel ApproveModel,string A_Status, string A_Level, string A_Remarks, string FilterDataList, string DashbordPendingStatus)
        {
            try

            {
                CompDataWithID();
                //PurchaseRequisition_Model _PRModel = new PurchaseRequisition_Model();
                ApproveModel.CreatedBy = UserID;
                string PRDate2 = DateTime.TryParse(ApproveModel.Req_date, out DateTime dt)
                           ? dt.ToString("yyyy-MM-dd")
                           : "";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _PaymentRequisition_ISERVICES.DocumentApprove(ApproveModel, CompID, BrchID, PRDate2, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                string PRNo = Message.Split(',')[1].Trim();
                string PRDate = Message.Split(',')[2].Trim();
                SetModelDataSwithchfeature(ApproveModel, "BtnSave", "Update", "Approved", "Approve", ApproveModel.RequisitionNumber, PRDate, "Disabled", FilterDataList, DashbordPendingStatus);
                return RedirectToAction("PaymentRequisitionDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public void SetModelDataSwithchfeature(PaymentRequisitionModel SaveModel, string Btn , string TransType,
            string Message, string command, string Pay_no, string Pay_dt, string DisabledPage,string PRData,string WF_status)
        {
            try
            {
                SaveModel.BtnName = Btn;
                SaveModel.TransType = TransType;
                SaveModel.Message = Message;
                SaveModel.Command = command;
                SaveModel.RequisitionNumber = Pay_no;
                SaveModel.Req_date = Pay_dt;
                SaveModel.DisabledPage = DisabledPage;
                SaveModel.WF_status = WF_status;
                SaveModel.DashbordPendingStatus = WF_status;
                SaveModel.FilterDataList = PRData;             
                TempData["Message"] = Message;
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
        }
        public ActionResult SaveUpdateData(PaymentRequisitionModel SaveModel)
        {
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");
            try
            {
                if(SaveModel.CancelFlag == false)
                {
                    DataTable PRHeader = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("user_id", typeof(int));
                    dtheader.Columns.Add("pr_dt", typeof(DateTime));
                    dtheader.Columns.Add("pr_no", typeof(string));
                    dtheader.Columns.Add("req_area", typeof(int));
                    dtheader.Columns.Add("req_by", typeof(string));
                    dtheader.Columns.Add("purpose", typeof(string));
                    dtheader.Columns.Add("Req_amt", typeof(string));
                    dtheader.Columns.Add("Curr", typeof(string));
                    dtheader.Columns.Add("pr_status", typeof(string));
                    dtheader.Columns.Add("remarks", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();

                    dtrowHeader["TransType"] = SaveModel.TransType;
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["comp_id"] = CompID;
                    dtrowHeader["br_id"] = BrchID;
                    dtrowHeader["user_id"] = UserID;
                    dtrowHeader["pr_no"] = SaveModel.RequisitionNumber;
                    dtrowHeader["pr_dt"] = SaveModel.Req_date;
                    dtrowHeader["req_area"] = SaveModel.req_area;
                    dtrowHeader["req_by"] = SaveModel.Req_By;
                    dtrowHeader["purpose"] = SaveModel.Purpose;
                    dtrowHeader["Req_amt"] = SaveModel.RequestedAmount;
                    dtrowHeader["curr"] = SaveModel.Currency_id;
                    dtrowHeader["pr_status"] = "D";
                    dtrowHeader["remarks"] = SaveModel.remarks;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;

                    dtheader.Rows.Add(dtrowHeader);
                    PRHeader = dtheader;

                    DataTable dtAttachment = new DataTable();
                    var _PurchaseRequisitionattch = TempData["ModelDataattch"] as PaymentRequisitionattch;
                    TempData["ModelDataattch"] = null;
                    if (SaveModel.attatchmentdetail != null)
                    {
                        if (_PurchaseRequisitionattch != null)
                        {
                            if (_PurchaseRequisitionattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _PurchaseRequisitionattch.AttachMentDetailItmStp as DataTable;
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
                            if (SaveModel.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = SaveModel.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(SaveModel.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(SaveModel.RequisitionNumber))
                                {
                                    dtrowAttachment1["id"] = SaveModel.RequisitionNumber;
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
                        if (SaveModel.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(SaveModel.RequisitionNumber))
                                {
                                    ItmCode = SaveModel.RequisitionNumber;
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

                    SaveMessage = _PaymentRequisition_ISERVICES.InsertUpdatePR(PRHeader, Attachments);

                    string[] Data = SaveMessage.Split(',');


                    string Message = Data[0];
                    string PRNo = Data[1];
                    string PRDate = Data[2];
                    if (Message == "DataNotFound")
                    {

                        var a = PRNo.Split(',');
                        var msg = "Data Not found" + " " + a[0].Trim() + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        SaveModel.Message = Message;
                        return RedirectToAction("PaymentRequisitionDetail", SaveModel);
                    }

                    if (Message == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_PurchaseRequisitionattch != null)
                        {
                            if (_PurchaseRequisitionattch.Guid != null)
                            {
                                //Guid = Session["Guid"].ToString();
                                Guid = _PurchaseRequisitionattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, PRNo, SaveModel.TransType, Attachments);


                    }
                    if (Message == "Update" || Message == "Save")
                    {

                        SetModelDataSwithchfeature(SaveModel, "BtnSave", "Update", "Save", "Update", PRNo, PRDate, "Disabled", SaveModel.FilterDataList, "");
                        SaveModel.AttachMentDetailItmStp = null;
                        SaveModel.Guid = null;

                    }
                }
                else
                {
                    SaveModel.CreatedBy = UserID;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = _PaymentRequisition_ISERVICES.CancelDocument(SaveModel, CompID, BrchID, mac_id);
                 
                    string ApMessage = SaveMessage.Split(',')[0].Trim();
                    string PRNo = SaveMessage.Split(',')[1].Trim();
                    string PRDate = SaveMessage.Split(',')[2].Trim();
                    SetModelDataSwithchfeature(SaveModel, "BtnRefresh", "Update", ApMessage, ApMessage, SaveModel.RequisitionNumber, PRDate,
                        "Disabled",SaveModel.FilterDataList,SaveModel.DashbordPendingStatus);
                }
              
                return RedirectToAction("PaymentRequisitionDetail", SaveModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (SaveModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (SaveModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = SaveModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
                
        }

        private ActionResult DeleteData(PaymentRequisitionModel DeleteModel, string command)
        {
            try
            {
                CompDataWithID();

                string doc_no = DeleteModel.RequisitionNumber;
                string Message = _PaymentRequisition_ISERVICES.DeleteData(DeleteModel, CompID, BrchID, DocumentMenuId);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    CommonPageDetails();
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, doc_no1, Server);
                }           
                SetModelDataSwithchfeature(DeleteModel, "BtnRefresh", "Refresh", "Deleted", command, "", "", "Disabled", DeleteModel.FilterDataList, "");
                return RedirectToAction("PaymentRequisitionDetail", DeleteModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            CompDataWithID();
            CommonPageDetails();
            try
            {
                PaymentRequisitionattch _PurchaseRequisitionattch = new PaymentRequisitionattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                //string TransType = "";
                //string PRNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
               
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
               
                _PurchaseRequisitionattch.Guid = DocNo;
              
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                   
                    _PurchaseRequisitionattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    
                    _PurchaseRequisitionattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _PurchaseRequisitionattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
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
        [HttpPost]
        public ActionResult SearchDetail(string req_area, string Fromdate, string Todate, string Status)
        {
            try
            {
                CompDataWithID();
       
                PaymentRequisitionModel SerachModel = new PaymentRequisitionModel();            
                DataTable dt1 = _PaymentRequisition_ISERVICES.GetAllData(CompID, BrchID, req_area, Fromdate, Todate, Status, UserID, "", DocumentMenuId, "List").Tables[2];

                ViewBag.PaymentRequisitionDetailsList = dt1;
                SerachModel.SearchFlag = "SearchData";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPaymentRequisitionList.cshtml", SerachModel);
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