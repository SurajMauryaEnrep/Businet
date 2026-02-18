using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Models
{
    public class FileDownloadAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as FileResult;
            if (result != null)
            {
                HttpContext.Current.Response.Cookies["fileDownLoad"].Expires = DateTime.Now.AddSeconds(-1);
            }
            base.OnActionExecuted(filterContext);
        }
    }
}