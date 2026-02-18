using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.NPD.SampleExecution
{
    public class SampleExecutionController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105109135", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        public SampleExecutionController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/SampleExecution
        public ActionResult SampleExecution()
        {
            ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/NPD/SampleExecution/SampleExecutionList.cshtml");
        }

        public ActionResult AddSampleExecutionDetail()
        {
            Session["Message"] = "New";
            Session["Command"] = "Add";
            Session["AppStatus"] = 'D';
            Session["TransType"] = "Save";
            Session["BtnName"] = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("SampleExecutionDetail", "SampleExecution");
        }
        public ActionResult SampleExecutionDetail()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/NPD/SampleExecution/SampleExecutionDetail.cshtml");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult SampleExecutionMain()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/NPD/SampleExecution/SampleExecutionMain.cshtml");
            }
            catch (Exception ex)
            {
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