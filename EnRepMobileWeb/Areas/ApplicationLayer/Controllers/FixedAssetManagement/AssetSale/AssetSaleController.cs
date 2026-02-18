using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FixedAssetManagement.AssetSale
{
    public class AssetSaleController : Controller
    {
        string CompID, language = String.Empty;
        string DocumentMenuId = "105106121", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        public AssetSaleController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/AssetSale
        public ActionResult AssetSale()
        {
            ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetSale/AssetSaleList.cshtml");
        }

        public ActionResult AddAssetSaleDetail()
        {
            Session["Message"] = "New";
            Session["Command"] = "Add";
            Session["AppStatus"] = 'D';
            Session["TransType"] = "Save";
            Session["BtnName"] = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("AssetSaleDetail", "AssetSale");
        }
        public ActionResult AssetSaleDetail()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/FixedAssetManagement/AssetSale/AssetSaleDetail.cshtml");
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