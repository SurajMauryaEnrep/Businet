using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.Common.Controllers
{
    public class PageInfoController : Controller
    {
        // GET: Common/PageInfo
        public ActionResult Index()
        {
            return View("~/Areas/Common/Views/PageInfo/PageInfo.cshtml");
        }
    }
}