using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.DASHBOARD;
using EnRepMobileWeb.SERVICES.SERVICES;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.BusinessLayer.GLDetail;
using OfficeOpenXml;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
//*** All Session Removed by shubham maurya on 21-12-2022 16:05***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class GLListController : Controller
    {
        string CompID, userid, Br_ID, language = String.Empty;
        string DocumentMenuId = "103170", title;
        Common_IServices _Common_IServices;
        DataTable GLListDs;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        GLList_ISERVICES _GLList_ISERVICES;
        public GLListController(Common_IServices _Common_IServices, GLList_ISERVICES _GLList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._GLList_ISERVICES = _GLList_ISERVICES;
        }
        // GET: BusinessLayer/GLList
        public ActionResult GLList()
        {
            try
            {
                GLDetailModel _GLDetail = new GLDetailModel();
                DASHBOARD_MODEL _DASHBOARD_MODEL = new DASHBOARD_MODEL();
                List<Branch> _BranchList = new List<Branch>();
                List<String> Menu = new List<string>();
                string Comp_ID = string.Empty;
                string Language = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ItemListFilter = TempData["ListFilterData"].ToString();
                    if (ItemListFilter != "" && ItemListFilter != null)
                    {


                        _GLDetail.ListFilterData = ItemListFilter;
                        var a = ItemListFilter.Split(',');
                        var GLID = a[0].Trim();
                        var GRPID = a[1].Trim();
                        var GLAct = a[2].Trim();
                        var GLAcctype = a[3].Trim();
                        #region Commented By Nitesh Search Data And Page Load Data Is Coming One Procedure
                        //    DataTable HoCompData = _GLList_ISERVICES.GetGLListFilterDAL(Comp_ID, GLID, GRPID, GLAct, GLAcctype).Tables[0];
                        // ViewBag.VBGLList = HoCompData;
                        #endregion
                        _GLDetail.ddlGLName = GLID;
                        _GLDetail.ddlGLGroup = GRPID;
                        _GLDetail.acc_type = Convert.ToInt32(GLAcctype);
                        _GLDetail.GLActStatus = GLAct;
                        _GLDetail.Flag = "FilterData";

                    }

                }
                else
                {
                    _GLDetail.Flag = "NotFilterData";
                    #region Commented By Nitesh Search Data And Page Load Data Is Coming One Procedure
                    // GLListDs = new DataTable();
                    //     GLListDs = _GLList_ISERVICES.GetGLListDAL(Comp_ID);
                    //  ViewBag.VBGLList = GLListDs;
                    #endregion
                }
                GetAllDropDownList(_GLDetail);
                ViewBag.MenuPageName = getDocumentName();

                _GLDetail.Title = title;
                //Session["GLSearch"] = "0";
                _GLDetail.GLSearch = "0";
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/GLSetup/GLList.cshtml", _GLDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void GetAllDropDownList(GLDetailModel _GLDetail)/***Added By Nitesh 19-03-2024 For All DropDown***/
        {
            try
            {
                string Comp_ID = string.Empty;
                string GroupName = string.Empty;
                string Language = string.Empty;
                string ddlGL_Group = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                GroupName = "0";
                ddlGL_Group = "0";
                DataSet dt = _GLList_ISERVICES.GetAllDropDownGL(GroupName, Comp_ID, Br_ID, ddlGL_Group, _GLDetail.ddlGLName, _GLDetail.ddlGLGroup
                    , _GLDetail.GLActStatus, _GLDetail.acc_type.ToString(), _GLDetail.Flag);
                List<GLName> _GLName = new List<GLName>();
                //  DataTable dt = GetGLNameList(_GLDetail);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    GLName ddlGLName = new GLName();
                    ddlGLName.ID = dr["acc_id"].ToString();
                    ddlGLName.Name = dr["acc_name"].ToString();
                    _GLName.Add(ddlGLName);
                }
                _GLName.Insert(0, new GLName() { ID = "0", Name = "All" });
                _GLDetail.GLNameList = _GLName;

                List<GLGroup> _GLGroup = new List<GLGroup>();
                //  DataTable dt1 = GetGLGroupList(_GLDetail);
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    GLGroup ddlGLGroup = new GLGroup();
                    ddlGLGroup.ID = dr["acc_grp_id"].ToString();
                    ddlGLGroup.Name = dr["AccGroupChildNood"].ToString();
                    _GLGroup.Add(ddlGLGroup);
                }
                _GLGroup.Insert(0, new GLGroup() { ID = "0", Name = "All" });
                _GLDetail.GLGroupList = _GLGroup;

                ViewBag.VBGLList = dt.Tables[2];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
        private DataTable GetGLNameList(GLDetailModel _GLDetail)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_GLDetail.ddlGLName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _GLDetail.ddlGLName;
                }
                DataTable dt = _GLList_ISERVICES.BindGetGLNameList(GroupName, Comp_ID);
                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable GetGLGroupList(GLDetailModel _GLDetail)
        {
            try
            {
                string GroupName = string.Empty;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_GLDetail.ddlGLGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _GLDetail.ddlGLGroup;
                }
                DataTable dt = _GLList_ISERVICES.BindGetGLGroupList(GroupName, Comp_ID);

                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteGLList(SearchSupp queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> GLList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                GLList = _GLList_ISERVICES.GLSetupGroupDAL(GroupName, Comp_ID);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(GLList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteAccGrp(SearchSupp queryParameters)
        {
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> suggestions = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }
                suggestions = _GLList_ISERVICES.AccGrpListGroupDAL(GroupName, Comp_ID);//GetGroupList(queryParameters.ddlGroup);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(suggestions.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult GetGLListFilter(string GLID, string GRPID, string GLAct, string GLAcctype)
        {
            try
            {
                GLDetailModel _GLDetail = new GLDetailModel();
                ViewBag.VBGLList = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                try
                {
                    DataTable HoCompData = _GLList_ISERVICES.GetGLListFilterDAL(Comp_ID, GLID, GRPID, GLAct, GLAcctype).Tables[0];
                    ViewBag.VBGLlist = HoCompData;
                    //Session["GLSearch"] = "GL_Search";
                    _GLDetail.GLSearch = "GL_Search";
                }
                catch (Exception ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, ex);
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialGLList.cshtml", _GLDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ErrorPage()
        {
            try
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult AddNewGL()
        {
            GLDetailModel _GLDetail = new GLDetailModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            _GLDetail.Message = "New";
            _GLDetail.Command = "Add";
            _GLDetail.AppStatus = "D";
            _GLDetail.TransType = "Save";
            _GLDetail.BtnName = "BtnAddNew";
            TempData["ModelData"] = _GLDetail;
            TempData["ListFilterData"] = null;
            return RedirectToAction("GLDetail", "GLDetail");
        }
        public ActionResult EditGL(string GLId, string act_status_tr, string acc_grp_id, string acc_type, string roa, string plr, string ibt, string iwt, string egl, string sta, string ListFilterData)
        {
            GLDetailModel _GLDetail = new GLDetailModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["GLCode"] = GLId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";

            //_GLDetail.Message = "New";
            //_GLDetail.Command = "Add";
            string Command = "Add";
            //_GLDetail.GLCode = GLId;
            //_GLDetail.TransType = "Update";
            string TransType = "Update";
            //_GLDetail.AppStatus = "D";
            //_GLDetail.BtnName = "BtnToDetailPage";
            string BtnName = "BtnToDetailPage";
            //_GLDetail.acc_type = Convert.ToInt32(acc_type);
            //_GLDetail.acc_grp_id = Convert.ToInt32(acc_grp_id);
            //if (act_status_tr != "N")
            //{
            //    _GLDetail.act_status_tr = true;
            //}
            //else
            //{
            //    _GLDetail.act_status_tr = false;
            //}
            //if (roa != "N")
            //{
            //    _GLDetail.roa = true;
            //}
            //else
            //{
            //    _GLDetail.roa = false;
            //}
            //if (plr != "N")
            //{
            //    _GLDetail.plr = true;
            //}
            //else
            //{
            //    _GLDetail.plr = false;
            //}
            //if (ibt != "N")
            //{
            //    _GLDetail.ibt = true;
            //}
            //else
            //{
            //    _GLDetail.ibt = false;
            //}
            //if (iwt != "N")
            //{
            //    _GLDetail.iwt = true;
            //}
            //else
            //{
            //    _GLDetail.iwt = false;
            //}
            //if (egl != "N")
            //{
            //    _GLDetail.egl = true;
            //}
            //else
            //{
            //    _GLDetail.egl = false;
            //}
            //if (sta != "N")
            //{
            //    _GLDetail.sta = true;
            //}
            //else
            //{
            //    _GLDetail.sta = false;
            //}
            //TempData["ModelData"] = _GLDetail;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("GLDetail", "GLDetail", new { GL_Code = GLId, Command = Command, TransType = TransType, BtnName = BtnName });
        }

        //added By Nitesh (GetGL_list) 03-11-2023 for get Opning and closing Balence and Currency
        [HttpPost]
        public JsonResult GetGL_list(string acc_id, string acc_grpid, string acc_type)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataSet result = _GLList_ISERVICES.GetGLSetup_data(Comp_ID, BranchID, acc_id, acc_grpid, acc_type);
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
        //---------------------------------------------Nidhi-------------------------------------
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                CommonController com_obj = new CommonController();
                DataTable GLDetail = new DataTable();
                DataTable BrDetail = new DataTable();
                DataSet obj_ds = new DataSet();
                GLDetail.Columns.Add("Account Type*", typeof(string));
                GLDetail.Columns.Add("Account Group*", typeof(string));
                GLDetail.Columns.Add("Account Name*(max 100 characters)", typeof(string));
                GLDetail.Columns.Add("Cash Flow Type", typeof(string));
                GLDetail.Columns.Add("Currency", typeof(string));
                GLDetail.Columns.Add("Bank Address(max 100 characters)", typeof(string));
                GLDetail.Columns.Add("Account Number(max 25 characters)", typeof(string));
                GLDetail.Columns.Add("IFSC Code(max 25 characters)", typeof(string));
                GLDetail.Columns.Add("SWIFT Code(max 25 characters)", typeof(string));

                BrDetail.Columns.Add("Account Name", typeof(string));
                BrDetail.Columns.Add("Branch Name", typeof(string));
                BrDetail.Columns.Add("Active", typeof(string));

                obj_ds.Tables.Add(GLDetail);
                obj_ds.Tables.Add(BrDetail);

                DataSet ds = _GLList_ISERVICES.GetMasterDataForExcelFormat(compId);
                string filePath = com_obj.CreateExcelFile("ImportGeneralLedgerTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "GeneralLedger");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ValidateExcelFile(string uploadStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable GeneralLedgerDetail = new DataTable();
                    DataTable GeneralLedgerBranch = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();

                                //string GLDetailQuery = "SELECT DISTINCT * From [GeneralLedgerDetail$] ; ";
                                string GLDetailQuery = "SELECT DISTINCT * FROM [GeneralLedgerDetail$] WHERE LEN([Account Name*(max 100 characters)]) > 0 ";

                                string GLBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";

                                connExcel.Open();
                                cmdExcel.CommandText = GLDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GeneralLedgerDetail);

                                cmdExcel.CommandText = GLBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GeneralLedgerBranch);

                                int dtItemscount = 0;
                                int dtItemslength = 0;
                                if (GeneralLedgerDetail.Rows.Count == 1 || GeneralLedgerDetail.Rows.Count == 0)
                                {
                                    foreach (DataRow dtItem in GeneralLedgerDetail.Rows)
                                    {
                                        dtItemslength = dtItem.ItemArray.Length;
                                        for (int i = 0; i < dtItem.ItemArray.Length; i++)
                                        {
                                            if (dtItem.IsNull(i) || string.IsNullOrWhiteSpace(dtItem[i].ToString()))
                                            {
                                                dtItemscount++;
                                            }
                                        }
                                    }
                                    if (dtItemscount >= dtItemslength)
                                    {
                                        return Json("Excel file is empty. Please fill data in excel file and try again");
                                    }
                                }
                                ds.Tables.Add(GeneralLedgerDetail);
                                ds.Tables.Add(GeneralLedgerBranch);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("You can try to enter invalid records in the Excel file");
                                ViewBag.ImportGLPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportGLDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet dsGL, string uploadStatus)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            try
            {
                DataSet dts = PrepareDataset(dsGL);
                if (dsGL.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsGL.Tables[0].Rows[0].ToString()))
                {
                    DataSet result = _GLList_ISERVICES.GetVerifiedDataOfExcel(compId, dts.Tables[0], dts.Tables[1]);
                    if (uploadStatus.Trim() == "0")
                        return result;

                    var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                    DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                    result.Tables[0].Clear();

                    for (int i = 0; i < newDataTable.Rows.Count; i++)
                    {
                        result.Tables[0].ImportRow(newDataTable.Rows[i]);
                    }
                    result.Tables[0].AcceptChanges();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataSet PrepareDataset(DataSet dsGL)
        {
            DataTable GLDetail = new DataTable();
            DataTable GLBranch = new DataTable();
            GLDetail.Columns.Add("acc_type", typeof(string));
            GLDetail.Columns.Add("acc_grp_name", typeof(string));
            GLDetail.Columns.Add("acc_name", typeof(string));
            GLDetail.Columns.Add("cf_type", typeof(string));
            GLDetail.Columns.Add("curr", typeof(string));
            GLDetail.Columns.Add("bank_addr", typeof(string));
            GLDetail.Columns.Add("acc_no", typeof(string));
            GLDetail.Columns.Add("ifsc_code", typeof(string));
            GLDetail.Columns.Add("swift_code", typeof(string));
            for (int i = 0; i < dsGL.Tables[0].Rows.Count; i++)
            {
                DataTable dtcustomerdetail = dsGL.Tables[0];
                DataRow dtr = GLDetail.NewRow();
                dtr["acc_type"] = dtcustomerdetail.Rows[i][0].ToString().Trim();
                dtr["acc_grp_name"] = dtcustomerdetail.Rows[i][1].ToString().Trim();
                dtr["acc_name"] = dtcustomerdetail.Rows[i][2].ToString().Trim();
                if (dtcustomerdetail.Rows[i][3].ToString() == "")
                {
                    dtr["cf_type"] = dtcustomerdetail.Rows[i][3].ToString().Trim();
                }
                else
                {
                    dtr["cf_type"] = dtcustomerdetail.Rows[i][3].ToString().Trim().Substring(0, 1);

                }
                dtr["curr"] = dtcustomerdetail.Rows[i][4].ToString().Trim();
                dtr["bank_addr"] = dtcustomerdetail.Rows[i][5].ToString().Trim();
                dtr["acc_no"] = dtcustomerdetail.Rows[i][6].ToString().Trim();
                dtr["ifsc_code"] = dtcustomerdetail.Rows[i][7].ToString().Trim();
                dtr["swift_code"] = dtcustomerdetail.Rows[i][8].ToString().Trim();
                GLDetail.Rows.Add(dtr);
            }
            //-------------------------------Branch Detail-------------------------
            GLBranch.Columns.Add("acc_name", typeof(string));
            GLBranch.Columns.Add("branch_name", typeof(string));
            GLBranch.Columns.Add("act_status", typeof(string));
            for (int i = 0; i < dsGL.Tables[1].Rows.Count; i++)
            {
                DataTable dtbranchdetail = dsGL.Tables[1];
                DataRow dtr = GLBranch.NewRow();
                dtr["acc_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                if (!string.IsNullOrEmpty(dtbranchdetail.Rows[i][2].ToString()))
                {
                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim().Substring(0, 1);
                }
                else
                {
                    dtr["act_status"] = "N";
                }
                GLBranch.Rows.Add(dtr);
            }
            //---------------------------End-------------------------------------
            DataSet dts = new DataSet();
            dts.Tables.Add(GLDetail);
            dts.Tables.Add(GLBranch);
            return dts;
        }
        public ActionResult ShowValidationError(string accountname)
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable GLDetail = new DataTable();
                    DataTable GLBranch = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string GLDetailQuery = "";
                                if (string.IsNullOrWhiteSpace(accountname))
                                {
                                    GLDetailQuery = "SELECT DISTINCT * FROM [GeneralLedgerDetail$] WHERE [Account Name*(max 100 characters)] IS NULL OR LTRIM(RTRIM([Account Name*(max 100 characters)])) = '';";
                                }
                                else
                                {
                                    GLDetailQuery = "SELECT DISTINCT * FROM [GeneralLedgerDetail$] WHERE [Account Name*(max 100 characters)] = '" + accountname + "';";
                                }
                                string GLBranchQuery = "SELECT DISTINCT * From [BranchMapping$] where [Account Name] = '" + accountname + "' ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = GLDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GLDetail);

                                cmdExcel.CommandText = GLBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GLBranch);

                                ds.Tables.Add(GLDetail);
                                ds.Tables.Add(GLBranch);
                                DataTable dts = VerifySingleData(ds);
                                ViewBag.ErrorDetails = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable VerifySingleData(DataSet ds)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            DataSet dts = PrepareDataset(ds);
            DataTable result = _GLList_ISERVICES.ShowExcelErrorDetail(compId, dts.Tables[0], dts.Tables[1]);
            return result;
        }
        public ActionResult BindGLBranch(string accountname)
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtGLdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string GLQuery = "SELECT DISTINCT * From [BranchMapping$] where [Account Name] = '" + accountname + "' ; ";
                                cmdExcel.CommandText = GLQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtGLdetail);

                                DataTable GLBranch = new DataTable();
                                GLBranch.Columns.Add("acc_name", typeof(string));
                                GLBranch.Columns.Add("branch_name", typeof(string));
                                GLBranch.Columns.Add("act_status", typeof(string));
                                for (int i = 0; i < dtGLdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtGLdetail;
                                    DataRow dtr = GLBranch.NewRow();
                                    dtr["acc_name"] = dtbranchdetail.Rows[i][0].ToString().Trim();
                                    dtr["branch_name"] = dtbranchdetail.Rows[i][1].ToString().Trim();
                                    dtr["act_status"] = dtbranchdetail.Rows[i][2].ToString().Trim();
                                    GLBranch.Rows.Add(dtr);
                                }
                                ViewBag.BranchDetail = GLBranch;
                                ViewBag.PageName = "GL";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialBranchMapping.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult ImportGLDetailFromExcel()
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable GLDetail = new DataTable();
                    DataTable GLBranch = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string GLDetailQuery = "SELECT DISTINCT * From [GeneralLedgerDetail$] ; ";
                                string GLBranchQuery = "SELECT DISTINCT * From [BranchMapping$] ; ";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = GLDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GLDetail);

                                cmdExcel.CommandText = GLBranchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(GLBranch);

                                ds.Tables.Add(GLDetail);
                                ds.Tables.Add(GLBranch);
                                string msg = SaveGLFromExcel(ds);
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                }
                else
                    return Json("No file selected", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("cannot insert duplicate"))
                    return Json("something went wrong", JsonRequestBehavior.AllowGet);
                else
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private string SaveGLFromExcel(DataSet dsCustomer)
        {
            string compId = "";
            string UserID = "";
            string BranchName = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            if (Session["BranchName"] != null)
                BranchName = Session["BranchName"].ToString();
            DataSet dts = PrepareDataset(dsCustomer);
            string result = _GLList_ISERVICES.BulkImportGLDetail(compId, UserID, BranchName, dts.Tables[0], dts.Tables[1]);
            return result;
        }
    }
}
