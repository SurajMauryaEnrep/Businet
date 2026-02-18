
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.Filter;
using EnRepMobileWeb.Models;

namespace EnRepMobileWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FileDownloadAttribute());
            // filters.Add(new LogCustomExceptionFilter());
           // filters.Add(new PreventFromUrl());
        }
    }
}
