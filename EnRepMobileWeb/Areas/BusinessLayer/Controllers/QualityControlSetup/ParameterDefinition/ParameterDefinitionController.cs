using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using OfficeOpenXml;
using System.IO;
using System.Configuration;
using System.Data.OleDb;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.QualityControlSetup.ParameterDefinition
{
    public class ParameterDefinitionController : Controller

    {
        string userid, CustId = String.Empty;
        string CompID, branchID, language = String.Empty;
        string DocumentMenuId = "103200101", title;
        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        QCParameterDefinition_ISERVICES _QCParameterDefinition_ISERVICES;
        public ParameterDefinitionController(Common_IServices _Common_IServices, QCParameterDefinition_ISERVICES QCParameterDefinition_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._QCParameterDefinition_ISERVICES = QCParameterDefinition_ISERVICES;
        }

        // GET: BusinessLayer/ParameterDefinition
        public ActionResult ParameterDefinition(string Pname, string Ptype, string paramId)
        {
            try
            {
                //ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
                if (Session["UserName"] != null)
                {
                    userid = Session["UserName"].ToString();
                }
                var _QCParameterDefinition1 = TempData["ModelData"] as QCParameterDefinition;
                if (_QCParameterDefinition1 != null)
                {
                    ViewBag.CreatedBy = userid;
                    ViewBag.CreatedDate = System.DateTime.Now.ToLongDateString();
                    // QCParameterDefinition _QCParameterDefinition1 = new QCParameterDefinition();
                    dt = GetItemList();
                    //ViewBag.MenuPageName = getDocumentName();
                    _QCParameterDefinition1.Title = title;
                    List<ParameterList> modelList = new List<ParameterList>();
                    //  var value = _QCItemParameter_ISERVICES.GetItemParameterList();
                    foreach (DataRow dr in dt.Rows)
                    {

                        ParameterList _ParameterList = new ParameterList();

                        _ParameterList.create_id = dr["create_id"].ToString();
                        _ParameterList.mod_id = dr["mod_id"].ToString();
                        _ParameterList.param_Id = Convert.ToInt32(dr["param_Id"]);
                        _ParameterList.param_name = dr["param_name"].ToString();
                        _ParameterList.param_type_val = dr["param_type_val"].ToString();
                        _ParameterList.param_type = dr["param_type"].ToString();
                        _ParameterList.create_dt = dr["CreateDate"].ToString();
                        _ParameterList.mod_dt = dr["modefiedDate"].ToString();
                        // _ParameterList.mod_id = Convert.ToInt32(dr["create_id"]);
                        modelList.Add(_ParameterList);
                    }
                    _QCParameterDefinition1.ParamDefinitionList = modelList;
                    //if (Session["MessagePrmDef"] != null)
                    if (_QCParameterDefinition1.MessagePrmDef != null)
                    {
                        //ViewBag.Message = Session["MessagePrmDef"].ToString();
                        ViewBag.Message = _QCParameterDefinition1.MessagePrmDef;
                        //Session["MessagePrmDef"] = null;
                        _QCParameterDefinition1.MessagePrmDef = null;
                        //Session["MessagePrmDef"] = null;
                        if (ViewBag.Message == "Duplicate")
                        {
                            //_QCParameterDefinition1.param_name = Session["Pname"].ToString();
                            _QCParameterDefinition1.param_name = _QCParameterDefinition1.Pname;
                            //_QCParameterDefinition1.param_type = Session["Ptype"].ToString();
                            _QCParameterDefinition1.param_type = _QCParameterDefinition1.Ptype;
                            //_QCParameterDefinition1.param_Id = Convert.ToInt32(Session["paramId"].ToString());
                            _QCParameterDefinition1.param_Id = Convert.ToInt32(_QCParameterDefinition1.paramId);
                            _QCParameterDefinition1.BtnName = "Update";
                            //Session["Pname"] = null;
                            //Session["Ptype"] = null;
                            //Session["paramId"] = null;
                        }
                        else if (ViewBag.Message == "Save")
                        {
                            _QCParameterDefinition1.param_name = null;
                            _QCParameterDefinition1.param_type = null;
                            _QCParameterDefinition1.param_Id = 0;
                            _QCParameterDefinition1.BtnName = "Save";
                        }
                        else
                        {
                            _QCParameterDefinition1.BtnName = "Save";
                        }
                        //Session["MessageHsn"] = null;
                        _QCParameterDefinition1.MessageHsn = null;
                    }
                    else if (Pname != null && Ptype != null)
                    {
                        _QCParameterDefinition1.param_name = Pname;
                        _QCParameterDefinition1.param_type = Ptype;
                        _QCParameterDefinition1.param_Id = Convert.ToInt32(paramId);
                        _QCParameterDefinition1.BtnName = "Update";
                    }
                    else
                    {

                        _QCParameterDefinition1.BtnName = "Save";
                    }
                    _QCParameterDefinition1.hdnSavebtn = null;
                    return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ParameterDefinition/ParameterDefinition.cshtml", _QCParameterDefinition1);
                }
                else
                {
                    ViewBag.CreatedBy = userid;
                    ViewBag.CreatedDate = System.DateTime.Now.ToLongDateString();
                    QCParameterDefinition _QCParameterDefinition = new QCParameterDefinition();
                    dt = GetItemList();
                    _QCParameterDefinition.Title = title;
                    List<ParameterList> modelList = new List<ParameterList>();
                    //  var value = _QCItemParameter_ISERVICES.GetItemParameterList();
                    foreach (DataRow dr in dt.Rows)
                    {

                        ParameterList _ParameterList = new ParameterList();

                        _ParameterList.create_id = dr["create_id"].ToString();
                        _ParameterList.mod_id = dr["mod_id"].ToString();
                        _ParameterList.param_Id = Convert.ToInt32(dr["param_Id"]);
                        _ParameterList.param_name = dr["param_name"].ToString();
                        _ParameterList.param_type_val = dr["param_type_val"].ToString();
                        _ParameterList.param_type = dr["param_type"].ToString();
                        _ParameterList.create_dt = dr["CreateDate"].ToString();
                        _ParameterList.mod_dt = dr["modefiedDate"].ToString();
                        // _ParameterList.mod_id = Convert.ToInt32(dr["create_id"]);
                        modelList.Add(_ParameterList);
                    }
                    _QCParameterDefinition.ParamDefinitionList = modelList;
                    //if (Session["MessagePrmDef"] != null)
                    if (_QCParameterDefinition.MessagePrmDef != null)
                    {
                        //ViewBag.Message = Session["MessagePrmDef"].ToString();
                        ViewBag.Message = _QCParameterDefinition.MessagePrmDef;
                        //Session["MessagePrmDef"] = null;
                        _QCParameterDefinition.MessagePrmDef = null;
                        //Session["MessagePrmDef"] = null;
                        if (ViewBag.Message == "Duplicate")
                        {
                            //_QCParameterDefinition.param_name = Session["Pname"].ToString();
                            _QCParameterDefinition.param_name = _QCParameterDefinition.Pname;
                            //_QCParameterDefinition.param_type = Session["Ptype"].ToString();
                            _QCParameterDefinition.param_type = _QCParameterDefinition.Ptype;
                            //_QCParameterDefinition.param_Id = Convert.ToInt32(Session["paramId"].ToString());
                            _QCParameterDefinition.param_Id = Convert.ToInt32(_QCParameterDefinition.paramId);
                            _QCParameterDefinition.BtnName = "Update";
                            //Session["Pname"] = null;
                            //Session["Ptype"] = null;
                            //Session["paramId"] = null;
                        }
                        else
                        {
                            _QCParameterDefinition.BtnName = "Save";
                        }
                        //Session["MessageHsn"] = null;
                        _QCParameterDefinition.MessageHsn = null;
                    }
                    else if (Pname != null && Ptype != null)
                    {
                        _QCParameterDefinition.param_name = Pname;
                        _QCParameterDefinition.param_type = Ptype;
                        _QCParameterDefinition.param_Id = Convert.ToInt32(paramId);
                        _QCParameterDefinition.BtnName = "Update";
                    }
                    else
                    {

                        _QCParameterDefinition.BtnName = "Save";
                    }
                    _QCParameterDefinition.hdnSavebtn = null;
                    return View("~/Areas/BusinessLayer/Views/QualityControlSetup/ParameterDefinition/ParameterDefinition.cshtml", _QCParameterDefinition);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
                    branchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    userid = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchID, userid, DocumentMenuId, language);
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


        public ActionResult SaveParameterDef(QCParameterDefinition QCParamModel, string Command)
        {
            try
            {
                switch (Command)
                {
                    case "Save":
                        //Session["BtnPrmDef"] = Command;
                        QCParamModel.BtnPrmDef = Command;
                        QCParameterItemSave(QCParamModel, QCParamModel.param_name, QCParamModel.param_type, 0);
                        TempData["ModelData"] = QCParamModel;
                        if (QCParamModel.MessagePrmDef == "Duplicate")
                        {
                            QCParamModel.hdnSavebtn = null;
                            var Pname = QCParamModel.Pname;
                            var Ptype = QCParamModel.Ptype;
                            var paramId = QCParamModel.paramId;
                            return RedirectToAction("ParameterDefinition", new { Pname, Ptype, paramId });
                        }
                        return RedirectToAction("ParameterDefinition");

                    case "Update":
                        //Session["BtnPrmDef"] = Command;
                        QCParamModel.BtnPrmDef = Command;
                        QCParameterItemSave(QCParamModel, QCParamModel.param_name, QCParamModel.param_type, QCParamModel.param_Id);
                        TempData["ModelData"] = QCParamModel;
                        if (QCParamModel.MessagePrmDef == "Duplicate")
                        {

                            var Pname = QCParamModel.Pname;
                            var Ptype = QCParamModel.Ptype;
                            var paramId = QCParamModel.paramId;
                            return RedirectToAction("ParameterDefinition", new { Pname, Ptype, paramId });
                        }
                        return RedirectToAction("ParameterDefinition");
                }

                return RedirectToAction("ParameterDefinition");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult QCParameterItemSave(QCParameterDefinition QCParamModel, string Pname, string Ptype, int paramId)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string Comp_ID = string.Empty;
                    string UserID = string.Empty;
                    string TransType = string.Empty;
                    string Output = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["UserId"] != null)
                    {
                        UserID = Session["UserId"].ToString();
                    }
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();

                    if (!string.IsNullOrEmpty(Pname) && !string.IsNullOrEmpty(Ptype) && paramId == 0)
                    {
                        TransType = "Insert";
                        DataSet Outputds = _QCParameterDefinition_ISERVICES.QCItemParameterSave(Convert.ToInt32(Comp_ID), Convert.ToInt32(UserID), Pname, Ptype, TransType, SystemDetail, paramId);
                        Output = Outputds.Tables[0].Rows[0]["Result"].ToString();

                    }
                    else if (Pname != string.Empty && paramId != 0)
                    {
                        TransType = "Update";
                        DataSet Outputds = _QCParameterDefinition_ISERVICES.QCItemParameterSave(Convert.ToInt32(Comp_ID), Convert.ToInt32(UserID), Pname, Ptype, TransType, SystemDetail, paramId);

                        Output = Outputds.Tables[0].Rows[0]["Result"].ToString();

                    }
                    if (Output == "Update" || Output == "Save")
                    {

                        //Session["MessagePrmDef"] = "Save";
                        QCParamModel.MessagePrmDef = "Save";
                    }
                    if (Output == "Duplicate")
                    {
                        QCParamModel.hdnSavebtn = null;
                        //Session["MessagePrmDef"] = "Duplicate";
                        //Session["Pname"] = IsNull(Pname, "");
                        //Session["Ptype"] = IsNull(Ptype, "");
                        //Session["paramId"] = IsNull(paramId.ToString(), "");
                        QCParamModel.MessagePrmDef = "Duplicate";
                        QCParamModel.Pname = IsNull(Pname, "");
                        QCParamModel.Ptype = IsNull(Ptype, "");
                        QCParamModel.paramId = IsNull(paramId.ToString(), "");


                    }
                    return RedirectToAction("ParameterDefinition");


                }
                catch (Exception ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, ex);
                    return Json("ErrorPage");
                }

            }
            else
            {

                return View("ParameterDefinition");
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

        public ActionResult QCParameterItemDelete(int paramId)
        {
            // JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                QCParameterDefinition QCParamModel = new QCParameterDefinition();
                string Validate = string.Empty;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                DataTable DTDetail = new DataTable();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                UserID = "0";
                // int ModId = 0;
                string Pname = "";
                string Ptype = "";
                string SystemDetail = "";
                var TransType = "Delete";
                DataSet value = _QCParameterDefinition_ISERVICES.QCItemParameterSave(Convert.ToInt32(Comp_ID), Convert.ToInt32(UserID), Pname, Ptype, TransType, SystemDetail, paramId);
                //Validate = Json(value.Tables[0].Rows[0]["Result"].ToString());
                Validate = value.Tables[0].Rows[0]["Result"].ToString();
                //Session["MessagePrmDef"] = Validate;
                QCParamModel.MessagePrmDef = Validate;
                TempData["ModelData"] = QCParamModel;
                return RedirectToAction("ParameterDefinition");


            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

            //return Validate;
        }

        [NonAction]
        private DataTable GetItemList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _QCParameterDefinition_ISERVICES.GetItemParameterList(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                CommonController com_obj = new CommonController();
                DataTable PardefDetail = new DataTable();
                DataSet obj_ds = new DataSet();

                PardefDetail.Columns.Add("Parameter Name*(max 50 characters)", typeof(string));
                PardefDetail.Columns.Add("Parameter Type*", typeof(string));
                obj_ds.Tables.Add(PardefDetail);

                CommonController obj = new CommonController();
                string filePath = obj.CreateExcelFile("ImportQCParameterTemplate", Server);
                com_obj.AppendExcel(filePath, null, obj_ds, "QCParameterDefinition");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
                    DataTable PDDetail = new DataTable();
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

                                string DetailQuery = "SELECT DISTINCT * FROM [ParameterDefinition$] WHERE LEN([Parameter Name*(max 50 characters)]) > 0;";

                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(PDDetail);

                                ds.Tables.Add(PDDetail);
                                DataSet dts = VerifyData(ds, uploadStatus);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImporPDdata = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialImportQC_PD_Detail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet ds, string uploadStatus)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            DataTable dts = PrepareDataset(ds);
            if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _QCParameterDefinition_ISERVICES.GetVerifiedDataOfExcel(compId, dts);
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
        public DataTable PrepareDataset(DataSet ds)
        {
            DataTable PDDetail = new DataTable();
            PDDetail.Columns.Add("param_name", typeof(string));
            PDDetail.Columns.Add("param_type", typeof(string));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataTable dspd = ds.Tables[0];
                DataRow dtr = PDDetail.NewRow();
                dtr["param_name"] = dspd.Rows[i][0].ToString().Trim();
                dtr["param_type"] = dspd.Rows[i][1].ToString().Trim();
                PDDetail.Rows.Add(dtr);
            }
            return PDDetail;
        }
        public ActionResult ShowValidationError(string paramname)
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
                    }
                    DataSet ds = new DataSet();
                    DataTable PDDetail = new DataTable();
                    
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
                                string  DetailQuery = "SELECT DISTINCT * FROM [ParameterDefinition$] WHERE [Parameter Name*(max 50 characters)] = '" + paramname + "';";

                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(PDDetail);

                                ds.Tables.Add(PDDetail);
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
            DataTable dts = PrepareDataset(ds);
            DataTable result = _QCParameterDefinition_ISERVICES.ShowExcelErrorDetail(compId, dts);
            return result;
        }
        public JsonResult ImportPDDetailFromExcel()
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
                    DataTable PDDetail = new DataTable();
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
                                string DetailQuery = "SELECT DISTINCT * FROM [ParameterDefinition$] WHERE LEN([Parameter Name*(max 50 characters)]) > 0;";
                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = DetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(PDDetail);
                                
                                ds.Tables.Add(PDDetail);
                                string msg = SavePDFromExcel(ds);
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
        private string SavePDFromExcel(DataSet ds)
        {
            string compId = "";
            string UserID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataTable dts = PrepareDataset(ds);
            string result = _QCParameterDefinition_ISERVICES.BulkImportPDDetail(compId, UserID, dts);
            return result;
        }
    }
}



