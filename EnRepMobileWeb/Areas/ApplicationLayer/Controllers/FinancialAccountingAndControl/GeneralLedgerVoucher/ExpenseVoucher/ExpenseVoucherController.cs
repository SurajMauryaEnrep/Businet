using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
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
using static EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher.ExpenseVoucherList_Model;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher
{
    public class ExpenseVoucherController : Controller
    {
        string CompID, Br_ID, UserID, language = String.Empty;
        string DocumentMenuId = "105104115147", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ExpenseVoucher_ISERVICE _ExpenseVoucher_ISERVICE;
        DataTable dt;
        public ExpenseVoucherController(Common_IServices _Common_IServices, ExpenseVoucher_ISERVICE ExpenseVoucher_Iservice)
        {
            this._Common_IServices = _Common_IServices;
            this._ExpenseVoucher_ISERVICE = ExpenseVoucher_Iservice;
           
        }
        // GET: ApplicationLayer/ExpenseVoucher
        public ActionResult ExpenseVoucher(ExpenseVoucherList_Model _ExpVouListModel)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            CommonPageDetails();
            ViewBag.DocumentMenuId = DocumentMenuId;

            List<StatusList> statusLists = new List<StatusList>();
            if (ViewBag.StatusList.Rows.Count > 0)
            {
                foreach (DataRow data in ViewBag.StatusList.Rows)
                {
                    StatusList _Statuslist = new StatusList();
                    _Statuslist.status_id = data["status_code"].ToString();
                    _Statuslist.status_name = data["status_name"].ToString();
                    statusLists.Add(_Statuslist);
                }
            }
            _ExpVouListModel._StatusLists = statusLists;
            //var other = new CommonController(_Common_IServices);
            //ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);

            //List<StatusList> statusLists = new List<StatusList>();
            //var statusListsC = other.GetStatusList1(DocumentMenuId);
            //var listOfStatus = statusListsC.ConvertAll(x => new StatusList { status_id = x.status_id, status_name = x.status_name });
            //_ExpVouListModel._StatusLists = listOfStatus;
            DataTable dt = GlAccountList();
            List<PayeeAccLst> Payeelst = new List<PayeeAccLst>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PayeeAccLst _Lst = new PayeeAccLst();
                    _Lst.acc_id = dr["acc_id"].ToString();
                    _Lst.acc_name = dr["acc_name"].ToString();
                    Payeelst.Add(_Lst);
                }
            }
            Payeelst.Insert(0, new PayeeAccLst() { acc_id = "0", acc_name = "All" });
            _ExpVouListModel.PaAccLst = Payeelst;
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            DataSet Ds = new DataSet();
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                if (ListFilterData != null && ListFilterData != "")
                {
                    var a = ListFilterData.Split(',');
                    _ExpVouListModel.FromDate = a[0].Trim();
                    _ExpVouListModel.ToDate = a[1].Trim();
                    _ExpVouListModel.EXvou_Status = a[2].Trim();
                    _ExpVouListModel.Payeeacc = a[3].Trim();

                    _ExpVouListModel.ListFilterData = TempData["ListFilterData"].ToString();

                    Ds = _ExpenseVoucher_ISERVICE.SerachListExpenseVoucher(CompID, Br_ID, _ExpVouListModel.Payeeacc, _ExpVouListModel.FromDate, _ExpVouListModel.ToDate, _ExpVouListModel.EXvou_Status);
                    //_ExpVouListModel.ExpVouSearch = "VouSearch";
                }
            }
            else
            {
                if (_ExpVouListModel.FromDate == null)
                {
                    _ExpVouListModel.FromDate = startDate;
                }
                var FromDate = _ExpVouListModel.FromDate;
                var ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                Ds = _ExpenseVoucher_ISERVICE.GetExpenseVouList(CompID, Br_ID, FromDate, ToDate);

                _ExpVouListModel.FromDate = Ds.Tables[1].Rows[0]["finstrdate"].ToString();
            }          
            List<ExVouList> VouLst = new List<ExVouList>();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    ExVouList Lst = new ExVouList();
                    Lst.Vou_No = dr["vou_no"].ToString();
                    Lst.Vou_Dt = dr["vou_dt"].ToString();
                    Lst.Vou_Date = dr["Vou_Date"].ToString();
                    Lst.acc_id = dr["acc_id"].ToString();
                    Lst.PayAmt = dr["PaymentAmt"].ToString();
                    Lst.ExpAmt = dr["ExpenseAmt"].ToString();
                    Lst.Vou_Status = dr["ExpVou_Status"].ToString();
                    Lst.CreateBy = dr["create_by"].ToString();
                    Lst.CreateDt = dr["CreateDate"].ToString();
                    Lst.Approve_by = dr["app_by"].ToString();
                    Lst.Approve_Date = dr["ApproveDate"].ToString();
                    Lst.Modify_By = dr["ModifyBy"].ToString();
                    Lst.Modify_Date = dr["ModifyDate"].ToString();
                    VouLst.Add(Lst);
                }
            }
            _ExpVouListModel.VouList = VouLst;
            _ExpVouListModel.Title = title;
            //ViewBag.MenuPageName = getDocumentName();
            //ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/ExpenseVoucher/ExpenseVoucherList.cshtml", _ExpVouListModel);
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        public ActionResult AddExpenseVoucherDetail(string ListFilterData1)
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            UrlData urlData = new UrlData();
            string BtnName = "BtnAddNew";
            string TransType = "Save";
            SetUrlData(urlData, "Add", TransType, BtnName,null,null, null, ListFilterData1,null,null);
            /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
            
            //if (Session["CompId"] != null)
            //    CompID = Session["CompId"].ToString();
            //if (Session["BranchId"] != null)
            //    Br_ID = Session["BranchId"].ToString();
            //DateTime dtnow = DateTime.Now;
            //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
            //var commCont = new CommonController(_Common_IServices);

            //string MsgNew = string.Empty;
            //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
            //if (MsgNew == "FY Not Exist")
            //{
            //    TempData["Message"] = "Financial Year not Exist";
            //    return RedirectToAction("ExpenseVoucher", urlData);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("ExpenseVoucher", urlData);
            //}
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("ExpenseVoucherDetail", "ExpenseVoucher", urlData);
        }

        public ActionResult ExpenseVoucherDetail(UrlData urlData)
        {
            try
            {
                ExpenseVoucher_Model expenseVoucher_Model = new ExpenseVoucher_Model();
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
                    UserID = Session["userid"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    expenseVoucher_Model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                expenseVoucher_Model.BtnCommand = urlData.Command;
                expenseVoucher_Model.TransType = urlData.TransType;
                expenseVoucher_Model.BtnName = urlData.BtnName;
                expenseVoucher_Model.Vou_No = urlData.Vou_No;
                //expenseVoucher_Model.Vou_No = urlData.Vou_No;
                expenseVoucher_Model.GLVoucherNo = urlData.gl_Vou_No;
                expenseVoucher_Model.GLVoucherDt = urlData.gl_Vou_Dt;

                if (urlData.Vou_Dt != null && urlData.Vou_Dt != "")
                {
                    expenseVoucher_Model.Vou_Date = urlData.Vou_Dt;
                }

                expenseVoucher_Model.ListFilterData1 = urlData.ListFilterData1;

                DataSet ds = _ExpenseVoucher_ISERVICE.GetExpenseVouDetails(CompID, Br_ID, UserID, expenseVoucher_Model.Vou_No, expenseVoucher_Model.Vou_Date);

                List<PayeeGlAccList> lst = new List<PayeeGlAccList>();
                dt = GlAccountList();
                foreach (DataRow dr in dt.Rows)
                {
                    PayeeGlAccList ls = new PayeeGlAccList();
                    ls.Payee_acc_id = dr["acc_id"].ToString();
                    ls.payee_acc_name = dr["acc_name"].ToString();
                    lst.Add(ls);
                }
                lst.Insert(0, new PayeeGlAccList() { Payee_acc_id = "0", payee_acc_name = "---Select---" });
                expenseVoucher_Model.payeeGlAccLists = lst;
                if (ds.Tables.Count > 0)
                {
                    expenseVoucher_Model.AccountDetail = ds.Tables[3].Rows[0]["ClosBL"].ToString();
                    expenseVoucher_Model.TotalPay = ds.Tables[1].Rows[0]["tot_pay_amt"].ToString();
                    expenseVoucher_Model.TotalAdj = ds.Tables[1].Rows[0]["tot_adj_amt"].ToString();
                    expenseVoucher_Model.TotalUnAdj = ds.Tables[1].Rows[0]["tot_unadj_amt"].ToString();
                    expenseVoucher_Model.TotalPend = ds.Tables[1].Rows[0]["tot_pending_amt"].ToString();
                    expenseVoucher_Model.TotalExp = ds.Tables[2].Rows[0]["TotalExp"].ToString();

                    ViewBag.AttechmentDetails = ds.Tables[7];

                    expenseVoucher_Model.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                    ViewBag.GLVoucherDetail = ds.Tables[8];
                    ViewBag.CostCenterData = ds.Tables[9];
                    ViewBag.GLVoucherTotalDetail = ds.Tables[10];
                }
                
                SetExpenseVouModelDetails(expenseVoucher_Model, ds);
                ViewBag.MenuPageName = getDocumentName();
                expenseVoucher_Model.Title = title;
                ViewBag.VBRoleList = GetRoleList();
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/ExpenseVoucher/ExpenseVoucherDetail.cshtml", expenseVoucher_Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetExpenseVouModelDetails(ExpenseVoucher_Model _model, DataSet Ds)
        {

            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            
            _model.DocumentMenuId = DocumentMenuId;
            if (Ds.Tables.Count > 0)
            {
                _model.Vou_No = Ds.Tables[0].Rows[0]["vou_no"].ToString();
                _model.Vou_Date = Ds.Tables[0].Rows[0]["vou_dt"].ToString();
                _model.PayeeGlList = Ds.Tables[0].Rows[0]["acc_id"].ToString();
                _model.Create_Id = Ds.Tables[0].Rows[0]["creator_id"].ToString();
                _model.Create_by = Ds.Tables[0].Rows[0]["creator_nm"].ToString();
                _model.Create_Dt = Ds.Tables[0].Rows[0]["create_dt"].ToString();
                _model.Amended_by = Ds.Tables[0].Rows[0]["mod_nm"].ToString();
                _model.Amended_on = Ds.Tables[0].Rows[0]["mod_dt"].ToString();
                _model.Approved_by = Ds.Tables[0].Rows[0]["app_nm"].ToString();
                _model.Approved_on = Ds.Tables[0].Rows[0]["app_dt"].ToString();
                _model.DocumentStatus = Ds.Tables[0].Rows[0]["status_code"].ToString();
                _model.Status = Ds.Tables[0].Rows[0]["app_status"].ToString();
                
                string create_id = Ds.Tables[0].Rows[0]["creator_id"].ToString();
               
                string Statuscode = Ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                string approval_id = Ds.Tables[0].Rows[0]["approval_id"].ToString();

                // Passing Supplier Table To view
                ViewBag.PaymentDetail = Ds.Tables[1];
                ViewBag.ExpenseDesc = Ds.Tables[2];

                if (Ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                {
                    _model.CancelledRemarks = Ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                    _model.CancelFlag = true;
                    _model.BtnName = "Refresh";
                }
                else
                {
                    _model.CancelFlag = false;
                }

                if (Statuscode == "A" || Statuscode == "C")
                {
                    if (Ds.Tables[8].Rows.Count > 0)
                    {
                        _model.GLVoucherType = Ds.Tables[8].Rows[0]["vou_type"].ToString();
                        _model.GLVoucherNo = Ds.Tables[8].Rows[0]["vou_no"].ToString();
                        _model.GLVoucherDt = Ds.Tables[8].Rows[0]["vou_dt"].ToString();
                    }
                }

                //For WorkFlow...
                _model.WFBarStatus = DataTableToJSONWithStringBuilder(Ds.Tables[6]);
                _model.WFStatus = DataTableToJSONWithStringBuilder(Ds.Tables[5]);
                if (ViewBag.AppLevel != null && _model.BtnCommand != "Edit")
                {

                    var sent_to = "";
                    var nextLevel = "";
                    if (Ds.Tables[4].Rows.Count > 0)
                    {
                        sent_to = Ds.Tables[4].Rows[0]["sent_to"].ToString();
                    }

                    if (Ds.Tables[5].Rows.Count > 0)
                    {
                        nextLevel = Ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                    }

                    if (Statuscode == "D")
                    {
                        if (create_id != UserID)
                        {
                            _model.BtnName = "Refresh";
                        }
                        else
                        {
                            if (nextLevel == "0")
                            {
                                if (create_id == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message"] != null)
                                    {
                                        ViewBag.Message = TempData["Message"];
                                    }
                                    if (TempData["FBMessage"] != null)
                                    {
                                        ViewBag.MessageFB = TempData["FBMessage"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                _model.BtnName = "BtnToDetailPage";
                            }
                            else
                            {
                                ViewBag.Approve = "N";
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message"] != null)
                                {
                                    ViewBag.Message = TempData["Message"];
                                   // _model.BtnName = "Refresh";
                                }
                                if (TempData["FBMessage"] != null)
                                {
                                    ViewBag.MessageFB = TempData["FBMessage"];
                                    //_model.BtnName = "Refresh";
                                }
                                //else
                                //{
                                //    _model.BtnName = "BtnToDetailPage";
                                //}
                                 _model.BtnName = "BtnToDetailPage";
                            }

                        }
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            _model.BtnName = "BtnToDetailPage";
                        }


                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message"] != null)
                                {
                                    ViewBag.Message = TempData["Message"];
                                }
                                if (TempData["FBMessage"] != null)
                                {
                                    ViewBag.MessageFB = TempData["FBMessage"];
                                }
                                /*End to chk Financial year exist or not*/
                                _model.BtnName = "BtnToDetailPage";
                            }


                        }
                    }
                    if (Statuscode == "F")
                    {
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            _model.BtnName = "BtnToDetailPage";
                        }
                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message"] != null)
                                {
                                    ViewBag.Message = TempData["Message"];
                                }
                                if (TempData["FBMessage"] != null)
                                {
                                    ViewBag.MessageFB = TempData["FBMessage"];
                                }
                                /*End to chk Financial year exist or not*/
                            }
                            _model.BtnName = "BtnToDetailPage";
                        }
                    }
                    if (Statuscode == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _model.BtnName = "BtnToDetailPage";

                        }
                        else
                        {
                            _model.BtnName = "Refresh";
                        }
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                    /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                    if (TempData["Message"] != null)
                    {
                        ViewBag.Message = TempData["Message"];
                    }
                    if (TempData["FBMessage"] != null)
                    {
                        ViewBag.MessageFB = TempData["FBMessage"];
                    }
                    /*End to chk Financial year exist or not*/
                }
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
        public DataTable GlAccountList()
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
                DataTable dt = _ExpenseVoucher_ISERVICE.GetGLAccList(CompID, Br_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetExpenseGlAccount(string SearchName)
        {
            try
            {
                Dictionary<string, string> ExpenseAcc = new Dictionary<string, string>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet ExpenseAcclst = _ExpenseVoucher_ISERVICE.GetExpenseAcc(CompID, Br_ID, SearchName);
                if (ExpenseAcclst.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < ExpenseAcclst.Tables[0].Rows.Count; i++)
                    {
                        string acc_id = ExpenseAcclst.Tables[0].Rows[i]["acc_id"].ToString();
                        string acc_name = ExpenseAcclst.Tables[0].Rows[i]["acc_name"].ToString();
                        //string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ExpenseAcc.Add(acc_id, acc_name);
                    }
                }
                return Json(ExpenseAcc.Select(c => new { acc_id = c.Key, acc_name = c.Value }).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
        public ActionResult GetVoucherList(string acc_id,string SearchName)
       {
            //JsonResult DataRows = null;
            try
            {
                Dictionary<string, string> VouList = new Dictionary<string, string>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet ExVouList = _ExpenseVoucher_ISERVICE.GetVouList(CompID, Br_ID,acc_id, SearchName);
                if (ExVouList.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ExVouList.Tables[0].Rows.Count; i++)
                    {
                        string ID = ExVouList.Tables[0].Rows[i]["vou_no"].ToString();
                        string Vou_No = ExVouList.Tables[0].Rows[i]["vou_no"].ToString();
                        VouList.Add(ID, Vou_No);
                    }
                    //for (int i = 0; i < ExVouList.Tables[0].Rows.Count; i++)
                    //{
                    //    string ID = ExVouList.Tables[0].Rows[i]["vou_no"].ToString();
                    //    string Vou_No = ExVouList.Tables[0].Rows[i]["vou_no"].ToString();
                    //    string voucher_dt = ExVouList.Tables[0].Rows[i]["voucher_dt"].ToString();
                    //    VouList.Add(ID, Vou_No+'_'+ voucher_dt );
                    //}
                }
                //DataRows = Json(JsonConvert.SerializeObject(ExVouList.Tables[0]));/*Result convert into Json Format for javasript*/
                return Json(VouList.Select(c => new { ID = c.Key , vou_no = c.Value }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            //return DataRows;
        }
        public ActionResult GetVoucherList1(string acc_id, string SearchName)
        {
            //JsonResult DataRows = null;
            try
            {
                JsonResult DataRows = null;
                Dictionary<string, string> VouList = new Dictionary<string, string>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet ExVouList = _ExpenseVoucher_ISERVICE.GetVouList(CompID, Br_ID, acc_id, SearchName);
                DataRow Drow = ExVouList.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";

                ExVouList.Tables[0].Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(ExVouList), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            //return DataRows;
        }
        public JsonResult GetVoucherDtAndAmt(string acc_id, string Vou_Dt)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable Result = _ExpenseVoucher_ISERVICE.GetClosingBal(CompID, Br_ID, acc_id, Vou_Dt);
                DataRows = Json(JsonConvert.SerializeObject(Result), JsonRequestBehavior.AllowGet);
                return DataRows;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public JsonResult Getgl_accgroup(string acc_id)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable Result = _ExpenseVoucher_ISERVICE.Getgl_accgroup(CompID, Br_ID, acc_id);
                DataRows = Json(JsonConvert.SerializeObject(Result), JsonRequestBehavior.AllowGet);
                return DataRows;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public JsonResult Get_VoucherDetails(string acc_id, string vou_no, string vou_dt)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable Result = _ExpenseVoucher_ISERVICE.Get_VouDetails(CompID, Br_ID, acc_id, vou_no, vou_dt);
                DataRows = Json(JsonConvert.SerializeObject(Result), JsonRequestBehavior.AllowGet);
                return DataRows;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult ExpenseVouActions(ExpenseVoucher_Model _ExVouModel, string Command)
        {
            try
            {/*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                UrlData urlData = new UrlData();
                if (_ExVouModel.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
                        //if (Session["CompId"] != null)
                        //    CompID = Session["CompId"].ToString();
                        //if (Session["BranchId"] != null)
                        //    Br_ID = Session["BranchId"].ToString();
                        //DateTime dtnow = DateTime.Now;
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                        //string MsgNew = string.Empty;
                        //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
                        //if (MsgNew == "FY Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    urlData.BtnName = "Refresh";
                        //    urlData.Command = "Refresh";
                        //    urlData.Vou_Dt = CurrentDate;
                        //    return RedirectToAction("ExpenseVoucherDetail",  urlData);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    urlData.BtnName = "Refresh";
                        //    urlData.Command = "Refresh";
                        //    urlData.Vou_Dt = CurrentDate;
                        //    return RedirectToAction("ExpenseVoucherDetail",urlData);
                        //}
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew",null, null, null, _ExVouModel.ListFilterData1,null,null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _ExVouModel.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_ExVouModel.DocumentStatus == "A" || _ExVouModel.DocumentStatus == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("DblClickExVouList", new { Vou_No = _ExVouModel.Vou_No, Vou_Dt = _ExVouModel.Vou_Date, ListFilterData1 = _ExVouModel.ListFilterData1});
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Edit", "Update", "BtnEdit", _ExVouModel.Vou_No, _ExVouModel.Vou_Date, null, _ExVouModel.ListFilterData1, null, null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);
                    case "Save":
                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _ExVouModel.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt1);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (Msg == "FY Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                            }
                            else
                            {
                                TempData["FBMessage"] = "Financial Book Closing";
                            }
                            if (_ExVouModel.Vou_No == null)
                            {
                                _ExVouModel.BtnCommand = "Refresh";
                                _ExVouModel.Command = "Refresh";
                                _ExVouModel.TransType = "Refresh";
                                _ExVouModel.BtnName = "Refresh";
                                _ExVouModel.DocumentStatus = null;
                                TempData["ModelData"] = _ExVouModel;
                                return RedirectToAction("ExpenseVoucherDetail",_ExVouModel);
                            }
                            else
                            {
                                return RedirectToAction("DblClickExVouList", new { Vou_No = _ExVouModel.Vou_No, Vou_Dt = _ExVouModel.Vou_Date, ListFilterData1 = _ExVouModel.ListFilterData1 });
                            }

                        }
                        /*End to chk Financial year exist or not*/
                        ExpenseVoucherSave(_ExVouModel);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _ExVouModel.Vou_No, _ExVouModel.Vou_Date, _ExVouModel.Message, _ExVouModel.ListFilterData1, null, null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);

                    case "Approve":
                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _ExVouModel.Vou_Date;

                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt3);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (Msg == "FY Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                            }
                            else
                            {
                                TempData["FBMessage"] = "Financial Book Closing";
                            }
                            return RedirectToAction("DblClickExVouList", new { Vou_No = _ExVouModel.Vou_No, Vou_Dt = _ExVouModel.Vou_Date, ListFilterData1 = _ExVouModel.ListFilterData1 });
                        }
                        /*End to chk Financial year exist or not*/
                        ApproveExpenseVou(_ExVouModel,_ExVouModel.Vou_No, _ExVouModel.Vou_Date,"","","","","",_ExVouModel.payGLvoucher_narr);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _ExVouModel.Vou_No, _ExVouModel.Vou_Date, _ExVouModel.Message, _ExVouModel.ListFilterData1, _ExVouModel.GLVoucherNo, _ExVouModel.GLVoucherDt);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);

                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _ExVouModel.ListFilterData1, null, null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);

                    case "Delete":
                        DeleteExpenseVouDetail(_ExVouModel);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh",null,null, _ExVouModel.Message, _ExVouModel.ListFilterData1, null, null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);

                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, null, _ExVouModel.ListFilterData1, null, null);
                        return RedirectToAction("ExpenseVoucher");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew",null,null, null, null, null, null);
                        return RedirectToAction("ExpenseVoucherDetail", urlData);


                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName,string Vou_No,string Vou_Dt, string Message = null, string ListFilterData1 = null,string gl_Vou_No=null, string gl_Vou_Dt=null)
        {

            urlData.Command = Command;
            urlData.TransType = TransType;
            urlData.BtnName = BtnName;
            urlData.Vou_No = Vou_No;
            urlData.Vou_Dt = Vou_Dt;
            urlData.Message = Message;
            urlData.ListFilterData1 = ListFilterData1;
            urlData.gl_Vou_No = gl_Vou_No;
            urlData.gl_Vou_Dt = gl_Vou_Dt;
            TempData["ListFilterData"] = ListFilterData1;
            TempData["UrlData"] = urlData;
            TempData["Message"] = Message;

        }
        public ActionResult ExpenseVoucherSave(ExpenseVoucher_Model _ExVou_Model) {
            string SaveMessage = "";
            getDocumentName();
            string PageName = title.Replace(" ", "");
            try {
                if (_ExVou_Model.CancelFlag == false)
                {

                    DataTable HeaderDetail = new DataTable();
                    DataTable PaymentDetail = new DataTable();
                    DataTable ExpenseDetail = new DataTable();
                    DataTable CostCenterDetails = new DataTable();
                    DataTable DtblVouGLDetail = new DataTable();

                    DataTable Hd = new DataTable();
                    Hd.Columns.Add("TransType", typeof(string));
                    Hd.Columns.Add("MenuDocumentId", typeof(string));
                    Hd.Columns.Add("comp_id", typeof(int));
                    Hd.Columns.Add("br_id", typeof(int));
                    Hd.Columns.Add("user_id", typeof(string));
                    Hd.Columns.Add("Payeeacc_id", typeof(string));
                    Hd.Columns.Add("Vou_No", typeof(string));
                    Hd.Columns.Add("Vou_Dt", typeof(string));
                    Hd.Columns.Add("Vou_Status", typeof(string));
                    Hd.Columns.Add("mac_id", typeof(string));

                    DataRow dtrow = Hd.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["TransType"] = _ExVou_Model.TransType;
                    dtrow["Vou_No"] = _ExVou_Model.Vou_No;
                    dtrow["Vou_Dt"] = _ExVou_Model.Vou_Date;
                    dtrow["Payeeacc_id"] = _ExVou_Model.PayeeAcc_Id;
                    dtrow["Vou_Status"] = "D";
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["user_id"] = Session["UserId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrow["mac_id"] = mac_id;// _model.cal_on;
                    Hd.Rows.Add(dtrow);
                    HeaderDetail = Hd;
                    /*-----Payment Detail Section------*/
                    DataTable PD = new DataTable();

                    PD.Columns.Add("PDVou_No", typeof(string));
                    PD.Columns.Add("PDVou_Dt", typeof(string));
                    PD.Columns.Add("PDVou_Amt", typeof(string));

                    JArray jObject = JArray.Parse(_ExVou_Model.PaymentVouDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow PDRow = PD.NewRow();

                        PDRow["PDVou_No"] = jObject[i]["PDVou_No"].ToString();
                        PDRow["PDVou_Dt"] = jObject[i]["PDVou_Dt"].ToString();
                        PDRow["PDVou_Amt"] = jObject[i]["PDVou_Amt"].ToString();
                        PD.Rows.Add(PDRow);
                    }
                    PaymentDetail = PD;
                    /*-----Payment Detail Section END------*/

                    /*-----Expense Detail Section------*/
                    DataTable ED = new DataTable();
                    ED.Columns.Add("ExpDesc", typeof(string));
                    ED.Columns.Add("Bill_No", typeof(string));
                    ED.Columns.Add("Bill_Date", typeof(string));
                    ED.Columns.Add("EDAmount", typeof(string));
                    ED.Columns.Add("EDacc_id", typeof(string));

                    JArray KObject = JArray.Parse(_ExVou_Model.ExpenseDescDetails);
                    for (int i = 0; i < KObject.Count; i++)
                    {
                        DataRow EDRow = ED.NewRow();

                        EDRow["ExpDesc"] = KObject[i]["ExpDesc"].ToString();
                        EDRow["Bill_No"] = KObject[i]["Bill_No"].ToString();
                        EDRow["Bill_Date"] = KObject[i]["Bill_Date"].ToString();
                        EDRow["EDAmount"] = KObject[i]["EDAmount"].ToString();
                        EDRow["EDacc_id"] = KObject[i]["EDacc_id"].ToString();
                        ED.Rows.Add(EDRow);
                    }
                    ExpenseDetail = ED;
                    /*-----Expense Detail Section END------*/
                    DtblVouGLDetail = ToDtblVouGlDetail(_ExVou_Model.VouGlDetails);
                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));



                    JArray JAObj = JArray.Parse(_ExVou_Model.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    CostCenterDetails = CC_Details;
                    ViewData["CCdetail"] = dtCCdetail(JAObj);

                    /**----------------Cost Center Section END--------------------*/


                    /*-----Attachment Section------*/

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable EVAttachments = new DataTable();
                    DataTable EVdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as ExpenseVoucher_model;
                    TempData["IMGDATA"] = null;
                    if (_ExVou_Model.attatchmentdetail != null)
                    {

                        //if (Session["AttachMentDetailItmStp"] != null)
                        //{
                        //    EVdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                        //}
                        if (attachData != null)
                        {
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                EVdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                EVdtAttachment.Columns.Add("id", typeof(string));
                                EVdtAttachment.Columns.Add("file_name", typeof(string));
                                EVdtAttachment.Columns.Add("file_path", typeof(string));
                                EVdtAttachment.Columns.Add("file_def", typeof(char));
                                EVdtAttachment.Columns.Add("comp_id", typeof(Int32));
                            }
                        }
                        else
                        {
                            if (_ExVou_Model.AttachMentDetailItmStp != null)
                            {
                                EVdtAttachment = _ExVou_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                EVdtAttachment.Columns.Add("id", typeof(string));
                                EVdtAttachment.Columns.Add("file_name", typeof(string));
                                EVdtAttachment.Columns.Add("file_path", typeof(string));
                                EVdtAttachment.Columns.Add("file_def", typeof(char));
                                EVdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_ExVou_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in EVdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = EVdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_ExVou_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _ExVou_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                EVdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ExVou_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string BP_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_ExVou_Model.Vou_No).ToString()))
                                {
                                    BP_CODE = (_ExVou_Model.Vou_No).ToString();
                                }
                                else
                                {
                                    BP_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + BP_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in EVdtAttachment.Rows)
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
                        EVAttachments = EVdtAttachment;
                    }

                    /*-----Attachment Section END------*/

                    SaveMessage = _ExpenseVoucher_ISERVICE.InsertExpenseVouDetail(HeaderDetail, PaymentDetail, ExpenseDetail, EVAttachments, DtblVouGLDetail, CostCenterDetails);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _ExVou_Model.BtnName = "Refresh";
                        _ExVou_Model.Command = "Refresh";
                        _ExVou_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("ExpenseVoucherDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _ExVou_Model.BtnName = "Refresh";
                        _ExVou_Model.Command = "Refresh";
                        _ExVou_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("ExpenseVoucherDetail");
                    }
                    else
                    {
                        if (SaveMessage.Split(',')[0] == "Update")
                        {
                            _ExVou_Model.Message = "Save";
                        }
                        else
                        {
                            _ExVou_Model.Message = SaveMessage.Split(',')[0];
                        }
                        _ExVou_Model.Vou_No = SaveMessage.Split(',')[1];
                        _ExVou_Model.Vou_Date = SaveMessage.Split(',')[2];
                    }
                }
                else
                {
                    UrlData urlData = new UrlData();
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
                        UserID = Session["userid"].ToString();
                    }
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string Result = _ExpenseVoucher_ISERVICE.CancelExpVouDt(CompID, Br_ID, _ExVou_Model.Vou_No, _ExVou_Model.Vou_Date, UserID, mac_id, _ExVou_Model.CancelledRemarks);
                    var Message = Result.Split(',')[0] == "C" ? "Cancelled" : "Error";
                    _ExVou_Model.Vou_No = Result.Split(',')[1];
                    _ExVou_Model.Vou_Date = Result.Split(',')[2];
                    _ExVou_Model.Message = Message;
                    //SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _ExVou_Model.Vou_No = Result.Split(',')[1], _ExVou_Model.Vou_Date = Result.Split(',')[2], _ExVou_Model.Message = Message);
                }
                return RedirectToAction("ExpenseVoucherDetail");              
            }                                                                 
            catch (Exception Ex) 
            {
                throw Ex;
            }
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }
        private DataTable ToDtblVouGlDetail(string VouGlDetails)
        {
            try
            {
                DataTable DtblVouGlDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("id", typeof(double));
                dtItem.Columns.Add("type", typeof(int));
                dtItem.Columns.Add("doctype", typeof(string));
                dtItem.Columns.Add("Value", typeof(int));
                dtItem.Columns.Add("DrAmt", typeof(string));
                dtItem.Columns.Add("CrAmt", typeof(string));
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("Gltype", typeof(string));

                JArray jObject = JArray.Parse(VouGlDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowLines["id"] = jObject[i]["id"].ToString();
                    dtrowLines["type"] = 'I';
                    dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                    dtrowLines["Value"] = jObject[i]["Value"].ToString();
                    dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                    dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                    dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();

                    dtItem.Rows.Add(dtrowLines);
                }
                DtblVouGlDetail = dtItem;
                ViewData["VouDetail"] = dtVoudetail(jObject);
                return DtblVouGlDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public DataTable dtVoudetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(string));
            dtItem.Columns.Add("acc_id", typeof(string));
            dtItem.Columns.Add("acc_name", typeof(string));
            dtItem.Columns.Add("type", typeof(string));
            dtItem.Columns.Add("doctype", typeof(string));
            dtItem.Columns.Add("Value", typeof(string));
            dtItem.Columns.Add("dr_amt_sp", typeof(string));
            dtItem.Columns.Add("cr_amt_sp", typeof(string));
            dtItem.Columns.Add("TransType", typeof(string));
            dtItem.Columns.Add("gl_type", typeof(string));
            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                dtrowLines["acc_id"] = jObject[i]["id"].ToString();
                dtrowLines["acc_name"] = jObject[i]["acc_name"].ToString();
                dtrowLines["type"] = jObject[i]["type"].ToString();
                dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                dtrowLines["Value"] = jObject[i]["Value"].ToString();
                dtrowLines["dr_amt_sp"] = jObject[i]["DrAmt"].ToString();
                dtrowLines["cr_amt_sp"] = jObject[i]["CrAmt"].ToString();
                dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                dtrowLines["gl_type"] = jObject[i]["Gltype"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public void DeleteExpenseVouDetail(ExpenseVoucher_Model _ExVou_Model)
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
                string Result = _ExpenseVoucher_ISERVICE.DeleteExpenseDetail(CompID, Br_ID, _ExVou_Model.Vou_No, _ExVou_Model.Vou_Date);
                _ExVou_Model.Message = Result.Split(',')[0];
                _ExVou_Model.Vou_No = null;
                _ExVou_Model.Vou_Date = null;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public ActionResult DblClickExVouList(string Vou_No, string Vou_Dt, string ListFilterData)
        {/*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
            UrlData urlData = new UrlData();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            string DblClkMsg = string.Empty;
            string BtnName = string.Empty;
            DblClkMsg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Vou_Dt);
            if (DblClkMsg == "FY Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                BtnName = "Refresh";
            }
            else if (DblClkMsg == "FB Close")
            {
                TempData["FBMessage"] = "Financial Book Closing";
               BtnName = "Refresh";
            }
            else
            {
                BtnName = "BtnToDetailPage";
            }
            /*End to chk Financial year exist or not*/
           // UrlData urlData = new UrlData();
            //string BtnName = "BtnToDetailPage";
            string TransType = "Update";
            SetUrlData(urlData, "Add", TransType, BtnName, Vou_No, Vou_Dt,null, ListFilterData, null,null);

            return RedirectToAction("ExpenseVoucherDetail", "ExpenseVoucher", urlData);
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UseId = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UseId = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UseId, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public DataTable GetApprovalLevel(string Comp_ID, string Br_ID, string DocumentMenuId)
        {
            try
            {

                DataSet ApprovalLevelList = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId);
                DataTable DT_AppLevel = new DataTable();
                DT_AppLevel = ApprovalLevelList.Tables[0];
                return DT_AppLevel;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }

        public ActionResult ExpVouSearchedList(string Acc_Id,string Fromdate,string Todate,string Status)
        {
            ExpenseVoucherList_Model _ExpVouListModel = new ExpenseVoucherList_Model();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            DataSet Ds = _ExpenseVoucher_ISERVICE.SerachListExpenseVoucher(CompID, Br_ID,Acc_Id,Fromdate,Todate,Status);
            _ExpVouListModel.ExpVouSearch = "VouSearch";
            List<ExVouList> VouLst = new List<ExVouList>();
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    ExVouList Lst = new ExVouList();
                    Lst.Vou_No = dr["vou_no"].ToString();
                    Lst.Vou_Dt = dr["vou_dt"].ToString();
                    Lst.Vou_Date = dr["vou_date"].ToString();
                    Lst.acc_id = dr["acc_id"].ToString();
                    Lst.PayAmt = dr["PaymentAmt"].ToString();
                    Lst.ExpAmt = dr["ExpenseAmt"].ToString();
                    Lst.Vou_Status = dr["ExpVou_Status"].ToString();
                    Lst.CreateBy = dr["create_by"].ToString();
                    Lst.CreateDt = dr["CreateDate"].ToString();
                    Lst.Approve_by = dr["app_by"].ToString();
                    Lst.Approve_Date = dr["ApproveDate"].ToString();
                    Lst.Modify_By = dr["ModifyBy"].ToString();
                    Lst.Modify_Date = dr["ModifyDate"].ToString();
                    VouLst.Add(Lst);
                }
            }
            _ExpVouListModel.VouList = VouLst;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialExpenseVoucherList.cshtml", _ExpVouListModel);
            
        }

        public ActionResult ApproveExpenseVou(ExpenseVoucher_Model _ExpModel,string Vou_No,string Vou_Date,string A_Status, string A_Level, string A_Remarks/*, TDSPosting_Model _model*/
            , string ListFilterData1, string WF_Status1, string payglnarr)
        {
            try
            {
                UrlData urlData = new UrlData();
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
                    UserID = Session["userid"].ToString();
                }
                /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                string Msg = string.Empty;
                var commCont = new CommonController(_Common_IServices);
                Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Vou_Date);
                if (Msg == "FY Not Exist" || Msg == "FB Close")
                {
                    if (Msg == "FY Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                    }
                    else
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                    }
                    return RedirectToAction("DblClickExVouList", new { Vou_No = Vou_No, Vou_Dt = Vou_Date, ListFilterData1 = ListFilterData1});
                }
               /*End to chk Financial year exist or not*/
               string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _ExpenseVoucher_ISERVICE.ApproveExpVouDt(CompID, Br_ID, Vou_No, Vou_Date, A_Status, A_Level, A_Remarks, UserID, mac_id, DocumentMenuId, payglnarr);

                var Message = Result.Split(',')[0] == "A" ? "Approved" : "Error";

                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _ExpModel.Vou_No=Result.Split(',')[1], _ExpModel.Vou_Date=Result.Split(',')[2], _ExpModel.Message=Message, ListFilterData1, _ExpModel.Vou_Date = Result.Split(',')[3], _ExpModel.Vou_Date = Result.Split(',')[4]);
                return RedirectToAction("ExpenseVoucherDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string Title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                ExpenseVoucher_model attachment_Model = new ExpenseVoucher_model();
                
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                
                if (TransType == "Save")
                {
                    
                    DocNo = gid.ToString();
                }
                //JVCode = JVCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = JVCode;
                attachment_Model.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + Br_ID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    attachment_Model.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    attachment_Model.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = attachment_Model;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/

        /*------------- Cost Center Section-----------------*/

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata, string TotalAmt = null, string Doc_ID = null)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                if (CC_rowdata.ToString() != null && CC_rowdata.ToString() != "" && CC_rowdata.ToString() != "[]")
                {
                    DataTable Cctype = new DataTable();
                    Cctype.Columns.Add("GlAccount", typeof(string));
                    Cctype.Columns.Add("ddl_cc_type", typeof(string));
                    Cctype.Columns.Add("dd_cc_type_id", typeof(string));
                    JArray arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow dtrowLines = Cctype.NewRow();
                        dtrowLines["GlAccount"] = arr[i]["GlAccount"].ToString();
                        dtrowLines["ddl_cc_type"] = arr[i]["ddl_CC_Type"].ToString();
                        dtrowLines["dd_cc_type_id"] = arr[i]["ddl_Type_Id"].ToString();
                        Cctype.Rows.Add(dtrowLines);
                    }
                    DataView dv = new DataView();
                    dv = Cctype.DefaultView;
                    ViewBag.CC_type = dv.ToTable(true, "GlAccount", "ddl_cc_type", "dd_cc_type_id");

                    DataTable ccitem = new DataTable();
                    ccitem.Columns.Add("dd_cc_typ_id", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name_id", typeof(string));
                    ccitem.Columns.Add("cc_Amount", typeof(string));

                    JArray Arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow DtrowLines = ccitem.NewRow();
                        DtrowLines["dd_cc_typ_id"] = arr[i]["ddl_Type_Id"].ToString();
                        DtrowLines["ddl_cc_name"] = arr[i]["ddl_CC_Name"].ToString();
                        DtrowLines["ddl_cc_name_id"] = arr[i]["ddl_Name_Id"].ToString();
                        DtrowLines["cc_Amount"] = arr[i]["CC_Amount"].ToString();
                        ccitem.Rows.Add(DtrowLines);
                    }
                    ViewBag.CC_Item = ccitem;
                }


                DataSet ds = _Common_IServices.GetCstCntrData(CompID, Br_ID, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CostcntrType Cc_type = new CostcntrType();
                    Cc_type.cc_id = dr["cc_id"].ToString();
                    Cc_type.cc_name = dr["cc_name"].ToString();
                    Cctypelist.Add(Cc_type);
                }
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;

                //List<CostcntrType> Cctypelist = new List<CostcntrType>();

                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    CostcntrType Cc_type = new CostcntrType();
                //    Cc_type.cc_id = dr["cc_id"].ToString();
                //    Cc_type.cc_name = dr["cc_name"].ToString();
                //    Cctypelist.Add(Cc_type);
                //}
                //Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                //_CC_Model.costcntrtype = Cctypelist;

                _CC_Model.disflag = Disableflag;
                ViewBag.CCTotalAmt = TotalAmt;//add by sm 09-12-2024
                ViewBag.DocId = Doc_ID;//add by sm 09-12-2024
                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public JsonResult GetCstCntrName(string CCtypeid)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCstCntrData(CompID, Br_ID, CCtypeid, "ccname");
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        /*------------- Cost Center Section END ----------------*/
        public FileResult ExpenseVoucherExporttoExcelDt(string Acc_Id, string FromDate, string Todate, string Status)
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataTable dt = new DataTable();
                DataSet Ds = _ExpenseVoucher_ISERVICE.SerachListExpenseVoucher(CompID, Br_ID, Acc_Id, FromDate, Todate, Status);
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Payee GL Account", typeof(string));
                dt.Columns.Add("Payment Amount", typeof(decimal));
                dt.Columns.Add("Expense Amount", typeof(decimal));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Created By", typeof(string));
                dt.Columns.Add("Created On", typeof(string));
                dt.Columns.Add("Approved By", typeof(string));
                dt.Columns.Add("Approved On", typeof(string));
                dt.Columns.Add("Amended By", typeof(string));
                dt.Columns.Add("Amended On", typeof(string));

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in Ds.Tables[0].Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["GL Voucher Number"] = dr["vou_no"].ToString();
                        dtrowLines["GL Voucher Date"] = dr["vou_dt"].ToString();
                        dtrowLines["Payee GL Account"] = dr["acc_id"].ToString();
                        dtrowLines["Payment Amount"] = dr["PaymentAmt"].ToString();
                        dtrowLines["Expense Amount"] = dr["ExpenseAmt"].ToString();
                        dtrowLines["Status"] = dr["ExpVou_Status"].ToString();
                        dtrowLines["Created By"] = dr["create_by"].ToString();
                        dtrowLines["Created On"] = dr["CreateDate"].ToString();
                        dtrowLines["Approved By"] = dr["app_by"].ToString();
                        dtrowLines["Approved On"] = dr["ApproveDate"].ToString();
                        dtrowLines["Amended By"] = dr["ModifyBy"].ToString();
                        dtrowLines["Amended On"] = dr["ModifyDate"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("ImprestVoucher", dt);
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