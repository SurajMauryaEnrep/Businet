using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.PlanningAnalysis
{
    public class PlanningAnalysisController : Controller
    {
        string CompID, language, title = String.Empty;
        string DocumentMenuId = "105105155101";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        public PlanningAnalysisController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/PlanningAnalysis
        public ActionResult PlanningAnalysis()
        {
            ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/PlanningAnalysis/PlanningAnalysis.cshtml");
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