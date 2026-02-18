using EnRepMobileWeb.MODELS.BusinessLayer.ItemSearch;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemSearch;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.ItemSearch
{
    public class ItemSearchController : Controller
    {
        string CompID, BranchId, language, UserID = String.Empty;
        string DocumentMenuId = "103107", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ItemSearch_ISERVICES _ItemSearch_IServices;
        public ItemSearchController(Common_IServices _Common_IServices, ItemSearch_ISERVICES _ItemSearch_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._ItemSearch_IServices = _ItemSearch_IServices;
        }
        // GET: BusinessLayer/ItemSearch
        public ActionResult ItemSearch()
        {
            try
            {
                ItemSearch_MODELS itemsearch_Model = new ItemSearch_MODELS();
                GetCompDeatil();
                //  GetItemList(itemsearch_Model);
                List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                ItemName_List _ItemList = new ItemName_List();
                _ItemList.Item_ID = "0";
                _ItemList.Item_Name = "---Select---";
                _ItemList1.Add(_ItemList);
                itemsearch_Model.ItemNameList = _ItemList1;
                ViewBag.MenuPageName = getDocumentName();
                itemsearch_Model.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/BusinessLayer/Views/ItemSearch/ItemSearch.cshtml", itemsearch_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        private string getDocumentName()
        {
            try
            {
                GetCompDeatil();
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

        
        public async Task<ActionResult> GetItemListFilter(string ItemID)
        {
            try
            {
                ItemSearch_MODELS itemsearch_Model = new ItemSearch_MODELS();
                GetCompDeatil();

                // Async call for data retrieval
                DataSet dt = await Task.Run(() => _ItemSearch_IServices.GetItemListDetail(CompID, BranchId, ItemID));

                if (dt.Tables[0].Rows.Count > 0)
                {
                    itemsearch_Model.ItemName = dt.Tables[0].Rows[0]["item_name"].ToString();
                    itemsearch_Model.ItemNameID = dt.Tables[0].Rows[0]["item_id"].ToString();
                    itemsearch_Model.UomName = dt.Tables[0].Rows[0]["uom_name"].ToString();
                    itemsearch_Model.OEMNo = dt.Tables[0].Rows[0]["item_oem_no"].ToString();
                    itemsearch_Model.AliasName = dt.Tables[0].Rows[0]["item_sam_cd"].ToString();
                    itemsearch_Model.ReferenceNumber = dt.Tables[0].Rows[0]["item_leg_cd"].ToString();
                    itemsearch_Model.TechnicalSpecification = dt.Tables[0].Rows[0]["item_tech_spec"].ToString();
                    itemsearch_Model.TechnicalDescription = dt.Tables[0].Rows[0]["item_tech_des"].ToString();
                    itemsearch_Model.GroupName = dt.Tables[0].Rows[0]["groupname"].ToString();
                    itemsearch_Model.HSNCode = dt.Tables[0].Rows[0]["HSN_code"].ToString();
                    itemsearch_Model.remarks = dt.Tables[0].Rows[0]["item_remarks"].ToString();
                    itemsearch_Model.WeightInKg = dt.Tables[0].Rows[0]["wght_kg"].ToString();
                    itemsearch_Model.VolumeInLitres = dt.Tables[0].Rows[0]["wght_ltr"].ToString();
                    itemsearch_Model.GrossWeight = dt.Tables[0].Rows[0]["gr_wght"].ToString();
                    itemsearch_Model.NetWeight = dt.Tables[0].Rows[0]["nt_wght"].ToString();
                    itemsearch_Model.actstatus = dt.Tables[0].Rows[0]["act_status"].ToString();
                    ViewBag.AttechmentDetails = dt.Tables[1];
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = itemsearch_Model.ItemNameID;
                    _ItemList.Item_Name = itemsearch_Model.ItemName;
                    _ItemList1.Add(_ItemList);
                    itemsearch_Model.ItemNameList = _ItemList1;

                }
                else
                {
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = "0";
                    _ItemList.Item_Name = "---Select---";
                    _ItemList1.Add(_ItemList);
                    itemsearch_Model.ItemNameList = _ItemList1;
                }
                ViewBag.MenuPageName = getDocumentName();
                itemsearch_Model.Title = title;
              
                //GetItemList(itemsearch_Model);
                itemsearch_Model.ddl_ItemName = ItemID;
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/BusinessLayer/Views/ItemSearch/ItemSearch.cshtml", itemsearch_Model);
               
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
     
        public void GetCompDeatil()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BranchId = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }
        }

        public  ActionResult GetItemList(ItemSearch_MODELS queryParameters)
        {
            GetCompDeatil();
            DataTable itemList1 = new DataTable();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddl_ItemName))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ddl_ItemName;
                }
                itemList1 = _ItemSearch_IServices.BindItemList(ItemName, CompID, BranchId);

                //List<ItemName_List> _ItemList = new List<ItemName_List>();
                //foreach (DataRow data in itemList1.Rows)
                //{
                //    ItemName_List _ItemDetail = new ItemName_List();
                //    _ItemDetail.Item_ID = data["Item_id"].ToString();
                //    _ItemDetail.Item_Name = data["Item_name"].ToString();
                //    _ItemList.Add(_ItemDetail);
                //}
                //queryParameters.ItemNameList = _ItemList;
                Common_Model _Model = new Common_Model();
                _Model.cmn_data = Json(JsonConvert.SerializeObject(itemList1)).Data.ToString();
                return PartialView("~/Areas/Common/Views/Cmn_DataContainer.cshtml", _Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
           
        }
        [HttpGet]
        public ActionResult PartialAttachmentDetail()
        {
            try
            {
                ViewBag.AttechmentDetails = null;
                return PartialView("~/Areas/Common/Views/Comn_PartialAttatchmentDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public FileResult GetitemsearchGenratePdfFile(string itemId,


            string PrintItemName,
            string ShowUOM,
            string ShowOEMNumber,
            string ShowItemAliasName,
           string ShowRefNumber,
            string ShowProdTechSpec,
            string ProdTechDesc,
            string ItemGroupName,
           string HsnNumber,
            string Remarks,
           string WeightinKG,
            string Volumeinliter,
            string grossweight,
            string Netweight,
           string PrintItemImage)


  
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintItemName", typeof(string));
            dt.Columns.Add("ShowUOM", typeof(string));
            dt.Columns.Add("ShowOEMNumber", typeof(string));
            dt.Columns.Add("ShowItemAliasName", typeof(string));
            dt.Columns.Add("ShowRefNumber", typeof(string));
            dt.Columns.Add("ShowProdTechSpec", typeof(string));
            dt.Columns.Add("ProdTechDesc", typeof(string));
            dt.Columns.Add("ItemGroupName", typeof(string));
            dt.Columns.Add("HsnNumber", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("WeightinKG", typeof(string));
            dt.Columns.Add("Volumeinliter", typeof(string));
            dt.Columns.Add("grossweight", typeof(string));
            dt.Columns.Add("Netweight", typeof(string));
            dt.Columns.Add("PrintItemImage", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["PrintItemName"] = PrintItemName;
            dtr["ShowUOM"] = ShowUOM;
            dtr["ShowOEMNumber"] = ShowOEMNumber;
            dtr["ShowItemAliasName"] = ShowItemAliasName;
            dtr["ShowRefNumber"] = ShowRefNumber;
            dtr["ShowProdTechSpec"] = ShowProdTechSpec;
            dtr["ProdTechDesc"] = ProdTechDesc;
            dtr["ItemGroupName"] = ItemGroupName;
            dtr["HsnNumber"] = HsnNumber;
            dtr["Remarks"] = Remarks;
            dtr["WeightinKG"] = WeightinKG;
            dtr["Volumeinliter"] = Volumeinliter;
            dtr["grossweight"] = grossweight;
            dtr["Netweight"] = Netweight;
            dtr["PrintItemImage"] = PrintItemImage;
           
           
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            return GenratePdfFile(itemId);
        }

            [HttpPost]
        public FileResult GenratePdfFile(string itemId)
        {
            

            ViewBag.DocumentMenuId = DocumentMenuId;
            return File(GetPdfData(itemId), "application/pdf", "ItemSearch.pdf");

        }
        public byte[] GetPdfData(string itemId)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _ItemSearch_IServices.GetPrintItemSearchDeatils(CompID, BranchId, itemId);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[1].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.Title = "Item Detail";
                ViewBag.Details = Details.Tables[0];
                ViewBag.ItemDetails = Details.Tables[1];
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/BusinessLayer/Views/ItemSearch/ItemSearchPrint.cshtml"));


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
    }

}