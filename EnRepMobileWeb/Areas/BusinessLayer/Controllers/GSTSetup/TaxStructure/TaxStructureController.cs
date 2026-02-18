using EnRepMobileWeb.MODELS.BusinessLayer.GSTSetup.TaxStructure;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxStructure;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.GSTSetup.TaxStructure
{
    public class TaxStructureController : Controller
    {
        string Comp_ID, branchId, userId, language, title,UserID = String.Empty;
        string DocumentMenuId = "103156005";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        TaxStructure_ISERVICE _TaxStructure_ISERVICE;
        TaxStructureModel _TaxStructureModel;
        DataTable dt;
        public TaxStructureController(Common_IServices _Common_IServices, TaxStructure_ISERVICE _TaxStructure_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._TaxStructure_ISERVICE = _TaxStructure_ISERVICE;
        }
        // GET: BusinessLayer/TaxStructure
        public ActionResult TaxStructure(/*TaxStructureModel _TaxStructureModel*/)
        {
            try
            {
                //_TaxStructureModel = new TaxStructureModel();
                CommonPageDetails();
                var _TaxStructureModel = TempData["ModelData"] as TaxStructureModel;
               if (_TaxStructureModel != null)
                {
                    //ViewBag.MenuPageName = getDocumentName();
                    _TaxStructureModel.Title = title;
                    string Comp_ID = string.Empty;
                    //_TaxStructureModel.igst_tax_id = 0;

                    //string TaxCode = _TaxStructureModel.TaxCode.ToString();commented by sm on 07-12-2024
                    string TaxCode = _TaxStructureModel.TaxCode;

                    #region Commented By Nitesh 08-04-2024 For All DropDown And List Data In One Procedure

                    //dt = GetTaxPercList();
                    //List<TaxPerc> _TaxPercList = new List<TaxPerc>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    TaxPerc _PercList = new TaxPerc();
                    //    _PercList.tax_id = dt["tax_id"].ToString();
                    //    _PercList.tax_perc = dt["tax_perc"].ToString();
                    //    _TaxPercList.Add(_PercList);
                    //}
                    //_TaxPercList.Insert(0, new TaxPerc() { tax_id = "0", tax_perc = "---Select---" });
                    //if (Convert.ToInt32(TaxCode) != 0)
                    //{
                    //    _TaxPercList.Insert(0, new TaxPerc() { tax_id = TaxCode, tax_perc = TaxCode });
                    //}
                    //_TaxStructureModel.TaxPercList = _TaxPercList;
                    #endregion
                    #region Commented By Nitesh 08-04-2024 For All DropDown And List Data In One Procedure
                    //dt = GetTaxList();
                    //List<Tax> _TaxList = new List<Tax>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    Tax _List = new Tax();
                    //    _List.tax_id = dt["tax_id"].ToString();
                    //    _List.tax_name = dt["tax_name"].ToString();
                    //    _TaxList.Add(_List);
                    //}
                    //_TaxList.Insert(0, new Tax() { tax_id = "0", tax_name = "---Select---" });
                    //_TaxStructureModel.TaxList = _TaxList;
                    #endregion
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    #region Commented By Nitesh 08-04-2024 For All DropDown And List Data In One Procedure
                    //  ViewBag.TaxStructureList = GetTaxStructureDetailList();
                    #endregion



                    #region Commented By Nitesh 08-04-2024 For All List Data In One Procedure
                    //if (_TaxStructureModel.TransType == "Update" || _TaxStructureModel.Command == "Edit")
                    //{
                    //    //string TaxCode = _TaxStructureModel.TaxCode.ToString();

                    //    if (Session["CompId"] != null)
                    //    {
                    //        Comp_ID = Session["CompId"].ToString();
                    //    }

                    //    DataSet ds1 = _TaxStructure_ISERVICE.GetTaxStructureViewDetail(TaxCode, Comp_ID);
                    //    _TaxStructureModel.igst_tax_id = ds1.Tables[0].Rows[0]["igst_tax_id"].ToString();
                    //    _TaxStructureModel.rcm_igst_tax_id = ds1.Tables[0].Rows[0]["rcm_igst_tax_id"].ToString();
                    //    _TaxStructureModel.igst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["igst_tax_per"].ToString());
                    //    _TaxStructureModel.sgst_tax_id = ds1.Tables[0].Rows[0]["sgst_tax_id"].ToString();
                    //    _TaxStructureModel.rcm_sgst_tax_id = ds1.Tables[0].Rows[0]["rcm_sgst_tax_id"].ToString();
                    //    _TaxStructureModel.sgst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["sgst_tax_per"].ToString());
                    //    _TaxStructureModel.cgst_tax_id = ds1.Tables[0].Rows[0]["cgst_tax_id"].ToString();
                    //    _TaxStructureModel.rcm_cgst_tax_id = ds1.Tables[0].Rows[0]["rcm_cgst_tax_id"].ToString();
                    //    _TaxStructureModel.cgst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["cgst_tax_per"].ToString());

                    //}
                    #endregion

                    GetAllData(_TaxStructureModel);
                    if (_TaxStructureModel.Message == "Deleted")
                    {
                        TempData["RemoveMsg"] = "NotRemoveMsg";
                    }
                    if (TempData["RemoveMsg"] == null)
                    {
                        _TaxStructureModel.Message = null;
                    }
                    TempData["RemoveMsg"] = null;
                    return View("~/Areas/BusinessLayer/Views/GSTSetup/TaxStructure/TaxStructure.cshtml", _TaxStructureModel);
                }
                else
                {
                    TaxStructureModel _TaxStructureModel1 = new TaxStructureModel();
                    //ViewBag.MenuPageName = getDocumentName();
                    _TaxStructureModel1.Title = title;
                    string Comp_ID = string.Empty;
                    //_TaxStructureModel1.igst_tax_id = 0;

                    string TaxCode = _TaxStructureModel1.TaxCode;
                    //string TaxCode = _TaxStructureModel1.TaxCode.ToString();
                    #region Commented By Nitesh 08-04-2024 For All DropDown And List Data In One Procedure
                    //dt = GetTaxPercList();
                    //List<TaxPerc> _TaxPercList = new List<TaxPerc>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    TaxPerc _PercList = new TaxPerc();
                    //    _PercList.tax_id = dt["tax_id"].ToString();
                    //    _PercList.tax_perc = dt["tax_perc"].ToString();
                    //    _TaxPercList.Add(_PercList);
                    //}
                    //_TaxPercList.Insert(0, new TaxPerc() { tax_id = "0", tax_perc = "---Select---" });
                    //if (Convert.ToInt32(TaxCode) != 0)
                    //{
                    //    _TaxPercList.Insert(0, new TaxPerc() { tax_id = TaxCode, tax_perc = TaxCode });
                    //}
                    //_TaxStructureModel1.TaxPercList = _TaxPercList;

                    //dt = GetTaxList();
                    //List<Tax> _TaxList = new List<Tax>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    Tax _List = new Tax();
                    //    _List.tax_id = dt["tax_id"].ToString();
                    //    _List.tax_name = dt["tax_name"].ToString();
                    //    _TaxList.Add(_List);
                    //}
                    //_TaxList.Insert(0, new Tax() { tax_id = "0", tax_name = "---Select---" });
                    //_TaxStructureModel1.TaxList = _TaxList;
                    #endregion
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    #region Commented By Nitesh 08-04-2024 For All DropDown And List Data In One Procedure
                    // ViewBag.TaxStructureList = GetTaxStructureDetailList();
                    #endregion


                    #region Commented By Nitesh 08-04-2024 For All List Data In One Procedure
                    //if (_TaxStructureModel1.TransType == "Update" || _TaxStructureModel1.Command == "Edit")
                    //{
                    //    //string TaxCode = _TaxStructureModel1.TaxCode.ToString();

                    //    if (Session["CompId"] != null)
                    //    {
                    //        Comp_ID = Session["CompId"].ToString();
                    //    }

                    //    DataSet ds1 = _TaxStructure_ISERVICE.GetTaxStructureViewDetail(TaxCode, Comp_ID);
                    //    _TaxStructureModel1.igst_tax_id = ds1.Tables[0].Rows[0]["igst_tax_id"].ToString();
                    //    _TaxStructureModel1.rcm_igst_tax_id = ds1.Tables[0].Rows[0]["rcm_igst_tax_id"].ToString();
                    //    _TaxStructureModel1.igst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["igst_tax_per"].ToString());
                    //    _TaxStructureModel1.sgst_tax_id = ds1.Tables[0].Rows[0]["sgst_tax_id"].ToString();
                    //    _TaxStructureModel1.rcm_sgst_tax_id = ds1.Tables[0].Rows[0]["rcm_sgst_tax_id"].ToString();
                    //    _TaxStructureModel1.sgst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["sgst_tax_per"].ToString());
                    //    _TaxStructureModel1.cgst_tax_id = ds1.Tables[0].Rows[0]["cgst_tax_id"].ToString();
                    //    _TaxStructureModel1.rcm_cgst_tax_id = ds1.Tables[0].Rows[0]["rcm_cgst_tax_id"].ToString();
                    //    _TaxStructureModel1.cgst_tax_perc = float.Parse(ds1.Tables[0].Rows[0]["cgst_tax_per"].ToString());

                    //}
                    #endregion

                    GetAllData(_TaxStructureModel1);
                    _TaxStructureModel1.TransType = "Save";
                    _TaxStructureModel1.BtnName = "BtnAddNew";
                    _TaxStructureModel1.Message = "New";
                    _TaxStructureModel1.Command = "Add";
                    if (_TaxStructureModel1.Message == "Deleted")
                    {
                        TempData["RemoveMsg"] = "NotRemoveMsg";
                    }
                    if (TempData["RemoveMsg"] == null)
                    {
                        _TaxStructureModel1.Message = null;
                    }
                    TempData["RemoveMsg"] = null;
                    return View("~/Areas/BusinessLayer/Views/GSTSetup/TaxStructure/TaxStructure.cshtml", _TaxStructureModel1);
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
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(Comp_ID, branchId, userId, DocumentMenuId, language);
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
        private void GetAllData(TaxStructureModel _TaxStructureModel)
        {
            try
            {
                #region Added By Nitesh 08-04-2024 For All Data in One Procedure 
                #endregion
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (_TaxStructureModel.TransType == "Update" || _TaxStructureModel.Command == "Edit")
                {
                    _TaxStructureModel.Flag = "DblClickData";
                }
                string TaxCode = _TaxStructureModel.TaxCode;
                //string TaxCode = _TaxStructureModel.TaxCode.ToString();
                DataSet GetAllData = _TaxStructure_ISERVICE.GetAllData(Comp_ID, _TaxStructureModel.Flag, TaxCode);


                /**-------------------------------Bind Tax PercenTage DropDown List----------------------**/
                List<TaxPerc> _TaxPercList = new List<TaxPerc>();
                foreach (DataRow Data in GetAllData.Tables[0].Rows)
                {
                    TaxPerc _PercList = new TaxPerc();
                    _PercList.tax_id = Data["tax_id"].ToString();
                    _PercList.tax_perc = Data["tax_perc"].ToString();
                    _TaxPercList.Add(_PercList);
                }
                _TaxPercList.Insert(0, new TaxPerc() { tax_id = "0", tax_perc = "---Select---" });
                //if (Convert.ToInt32(TaxCode) != 0)
                if (TaxCode != "0" && TaxCode != null)
                {
                    _TaxPercList.Insert(0, new TaxPerc() { tax_id = TaxCode, tax_perc = TaxCode });
                }
                _TaxStructureModel.TaxPercList = _TaxPercList;
                /**-------------------------------------------End------------------------------------**/
                /**-------------------Bind Tax DropDown List---------------------------------**/
                List<Tax> _TaxList = new List<Tax>();
                foreach (DataRow Data in GetAllData.Tables[1].Rows)
                {
                    Tax _List = new Tax();
                    _List.tax_id = Data["tax_id"].ToString();
                    _List.tax_name = Data["tax_name"].ToString();
                    _TaxList.Add(_List);
                }
                _TaxList.Insert(0, new Tax() { tax_id = "0", tax_name = "---Select---" });
                _TaxStructureModel.TaxList = _TaxList;
                /**----------------------------End------------------------------------------**/
                /*-------------------------------Get List Data-------------------------------------**/
                ViewBag.TaxStructureList = GetAllData.Tables[2];
                /**---------------------End---------------------------------------**/

                if(_TaxStructureModel.Flag== "DblClickData")
                {
                    if (GetAllData.Tables[3].Rows.Count > 0)
                    {
                        _TaxStructureModel.igst_tax_id = GetAllData.Tables[3].Rows[0]["igst_tax_id"].ToString();
                        _TaxStructureModel.rcm_igst_tax_id = GetAllData.Tables[3].Rows[0]["rcm_igst_tax_id"].ToString();
                        _TaxStructureModel.igst_tax_perc = float.Parse(GetAllData.Tables[3].Rows[0]["igst_tax_per"].ToString());
                        _TaxStructureModel.sgst_tax_id = GetAllData.Tables[3].Rows[0]["sgst_tax_id"].ToString();
                        _TaxStructureModel.rcm_sgst_tax_id = GetAllData.Tables[3].Rows[0]["rcm_sgst_tax_id"].ToString();
                        _TaxStructureModel.sgst_tax_perc = float.Parse(GetAllData.Tables[3].Rows[0]["sgst_tax_per"].ToString());
                        _TaxStructureModel.cgst_tax_id = GetAllData.Tables[3].Rows[0]["cgst_tax_id"].ToString();
                        _TaxStructureModel.rcm_cgst_tax_id = GetAllData.Tables[3].Rows[0]["rcm_cgst_tax_id"].ToString();
                        _TaxStructureModel.cgst_tax_perc = float.Parse(GetAllData.Tables[3].Rows[0]["cgst_tax_per"].ToString());
                    }
                    
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AddNewTaxStructure()
        {
            TaxStructureModel _TaxStructureModel = new TaxStructureModel();

            _TaxStructureModel.AppStatus = "D";
            _TaxStructureModel.TransType = "Save";
            _TaxStructureModel.BtnName = "BtnAddNew";
            _TaxStructureModel.Message = "New";
            _TaxStructureModel.Command = "Add";
            //_TaxStructureModel.TaxCode =0;//commented by sm on 07-12-2024
            _TaxStructureModel.TaxCode ="0";
            TempData["ModelData"] = _TaxStructureModel;
            return RedirectToAction("TaxStructure", "TaxStructure", _TaxStructureModel);
        }
        public ActionResult EditTaxStructure(string Tax_id,string EditData)
        {
            TaxStructureModel _TaxStructureModel = new TaxStructureModel();
           
            _TaxStructureModel.Message = "New";
            _TaxStructureModel.Command = "Edit";
            //_TaxStructureModel.TaxCode = Convert.ToInt32(Tax_id);//commented by sm on 07-12-2024
            _TaxStructureModel.TaxCode = Tax_id;
            _TaxStructureModel.AppStatus = "D";
            _TaxStructureModel.TransType = "Update";
            _TaxStructureModel.EditData = EditData;
            TempData["ModelData"] = _TaxStructureModel;
            return RedirectToAction("TaxStructure", "TaxStructure");
        }
       
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(Comp_ID, DocumentMenuId, language);
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
        public ActionResult SaveTaxStructure(TaxStructureModel _TaxStructureModel, string command)
        {
            try
            {
                if (_TaxStructureModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                string tax_perc;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                switch (command)
                {                   

                    case "Delete":                       
                       
                        _TaxStructureModel.Command = command;
                        //tax_perc = _TaxStructureModel.TaxCode.ToString(); 
                        tax_perc = _TaxStructureModel.TaxCode; 
                        string SaveMessage = _TaxStructure_ISERVICE.DeleteTaxDetail(tax_perc, CompID);                       
                        _TaxStructureModel.AppStatus = "D";
                        _TaxStructureModel.TransType = "Save";
                        _TaxStructureModel.BtnName = "BtnAddNew";
                        _TaxStructureModel.Message = "Deleted";
                        _TaxStructureModel.Command = "Add";
                        //_TaxStructureModel.TaxCode = 0;//commented by sm on 07-12-2024
                        _TaxStructureModel.TaxCode = "0";
                        _TaxStructureModel.DeleteCommand = null;
                        _TaxStructureModel.hdnSavebtn = null;
                        TempData["ModelData"] = _TaxStructureModel;
                        return RedirectToAction("TaxStructure");

                    case "Save":                       
                        _TaxStructureModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            if (_TaxStructureModel.TransType == "Update")
                            {
                                _TaxStructureModel.TransType = "Update";
                            }
                            else
                            {
                                _TaxStructureModel.TransType = "Save";
                            }
                            
                            InsertTaxStructure(_TaxStructureModel);
                            //_TaxStructureModel.TaxCode = _TaxStructureModel.TaxCode; 
                            //_TaxStructureModel = null;
                            _TaxStructureModel.igst_tax_perc = null;
                            _TaxStructureModel.igst_tax_id = null;
                            _TaxStructureModel.rcm_igst_tax_id = null;
                            _TaxStructureModel.cgst_tax_id = null;
                            _TaxStructureModel.rcm_cgst_tax_id = null;
                            _TaxStructureModel.sgst_tax_id = null;
                            _TaxStructureModel.rcm_sgst_tax_id = null;
                            _TaxStructureModel.sgst_tax_perc = null;
                            _TaxStructureModel.cgst_tax_perc = null;
                            _TaxStructureModel.DeleteCommand = null;
                            _TaxStructureModel.AppStatus = "D";
                            //_TaxStructureModel.TransType = "Save";
                            _TaxStructureModel.BtnName = "BtnAddNew";
                            _TaxStructureModel.Message = "Save";
                            _TaxStructureModel.Command = "Add";
                            //_TaxStructureModel.TaxCode = 0;//commented by sm on 07-12-2024
                            _TaxStructureModel.TaxCode = "0";
                            _TaxStructureModel.hdnSavebtn = null;
                            TempData["RemoveMsg"] = "RemoveMsg";
                        }
                        TempData["ModelData"] = _TaxStructureModel;
                        return RedirectToAction("TaxStructure");

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
        public ActionResult InsertTaxStructure(TaxStructureModel _TaxStructureModel)
        {
            try
            {

                if (Session["compid"] != null)
                {
                    Comp_ID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }

                DataTable TaxStructure = new DataTable();
              
                DataTable TaxDt = new DataTable();
                TaxDt.Columns.Add("TransType", typeof(string));
                TaxDt.Columns.Add("comp_id", typeof(int));
                TaxDt.Columns.Add("igst_tax_id", typeof(int));
                //TaxDt.Columns.Add("igst_tax_perc", typeof(float));              
                TaxDt.Columns.Add("igst_tax_perc", typeof(string));              
                TaxDt.Columns.Add("rcm_igst_tax_id", typeof(int));
                TaxDt.Columns.Add("sgst_tax_id", typeof(int));
                //TaxDt.Columns.Add("sgst_tax_perc", typeof(float));                
                TaxDt.Columns.Add("sgst_tax_perc", typeof(string));                
                TaxDt.Columns.Add("rcm_sgst_tax_id", typeof(int));
                TaxDt.Columns.Add("cgst_tax_id", typeof(int));
                //TaxDt.Columns.Add("cgst_tax_perc", typeof(float));               
                TaxDt.Columns.Add("cgst_tax_perc", typeof(string));               
                TaxDt.Columns.Add("rcm_cgst_tax_id", typeof(int));

                DataRow TaxDtrow = TaxDt.NewRow();
                                
                TaxDtrow["TransType"] = _TaxStructureModel.TransType;
                TaxDtrow["comp_id"] = Comp_ID;
                TaxDtrow["igst_tax_id"] = _TaxStructureModel.igst_tax_id;
                TaxDtrow["igst_tax_perc"] = _TaxStructureModel.igst_tax_perc;
                TaxDtrow["rcm_igst_tax_id"] = _TaxStructureModel.rcm_igst_tax_id;
                TaxDtrow["sgst_tax_id"] = _TaxStructureModel.sgst_tax_id;
                TaxDtrow["sgst_tax_perc"] = _TaxStructureModel.sgst_tax_perc;               
                TaxDtrow["rcm_sgst_tax_id"] = _TaxStructureModel.rcm_sgst_tax_id;
                TaxDtrow["cgst_tax_id"] = _TaxStructureModel.cgst_tax_id;
                TaxDtrow["cgst_tax_perc"] = _TaxStructureModel.cgst_tax_perc;
                TaxDtrow["rcm_cgst_tax_id"] = _TaxStructureModel.rcm_cgst_tax_id;


                TaxDt.Rows.Add(TaxDtrow);

                TaxStructure = TaxDt;

                String SaveMessage = _TaxStructure_ISERVICE.InsertGstTaxStructureDetail(TaxStructure);

                //string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                //string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                if (SaveMessage == "Update" || SaveMessage == "Save")
                {
                    _TaxStructureModel.Message = "Save";
                    //_TaxStructureModel.TaxCode = Convert.ToInt32(TaxCode);
                    _TaxStructureModel.TransType = "Save";
                }
                else
                {
                    _TaxStructureModel.Message = "";
                    //_TaxStructureModel.TaxCode = Convert.ToInt32(TaxCode);
                    _TaxStructureModel.TransType = "";
                }

                _TaxStructureModel.hdnSavebtn = null;
                //TempData["ModelData"] = _TaxStructureModel;
                TempData["ModelData"] = _TaxStructureModel;
                return RedirectToAction("TaxStructure");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private DataTable GetTaxStructureDetailList()
        {
            try
            {
                
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }                
                DataTable OpBalList = _TaxStructure_ISERVICE.GetTaxStructureDetail(Comp_ID);

                return OpBalList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetTaxPercList()
        {
            try
            {
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TaxStructure_ISERVICE.GetTaxPercDetail(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null; ;
            }

        }
        [NonAction]
        private DataTable GetTaxList()
        {
            try
            {
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TaxStructure_ISERVICE.GetTaxListDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null; ;
            }

        }


    }

}
