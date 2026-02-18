using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.Dashboard.Controllers.TutorialVideos
{
    public class TutorialVideosController : Controller
    {
        // GET: Dashboard/TutorialVideos
        public ActionResult Index()
        {
            return View("~/Areas/Dashboard/Views/TutorialVideos/TutorialVideos.cshtml");
        }
    }
}