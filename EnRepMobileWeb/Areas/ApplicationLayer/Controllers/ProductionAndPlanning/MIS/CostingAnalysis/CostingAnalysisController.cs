using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MIS.CostingAnalysis
{
    public class CostingAnalysisController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105105155115";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        public CostingAnalysisController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/CostingAnalysis
        public ActionResult CostingAnalysis()
        {
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MIS/CostingAnalysis/CostingAnalysis.cshtml");
        }

    }

}