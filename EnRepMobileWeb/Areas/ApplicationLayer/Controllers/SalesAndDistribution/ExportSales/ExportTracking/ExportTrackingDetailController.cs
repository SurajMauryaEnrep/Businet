using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.ExportSales.ExportTracking
{
    public class ExportTrackingDetailController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105103145135", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        public ExportTrackingDetailController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/ExportTrackingDetail
        public ActionResult ExportTrackingDetail()
        {
            ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ExportSales/ExportTracking/ExportTrackingDetail.cshtml");
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