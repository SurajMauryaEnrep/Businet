using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.MISOrderIntimation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISOrderIntimation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.MISOrderIntimation
{
    public class MISOrderIntimationController : Controller
    {
        string CompID,BrId, userid, language = String.Empty;
        string DocumentMenuId = "105103190125", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        OrderIntimation_IService _OrderIntimation_IService;
        public MISOrderIntimationController(Common_IServices _Common_IServices, OrderIntimation_IService _OrderIntimation_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._OrderIntimation_IService = _OrderIntimation_IService;
        }
        // GET: ApplicationLayer/MISOrderIntimation
        public ActionResult MISOrderIntimation()
        {
            CommonPageDetails();
            OrderIntimation_Model model = new OrderIntimation_Model();
            List<OrdNumberList> shipNoList = new List<OrdNumberList>();
            shipNoList.Add(new OrdNumberList { Order_number = "---Select---"/*, Order_date = "0" */});
            model.OrderNumbers = shipNoList;

            var range = CommonController.Comman_GetFutureDateRange();
            model.From_dt = range.FromDate;
            model.To_dt = range.ToDate;
            ViewBag.DocumentMenuId = "105103190125";
            ViewBag.SalesOrderIntimationList = GetOrderIntimationDetails("0", model.From_dt, model.To_dt, "P", "0","0","0");
            model.SalePersonList = GetSalesPersonList();
            model.ItemsList = GetItemsList();
            model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISOrderIntimation/MISOrderIntimation.cshtml", model);
        }
        public ActionResult GetOrderIntimationByFilter(string cust_id, string From_dt, string To_dt, string OrderType, string OrderNumber,string SalesPerson,string ItemId)
        {
            try
            {
                ViewBag.SalesOrderIntimationList = GetOrderIntimationDetails(cust_id, From_dt, To_dt, OrderType, OrderNumber,SalesPerson, ItemId);
                ViewBag.ODFilter = "SDFilter";
                ViewBag.DocumentMenuId = "105103190125";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_OrderIntimationList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public DataTable GetOrderIntimationDetails(string cust_id, string From_dt, string To_dt, string OrderType, string OrderNumber,string SalesPerson,string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    userid = Session["UserId"].ToString();
                }
                ds = _OrderIntimation_IService.GetOrderIntimationDetail(CompID, BrId, userid, cust_id, From_dt, To_dt, OrderType, OrderNumber, SalesPerson, ItemId);
                dt = ds.Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderIntimationCommand(OrderIntimation_Model model, string command)
        {
            try
            {
                if (model.hdnCommand == "Print")
                {
                    command = "Print";
                }
                switch (command) 
                {
                    case "Print":
                        return GenratePdfFile(model);
                    default:
                        return new EmptyResult();
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public FileResult GenratePdfFile(OrderIntimation_Model model)
        {
            DataTable SodataTable = new DataTable();
            SodataTable.Columns.Add("so_no", typeof(string));
            SodataTable.Columns.Add("so_dt", typeof(string));
            SodataTable.Columns.Add("Item_id", typeof(string));

            if (model.hdnIntimationList != null)
            {
                JArray jObject = JArray.Parse(model.hdnIntimationList);           
                for (int i = 0; i < jObject.Count; i++)
                {   
                    DataRow rows = SodataTable.NewRow();
                    rows["so_no"] = jObject[i]["so_no"].ToString();
                    rows["so_dt"] = jObject[i]["so_dt"].ToString();
                    rows["Item_id"] = jObject[i]["item_id"].ToString();
                    SodataTable.Rows.Add(rows);
                }     
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("ShowItemName", typeof(string));
            dt.Columns.Add("ShowUOM", typeof(string));
            dt.Columns.Add("ShowRefrenceNumber", typeof(string));
            dt.Columns.Add("ShowTechSpec", typeof(string));
            dt.Columns.Add("ShowTechDesc", typeof(string));
            dt.Columns.Add("ShowWeight", typeof(string));
            dt.Columns.Add("ShowCustName", typeof(string));
            dt.Columns.Add("ShowPrice", typeof(string));
            dt.Columns.Add("ShowHSN", typeof(string));
            dt.Columns.Add("ShowPendingAmt", typeof(string));
            dt.Columns.Add("ShowBomAvl", typeof(string));
            dt.Columns.Add("ShowCustSpecItemDesc", typeof(string));
            dt.Columns.Add("ShowOEMNo", typeof(string));
            dt.Columns.Add("ShowOrderedQuantity", typeof(string));
            dt.Columns.Add("ShowRemarks", typeof(string));

            DataRow dtr = dt.NewRow();
            dtr["ShowItemName"] = model.ShowItemName;
            dtr["ShowUOM"] = model.ShowUom;
            dtr["ShowRefrenceNumber"] = model.ShowRefNumber;
            dtr["ShowTechSpec"] = model.ShowTechSpec;
            dtr["ShowTechDesc"] = model.ShowTechDesc;
            dtr["ShowWeight"] = model.ShowWeight;
            dtr["ShowCustName"] = model.ShowCustName;
            dtr["ShowPrice"] = model.ShowPrice;
            dtr["ShowHSN"] = model.ShowHSN;
            dtr["ShowPendingAmt"] = model.ShowPendingAmt;
            dtr["ShowBomAvl"] = model.ShowBomAvl;
            dtr["ShowCustSpecItemDesc"] = model.ShowCustSpecItemDesc;
            dtr["ShowOEMNo"] = model.ShowOEMNo;
            dtr["ShowOrderedQuantity"] = model.ShowOrderedQuantity;
            dtr["ShowRemarks"] = model.ShowRemarks;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;

            return File(GetPdfData(SodataTable), "application/pdf", "OrderIntimation.pdf");

        }
        public byte[] GetPdfData(DataTable SodataTable)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string htmlcontent = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                Byte[] bytes = null;
                DataSet Details = new DataSet();
                string LogoImage = string.Empty;
                ViewBag.PageName = "SO";
                ViewBag.Title = "Order Intimation";

                DataSet ds = _OrderIntimation_IService.GetIntimationDetail(CompID, BrId, SodataTable);
                ViewBag.Details1 = ds;
                ViewBag.ItemCountDetails = ds.Tables[1];
                ViewBag.TotalDetails = ds.Tables[2];
                ViewBag.DocumentMenuId = "105103190125";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + ds.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                DataTable dt = ds.Tables[0];
                ViewBag.Details = dt?.AsEnumerable().Take(9).CopyToDataTable();

                int batchSize = 9;
                int total = ds.Tables[0].Rows.Count;
                //htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/intimation.cshtml"));
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISOrderIntimation/MISIntimation.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    for (int i = 9; i < total; i += batchSize)
                    {
                        ViewBag.Details = null;
                        DataTable dt1 = ds.Tables[0];
                        ViewBag.Details = dt1.AsEnumerable().Skip(i).Take(batchSize).CopyToDataTable();
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISOrderIntimation/PartialOrderIntimation.cshtml"));
                        var sr = new StringReader(htmlcontent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }
                    pdfDoc.Close();
                    bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font fontnormal = new Font(bf1, 9, Font.NORMAL);
                    Font fontbold = new Font(bf1, 9, Font.BOLD);
                    Font fonttitle = new Font(bf1, 15, Font.BOLD);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                }
                return bytes.ToArray();
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //public byte[] GetPdfData1(DataTable SodataTable)
        //{
        //    StringReader reader = null;
        //    Document pdfDoc = null;
        //    PdfWriter writer = null;
        //    try
        //    {
        //        string htmlcontent = "";
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrId = Session["BranchId"].ToString();
        //        }
        //        Byte[] bytes = null;
        //        DataSet Details = new DataSet();
        //        string LogoImage = string.Empty;
        //        ViewBag.PageName = "SO";
        //        ViewBag.Title = "Order Intimation";

        //        DataSet ds = _OrderIntimation_IService.GetIntimationDetail(CompID, BrId, SodataTable);
        //        ViewBag.Details = ds;
        //        ViewBag.DocumentMenuId = "105103190125";
        //        string path1 = Server.MapPath("~") + "..\\Attachment\\";
        //        string LogoPath = path1 + ds.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
        //        ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
        //        //htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/intimation.cshtml"));
        //        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/MISOrderIntimation/MISIntimation.cshtml"));
        //        using (MemoryStream stream = new System.IO.MemoryStream())
        //        {
        //            reader = new StringReader(htmlcontent);
        //            pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
        //            writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
        //            pdfDoc.Close();
        //            bytes = stream.ToArray();
        //            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
        //            string draftImage = Server.MapPath("~/Content/Images/draft.png");
        //            BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //            Font fontnormal = new Font(bf1, 9, Font.NORMAL);
        //            Font fontbold = new Font(bf1, 9, Font.BOLD);
        //            Font fonttitle = new Font(bf1, 15, Font.BOLD);
        //            using (var reader1 = new PdfReader(bytes))
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    using (var stamper = new PdfStamper(reader1, ms))
        //                    {
        //                        var draftimg = Image.GetInstance(draftImage);
        //                        draftimg.SetAbsolutePosition(0, 160);
        //                        draftimg.ScaleAbsolute(580f, 580f);

        //                        int PageCount = reader1.NumberOfPages;
        //                        for (int i = 1; i <= PageCount; i++)
        //                        {
        //                            var content = stamper.GetUnderContent(i);
        //                            if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
        //                            {
        //                                content.AddImage(draftimg);
        //                            }
        //                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
        //                        }
        //                    }
        //                    bytes = ms.ToArray();
        //                }
        //            }
        //        }
        //        return bytes.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
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
        private void CommonPageDetails()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                userid = Session["UserId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            ViewBag.DocumentMenuId = "105103190125";
            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrId, userid, DocumentMenuId, language);
            ViewBag.AppLevel = ds.Tables[0];
            ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
            string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
            ViewBag.VBRoleList = ds.Tables[3];
            ViewBag.StatusList = ds.Tables[4];
            ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
        }
        [HttpPost]
        public JsonResult GetoOrderIntimationSONoLists(string Cust_id, string Curr_Id, string doc_id ,string From_dt,string To_dt,string OrderType)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //DataSet result = _DomesticSaleInvoice_ISERVICE.GetShipmentList(Cust_id, Comp_ID, Br_ID);
                DataSet result = _OrderIntimation_IService.GetoOrderIntimationSONoList(Comp_ID, Br_ID, Cust_id, Curr_Id, doc_id, From_dt, To_dt, OrderType);
                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                Drow[2] = "0";
                Drow[3] = "0";

                result.Tables[0].Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private List<SalePersonList> GetSalesPersonList()
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                DataTable dt = _OrderIntimation_IService.GetSalesPersonList(CompID, BrId, userid);
                List<SalePersonList> slsperslist = new List<SalePersonList>();
                foreach (DataRow dr in dt.Rows)
                {
                    SalePersonList slspers = new SalePersonList
                    {
                        salep_id = dr["sls_pers_id"].ToString(),
                        salep_name = dr["sls_pers_name"].ToString()
                    };
                    slsperslist.Add(slspers);
                }
                slsperslist.Insert(0, new SalePersonList { salep_id = "0", salep_name = "---Select---" });
                return slsperslist;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private List<ItemsModel> GetItemsList()
        {
            try
            {
                string compId = string.Empty;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();

                DataTable dt = _OrderIntimation_IService.BindGetItemList("", compId, BrId);
                List<ItemsModel> itemsList = new List<ItemsModel>();
                itemsList.Insert(0, new ItemsModel { ItemId = "0", ItemName = "---All---" });
                return itemsList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }

}
