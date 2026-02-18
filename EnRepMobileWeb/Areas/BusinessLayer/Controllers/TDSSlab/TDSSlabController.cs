using EnRepMobileWeb.MODELS.BusinessLayer.TDSSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TDSSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.TDSSlab
{
    public class TDSSlabController : Controller
    {
        string CompID, branchId, userId, language, title = String.Empty;
        string DocumentMenuId = "103153";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        TDSSlab_ISERVICES _tdsIservices;
        public TDSSlabController(Common_IServices _Common_IServices, TDSSlab_ISERVICES tds_Iservices)
        {
            this._Common_IServices = _Common_IServices;
            this._tdsIservices = tds_Iservices;
        }
        // GET: BusinessLayer/TDSSlab
        public ActionResult TDSSlab()
        {
            try
            {
                TDSSlab_Model _model = new TDSSlab_Model();
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                _model.Title = title;
                _model.Message = TempData["Message"]==null?null:TempData["Message"].ToString();
                DataSet ds = _tdsIservices.GetTDSList(CompID);
                _model.TDSList = ds.Tables[0];

                List<TdsAccList> _AccList = new List<TdsAccList>();
                _AccList.Add(new TdsAccList { tds_acc_id = "0", tds_acc_name = "---Select---" });
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _AccList.Add(new TdsAccList { tds_acc_id = dr["tax_id"].ToString(), tds_acc_name = dr["tax_name"].ToString() });
                }
                _model.tdsAccLists = _AccList;
                return View("~/Areas/BusinessLayer/Views/TDSSlab/TDSSlab.cshtml", _model);
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
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchId, userId, DocumentMenuId, language);
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
        public ActionResult TDSSlabSave(TDSSlab_Model _tdsModel, string Command)
        {
            try
            {
                if (_tdsModel.HdnCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {

                    case "Save":
                        TempData["Message"] = TDS_SaveDetails(_tdsModel, Command);
                        _tdsModel.hdnSavebtn = null;
                        return RedirectToAction("TDSSlab");
                    case "Update":
                        TempData["Message"] = TDS_SaveDetails(_tdsModel, Command);
                        _tdsModel.hdnSavebtn = null;
                        return RedirectToAction("TDSSlab");
                    case "Delete":
                        TempData["Message"] = TDS_DeleteDetails(_tdsModel, Command);
                        _tdsModel.hdnSavebtn = null;
                        return RedirectToAction("TDSSlab");
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
        private string TDS_SaveDetails(TDSSlab_Model _tdsModel,string TransType)
        {
            string result = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                _tdsModel.comp_id = CompID;
                result = _tdsIservices.TDS_SlabInserUpdate(_tdsModel, TransType);
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private string TDS_DeleteDetails(TDSSlab_Model _tdsModel, string TransType)
        {
            string result = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                result = _tdsIservices.TDS_SlabDelete(CompID, _tdsModel.slab_id, TransType);
                return result;
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

    }

}