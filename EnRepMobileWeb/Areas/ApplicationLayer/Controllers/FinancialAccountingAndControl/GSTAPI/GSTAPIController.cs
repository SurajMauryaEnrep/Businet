using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GSTAPI;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GSTAPI;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZXing;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GSTAPI
{
    public class GSTAPIController : Controller
    {
        #region Code wrote by - Sanjay Prasad(from the beginning of the page), Dated - 06-10-2023, Description- GST API Integration 
        #endregion
        string DocumentMenuId = "105104133", title;
        string compId, brId, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly GSTAPI_IService _gstApi;
        private readonly DomesticSaleInvoice_ISERVICE _LocalSalesInvoice_ISERVICES;
        private readonly CustomInvoice_ISERVICE _CustomInvoice_ISERVICE;
        public GSTAPIController(Common_IServices _Common_IServices, GSTAPI_IService gstApi, DomesticSaleInvoice_ISERVICE LocalSalesInvoice_ISERVICES,
            CustomInvoice_ISERVICE CustomInvoice_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            _gstApi = gstApi;
            _LocalSalesInvoice_ISERVICES = LocalSalesInvoice_ISERVICES;
            _CustomInvoice_ISERVICE = CustomInvoice_ISERVICE;
        }
        // GET: ApplicationLayer/GSTAPI
        public ActionResult GSTAPI()
        {
            ViewBag.MenuPageName = getDocumentName();
            GstApiModel _model = new GstApiModel();
            DateTime dtnow = DateTime.Now;
            string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            _model.Title = title;
            _model.FromDate = FromDate;
            _model.ToDate = ToDate;
            _model.FinMonthYear = GetFinMonthYear();
            #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- Return empty view 
            #endregion

            //ViewBag.DocLink = GenerateEinvIrnPdf("", "", "", "");
            //EWBDetailsPDF("", "", "", "");
            //EWBSummaryPDF("", "", "", "");
            //GenerateEinvIrnPdf("", "", "", "");
            // var result = Task.Run(async () => await GetPdfEinvIRN()).ConfigureAwait(false).GetAwaiter().GetResult();
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GSTAPI/GSTAPI.cshtml", _model);

        }
        //Getting MonthYear Name to Reconsile GST Data on EaseMyGst
        private List<FinMonthYearModel> GetFinMonthYear()
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            List<FinMonthYearModel> objModel = new List<FinMonthYearModel>();
            DataTable dttbl = _gstApi.GetFinMonthYearForGSTR(compId, brId);
            if (dttbl.Rows.Count > 0)
            {
                for (int i = 0; i < dttbl.Rows.Count; i++)
                {
                    var data = new FinMonthYearModel()
                    {
                        FinMonthYearName = dttbl.Rows[i]["FInMonthYear"].ToString(),
                        MnthYear = dttbl.Rows[i]["MnthYear"].ToString()
                    };
                    objModel.Add(data);
                }
            }
            return objModel;
        }
        //PDF Generation Test
        public string GetEinvoiceIRNPDF(string invType, string invNo, string returnYear, string returnMonth)
        {
            return Task.Run(async () => await GetPdfEinvIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        //Sales invoice details
        DataSet GetSlsInvDetail(string fromDate, string toDate, string dataType, string docStatus, string GSTR_DateOption,string GstCat)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                //  GenerateEinvIrnPdf("", "", "", "");
                DataSet ds = _gstApi.GetGSTAPIPostingDetails(compId, brId, fromDate, toDate, dataType, docStatus, GSTR_DateOption, GstCat);
                return ds;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //Get items details on click info icon 
        DataTable GetSlsItemDetails(string dataType, string invNo, string invDt)
        {
            // nEED TO CHANGE QUERY FOR ACK NO AND DATE
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataTable dt = _gstApi.GetSalesItemDetails(compId, brId, dataType, invNo, invDt);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //Getting sales item details
        public ActionResult SalesItemsDetails(string dataType, string invNo, string invDt)
        {
            try
            {
                ViewBag.action = dataType;
                ViewBag.SalesItemDetails = GetSlsItemDetails(dataType, invNo, invDt);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSaleInvItemDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        //Getting service sales item details
        public ActionResult ServiceSalesItemsDetails(string dataType, string invNo, string invDt)
        {
            try
            {
                ViewBag.ServiceSalesItemDetails = GetSlsItemDetails(dataType, invNo, invDt);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialServiceSaleInvItemDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        //Getting sales invoice details
        public ActionResult GetSlsInvDetails(string fromDate, string toDate, string dataType, string docStatus,string GSTR_DateOption,string GstCat)
        {
            try
            {
                #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- Bind GST API Details 
                #endregion
                ViewBag.FILTER = "FILTER";
                ViewBag.action = dataType;
                DataSet ds = GetSlsInvDetail(fromDate, toDate, dataType, docStatus, GSTR_DateOption, GstCat);
                if (ds.Tables.Count > 0)
                {
                    ViewBag.GstApiData = ds.Tables[0];
                }
                if (dataType == "SR" || dataType == "PR")
                {
                    if (ds.Tables.Count > 1)
                    {
                        ViewBag.SrtData = ds.Tables[1];
                        if(dataType == "SR")
                        {
                            ViewBag.GstApiOcData = ds.Tables[2];
                        }
                    }
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGSTAPISaleRegister.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGSTAPI.cshtml");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        // Getting document name
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    compId = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(compId, DocumentMenuId, language);
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
        //Sales invoice GST api json
        public string InvoiceGstApiBody(string invType, string invNo, string invDt)
        {
            #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- Return GST api posting details body 
            #endregion
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataSet ds = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                GstApiRequestModel objModel = new GstApiRequestModel();

                objModel.Version = "1.0";
                objModel.TranDtls = GetTransDetails(ds);
                objModel.DocDtls = GetDocDetails(ds);
                //objModel.SellerDtls = GetSellerDetails(ds);
                DataTable clientdtl = _gstApi.GetApiClientDetails(compId, brId);
                if (clientdtl.Rows.Count > 0)
                {
                    // emgorg (testing credientials)
                    // test-1234 (testing credientials)
                    if (clientdtl.Rows[0]["client_id"].ToString().ToLower() != "emgorg" && clientdtl.Rows[0]["client_id"].ToString().ToLower() != "test-1234")
                        objModel.SellerDtls = GetSellerDetails(ds);
                    else
                        objModel.SellerDtls = TempSellerDetails(ds);
                }
                //Bind buyer details
                objModel.BuyerDtls = BuyerDetails(ds);
                // Dispatch details
                objModel.DispDtls = DispatchDetails(ds);
                //objModel.ShipDtls = ExportInvShippingDetails(ds);
                //material shipping details
                objModel.ShipDtls = ShippingDetails(ds);
                // Bind all items list
                objModel.ItemList = GetItemsList(ds);
                objModel.ValDtls = GetValDetails(ds);
                objModel.PayDtls = GetPayDtls(ds);
                objModel.RefDtls = GetRefDtls(ds);
                objModel.AddlDocDtls = GetDocDtls(ds);
                objModel.ExpDtls = GetExportDetails(ds);
                //bind eway-bill details as empty 
                var ewbDetails = new EwbDtls()
                {
                    TransId = null,
                    TransName = null,
                    TransMode = null,
                    Distance = null,
                    TransDocDt = null,
                    TransDocNo = null,
                    VehNo = null,
                    VehType = null
                };
                objModel.EwbDtls = ewbDetails;
                var requestBody = JsonConvert.SerializeObject(objModel);
                return requestBody;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //Service sales invoice GST api json
        public string ServiceInvoiceGstApiBody(string invNo, string invDt)
        {
            #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- Return GST api posting details body 
            #endregion
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataSet ds = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDt);
                GstApiRequestModel objModel = new GstApiRequestModel();

                objModel.Version = "1.0";
                objModel.TranDtls = GetTransDetails(ds);
                objModel.DocDtls = GetDocDetails(ds);
                //objModel.SellerDtls = GetSellerDetails(ds);
                //objModel.SellerDtls = TempSellerDetails(ds);
                DataTable clientdtl = _gstApi.GetApiClientDetails(compId, brId);

                if (clientdtl.Rows.Count > 0)
                {
                    // emgorg (testing credientials)
                    // test-1234 (testing credientials)
                    if (clientdtl.Rows[0]["client_id"].ToString().ToLower() != "emgorg"&& clientdtl.Rows[0]["client_id"].ToString().ToLower() != "test-1234")
                        objModel.SellerDtls = GetSellerDetails(ds);
                    else
                        objModel.SellerDtls = TempSellerDetails(ds);
                }
                objModel.BuyerDtls = BuyerDetails(ds);
                objModel.DispDtls = DispatchDetails(ds);
                objModel.ShipDtls = ShippingDetails(ds);
                objModel.ItemList = GetItemsList(ds);
                objModel.ValDtls = GetValDetails(ds);
                objModel.PayDtls = GetPayDtls(ds);
                objModel.RefDtls = GetRefDtls(ds);
                objModel.AddlDocDtls = GetDocDtls(ds);
                objModel.ExpDtls = new ExpDtls();
                var ewbDetails = new EwbDtls()
                {
                    TransId = null,
                    TransName = null,
                    TransMode = null,
                    Distance = null,
                    TransDocDt = null,
                    TransDocNo = null,
                    VehNo = null,
                    VehType = null
                };
                objModel.EwbDtls = ewbDetails;

                var requestBody = JsonConvert.SerializeObject(objModel);
                return requestBody;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //EWB GST api json
        public string EWBGstApiBody(string invType, string invNo, string invDt)
        {
            #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- EWB Details GST API Body 
            #endregion
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataSet ds = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                GstApiRequestModel objModel = new GstApiRequestModel();

                objModel.DocDtls = GetDocDetails(ds);
                var sellerDetails = new SellerDtls
                {
                    Gstin = ds.Tables[1].Rows[0]["gst_no"].ToString()
                };
                objModel.SellerDtls = sellerDetails;
                List<ItemList> ItemsLst = new List<ItemList>();
                foreach (DataRow row in ds.Tables[4].Rows)
                {
                    var itemsDetails = new ItemList();
                    itemsDetails.SlNo = row["rowNo"].ToString();
                    ItemsLst.Add(itemsDetails);
                }
                objModel.ItemList = ItemsLst;
                objModel.EwbDtls = GetEWBDetails(ds);
                // Dispatch details
                objModel.DispDtls = DispatchDetails(ds);
                objModel.ShipDtls = ExportEWBShippingDetails(ds);
                var requestBody = JsonConvert.SerializeObject(objModel);
                return requestBody;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        ///////////////////////////////////////////
        //      Prepare data to send on GET     ///
        ///////////////////////////////////////////
        // Transporter details
        private TranDtls GetTransDetails(DataSet ds)
        {
            var objTxnDtl = new TranDtls()
            {
                TaxSch = "GST",
                SupTyp = ds.Tables[0].Rows[0]["SuppType"].ToString(),
                RegRev = "N",
                EcmGstin = null,
                IgstOnIntra = "N"
            };
            return objTxnDtl;
        }
        //Invoice Header details (Document Details)
        private DocDtls GetDocDetails(DataSet ds)
        {
            var docDetail = new DocDtls()
            {
                No = ds.Tables[0].Rows[0]["No"].ToString(),
                Dt = ds.Tables[0].Rows[0]["dt"].ToString(),
                Typ = ds.Tables[0].Rows[0]["Typ"].ToString(),
            };
            return docDetail;
        }
        // Seller details (here we are passing company details)
        private SellerDtls GetSellerDetails(DataSet ds)
        {
            var sellerDtl = new SellerDtls
            {
                Gstin = ds.Tables[1].Rows[0]["gst_no"].ToString(),
                LglNm = ds.Tables[1].Rows[0]["comp_nm"].ToString(),
                TrdNm = ds.Tables[1].Rows[0]["Trdnm"].ToString(),
                Addr1 = ds.Tables[1].Rows[0]["Comp_Add"].ToString(),
                Loc = ds.Tables[1].Rows[0]["Loc"].ToString(),
                Addr2 = ds.Tables[1].Rows[0]["Comp_Add2"].ToString(),
                Em = ds.Tables[1].Rows[0]["email_id1"].ToString(),
                Ph = ds.Tables[1].Rows[0]["cont_num1"].ToString(),
                Pin = ds.Tables[1].Rows[0]["pin"].ToString(),
                Stcd = ds.Tables[1].Rows[0]["stcd"].ToString()
            };
            return sellerDtl;
        }
        private SellerDtls TempSellerDetails(DataSet ds)
        {
            var sellerDtl = new SellerDtls
            {
                Gstin = "02AMBPG7773M002",
                LglNm = "EaseMyGST",
                TrdNm = "",
                Addr1 = "Himachal Pradesh",
                Loc = "Himachal Pradesh",
                Addr2 = null,
                Em = ds.Tables[1].Rows[0]["email_id1"].ToString(),
                Ph = ds.Tables[1].Rows[0]["cont_num1"].ToString(),
                Pin = "175034",
                Stcd = "HP"
            };
            return sellerDtl;
        }
        //private BuyerDtls GetBuyerDetails(DataSet ds)
        //{
        //    var buyerDetails = new BuyerDtls
        //    {
        //        Gstin = ds.Tables[2].Rows[0]["cust_gst_no"].ToString(),
        //        LglNm = ds.Tables[2].Rows[0]["cust_name"].ToString(),
        //        TrdNm = ds.Tables[2].Rows[0]["TrdNm"].ToString(),
        //        Pos = ds.Tables[2].Rows[0]["Pos"].ToString(),
        //        Addr1 = ds.Tables[2].Rows[0]["cust_add"].ToString(),
        //        Addr2 = null,
        //        Loc = ds.Tables[2].Rows[0]["Location"].ToString(),
        //        Pin = ds.Tables[2].Rows[0]["cust_pin"].ToString(),
        //        Stcd = ds.Tables[2].Rows[0]["Stcd"].ToString(),
        //        Ph = ds.Tables[2].Rows[0]["cont_num1"].ToString(),
        //        Em = ds.Tables[2].Rows[0]["cont_email"].ToString()
        //    };
        //    return buyerDetails;
        //}
        private BuyerDtls BuyerDetails(DataSet ds)
        {
            var buyerDetails = new BuyerDtls
            {
                Gstin = ds.Tables[2].Rows[0]["cust_gst_no"].ToString(),
                LglNm = ds.Tables[2].Rows[0]["cust_name"].ToString(),
                TrdNm = ds.Tables[2].Rows[0]["TrdNm"].ToString(),
                Pos = ds.Tables[2].Rows[0]["Pos"].ToString(),
                Addr1 = ds.Tables[2].Rows[0]["cust_add"].ToString(),
                Addr2 = ds.Tables[2].Rows[0]["cust_add2"].ToString(),
                Loc = ds.Tables[2].Rows[0]["Location"].ToString(),
                Pin = ds.Tables[2].Rows[0]["cust_pin"].ToString(),
                Stcd = ds.Tables[2].Rows[0]["Stcd"].ToString(),
                Ph = ds.Tables[2].Rows[0]["cont_num1"].ToString(),
                Em = ds.Tables[2].Rows[0]["cont_email"].ToString()
            };
            return buyerDetails;
        }
        //private DispDtls GetDispatchDetails(DataSet ds)
        //{
        //    var dispatchDetails = new DispDtls
        //    {
        //        Nm = ds.Tables[3].Rows[0]["cust_name"].ToString(),
        //        Addr1 = ds.Tables[3].Rows[0]["cust_add"].ToString(),
        //        Addr2 = null,
        //        Loc = ds.Tables[3].Rows[0]["Location"].ToString(),
        //        Pin = ds.Tables[3].Rows[0]["cust_pin"].ToString(),
        //        Stcd = ds.Tables[3].Rows[0]["Stcd"].ToString()
        //    };
        //    return dispatchDetails;
        //}
        //Dispatch Details
        private DispDtls DispatchDetails(DataSet ds)
        {
            var dispatchDetails = new DispDtls
            {
                Nm = ds.Tables[10].Rows[0]["Nm"].ToString(),
                Addr1 = ds.Tables[10].Rows[0]["disp_Addr1"].ToString(),
                Addr2 = ds.Tables[10].Rows[0]["disp_Addr2"].ToString(),
                Loc = ds.Tables[10].Rows[0]["Loc"].ToString(),
                Pin = ds.Tables[10].Rows[0]["disp_pin"].ToString(),
                Stcd = ds.Tables[10].Rows[0]["state_code"].ToString()
            };
            return dispatchDetails;
        }
        //Shipping Details
        private ShipDtls ShippingDetails(DataSet ds)
        {
            var shippingDetails = new ShipDtls
            {
                Gstin = ds.Tables[3].Rows[0]["cust_gst_no"].ToString(),
                LglNm = ds.Tables[3].Rows[0]["cust_name"].ToString(),
                TrdNm = ds.Tables[3].Rows[0]["TrdNm"].ToString(),
                Addr1 = ds.Tables[3].Rows[0]["cust_add"].ToString(),
                Addr2 = ds.Tables[3].Rows[0]["cust_add2"].ToString(),
                Loc = ds.Tables[3].Rows[0]["Location"].ToString(),
                Pin = ds.Tables[3].Rows[0]["cust_pin"].ToString(),
                Stcd = ds.Tables[3].Rows[0]["Stcd"].ToString()
            };
            return shippingDetails;
        }
        //Export invoice Shipping Details
        private ShipDtls ExportInvShippingDetails(DataSet ds)
        {
            var shippingDetails = new ShipDtls
            {
                Gstin = "",
                LglNm = "",
                TrdNm = "",
                Addr1 = "",
                Addr2 = "",
                Loc = "",
                Pin = "",
                Stcd = ""
            };
            return shippingDetails;
        }
        //Export EWB Shipping Details
        private ShipDtls ExportEWBShippingDetails(DataSet ds)
        {
            var shippingDetails = new ShipDtls
            {
                Gstin = "URP",
                LglNm = ds.Tables[3].Rows[0]["cust_name"].ToString(),
                TrdNm = ds.Tables[3].Rows[0]["cust_name"].ToString(),
                Addr1 = ds.Tables[3].Rows[0]["cust_add"].ToString(),
                Addr2 = ds.Tables[3].Rows[0]["cust_add2"].ToString(),
                Loc = ds.Tables[3].Rows[0]["PortCode"].ToString(),
                Pin = ds.Tables[3].Rows[0]["PortPin"].ToString(),
                Stcd = ds.Tables[3].Rows[0]["PortStcd"].ToString()
            };
            return shippingDetails;
        }
        // Get Items List
        private List<ItemList> GetItemsList(DataSet ds)
        {
            List<ItemList> ItemsLst = new List<ItemList>();
            foreach (DataRow row in ds.Tables[4].Rows)
            {
                string itemId = row["item_id"].ToString();
                DataView dvBatch = new DataView(ds.Tables[5]);
                dvBatch.RowFilter = "item_id='" + itemId + "'";
                DataTable dtBatch = dvBatch.ToTable();

                DataView dvAttr = new DataView(ds.Tables[6]);
                dvAttr.RowFilter = "item_id='" + itemId + "'";
                DataTable dtAttr = dvAttr.ToTable();

                List<BchDtls> batchList = new List<BchDtls>();
                foreach (DataRow batch in dtBatch.Rows)
                {
                    var batchDetail = new BchDtls
                    {
                        Nm = batch["batch_no"].ToString(),
                        Expdt = batch["expiry_date"].ToString(),
                        wrDt = batch["Wrdt"].ToString()
                    };
                    batchList.Add(batchDetail);
                }
                if (batchList.Count == 0)
                {
                    var batchDetail = new BchDtls
                    {
                        Nm = null,
                        Expdt = null,
                        wrDt = null
                    };
                    batchList.Add(batchDetail);
                }
                List<AttribDtls> attrList = new List<AttribDtls>();
                foreach (DataRow attr in dtAttr.Rows)
                {
                    var attrDetail = new AttribDtls
                    {
                        Nm = attr["attr_name"].ToString(),
                        Val = attr["attr_val_name"].ToString()
                    };
                    attrList.Add(attrDetail);
                }
                if (attrList.Count == 0)
                {
                    var attrDetail = new AttribDtls
                    {
                        Nm = null,
                        Val = null
                    };
                    attrList.Add(attrDetail);
                }

                var itemsDetails = new ItemList();

                itemsDetails.SlNo = row["rowNo"].ToString();
                itemsDetails.PrdDesc = row["item_name"].ToString();
                itemsDetails.IsServc = row["i_srvc"].ToString();
                itemsDetails.HsnCd = row["hsn_code"].ToString();
                itemsDetails.Barcde = row["BarCde"].ToString();
                itemsDetails.Qty = Convert.ToDecimal(row["Ship_Qty"].ToString());
                itemsDetails.FreeQty = Convert.ToDecimal(row["FreeQty"].ToString());
                itemsDetails.Unit = row["uom_alias"].ToString();
                try
                {
                    itemsDetails.UnitPrice = Convert.ToDecimal(row["item_rate"].ToString());
                }
                catch
                {
                    itemsDetails.UnitPrice = Convert.ToDecimal(row["item_inv_rate"].ToString());
                }
                itemsDetails.TotAmt = Convert.ToDecimal(row["item_gr_val"].ToString());
                itemsDetails.Discount = Convert.ToDecimal(row["item_disc_amt"].ToString());
                itemsDetails.PreTaxVal = Convert.ToDecimal(row["AssAmt"].ToString());
                itemsDetails.AssAmt = Convert.ToDecimal(row["AssAmt"].ToString());
                itemsDetails.GstRt = Convert.ToDecimal(row["GstRate"].ToString());
                itemsDetails.IgstAmt = Convert.ToDecimal(row["Igst"].ToString());
                itemsDetails.CgstAmt = Convert.ToDecimal(row["Cgst"].ToString());
                itemsDetails.SgstAmt = Convert.ToDecimal(row["Sgst"].ToString());
                itemsDetails.CesRt = Convert.ToDecimal(row["CesRt"].ToString());
                itemsDetails.CesAmt = Convert.ToDecimal(row["CesAmt"].ToString());
                itemsDetails.CesNonAdvlAmt = Convert.ToDecimal(row["CesNonAdvlAmt"].ToString());
                itemsDetails.StateCesRt = Convert.ToDecimal(row["StateCesRt"].ToString());
                itemsDetails.StateCesAmt = Convert.ToDecimal(row["StateCesAmt"].ToString());
                itemsDetails.StateCesNonAdvlAmt = Convert.ToDecimal(row["StateCesNonAdvlAmt"].ToString());
                itemsDetails.OthChrg = Convert.ToDecimal(row["item_oc_amt"].ToString());
                itemsDetails.TotItemVal = Convert.ToDecimal(row["item_net_val_bs"].ToString());
                itemsDetails.OrdLineRef = row["OrdLineRef"].ToString();
                itemsDetails.OrgCntry = row["OrgCntry"].ToString();
                itemsDetails.PrdSlNo = row["PrdSlNo"].ToString();
                itemsDetails.AttribDtls = attrList;
                itemsDetails.BchDtls = batchList;
                ItemsLst.Add(itemsDetails);
            }
            return ItemsLst;
        }
        // Values Details
        private ValDtls GetValDetails(DataSet ds)
        {
            var valdtl = new ValDtls
            {
                AssVal = Convert.ToDecimal(ds.Tables[7].Rows[0]["AssVal"]),
                CgstVal = Convert.ToDecimal(ds.Tables[7].Rows[0]["Cgst"]),
                SgstVal = Convert.ToDecimal(ds.Tables[7].Rows[0]["Sgst"]),
                IgstVal = Convert.ToDecimal(ds.Tables[7].Rows[0]["Igst"]),
                CesVal = 0,
                StCesVal = 0,
                Discount = Convert.ToDecimal(ds.Tables[7].Rows[0]["ItemDiscountVal"]),
                OthChrg = Convert.ToDecimal(ds.Tables[7].Rows[0]["item_oc_amt"]),
                RndOffAmt = Convert.ToDecimal(ds.Tables[7].Rows[0]["RndOffAmt"]),
                TotInvVal = Convert.ToDecimal(ds.Tables[7].Rows[0]["TotInvVal"]),
                TotInvValFc = Convert.ToDecimal(ds.Tables[7].Rows[0]["TotInvValFc"])
            };
            return valdtl;
        }
        //Payment Details --- Currently Sending Empty because this is not required
        private PayDtls GetPayDtls(DataSet ds)
        {
            var paydtl = new PayDtls
            {
                Nm = null,
                AccDet = null,
                Mode = null,
                FinInsBr = null,
                PayTerm = null,
                PayInstr = null,
                CrTrn = null,
                DirDr = null,
                CrDay = null,
                PaidAmt = null,
                PaymtDue = null
            };
            return paydtl;
        }
        // Reference details
        private RefDtls GetRefDtls(DataSet ds)
        {
            List<ContrDtls> cntrList = new List<ContrDtls>();
            var contr = new ContrDtls()
            {
                RecAdvRefr = null,
                RecAdvDt = null,
                TendRefr = null,
                ContrRefr = null,
                ExtRefr = null,
                ProjRefr = null,
                PORefr = null,
                PORefDt = null
            };
            cntrList.Add(contr);

            var docper = new DocPerdDtls()
            {
                InvEndDt = null,
                InvStDt = null
            };
            List<PrecDocDtls> precDocList = new List<PrecDocDtls>();
            var precdoc = new PrecDocDtls()
            {
                InvDt = null,
                InvNo = null,
                OthRefNo = null
            };
            precDocList.Add(precdoc);
            var refdtl = new RefDtls
            {
                ContrDtls = cntrList,
                DocPerdDtls = docper,
                InvRm = null,
                PrecDocDtls = precDocList
            };
            return refdtl;
        }
        // Document details
        private List<AddlDocDtls> GetDocDtls(DataSet ds)
        {
            List<AddlDocDtls> addlDocLst = new List<AddlDocDtls>();
            var addlDoc = new AddlDocDtls()
            {
                Url = null,
                Docs = null,
                Info = null
            };
            addlDocLst.Add(addlDoc);
            return addlDocLst;
        }
        // Export details
        private ExpDtls GetExportDetails(DataSet ds)
        {
            var exportDtl = new ExpDtls();
            if (ds.Tables[0].Rows[0]["inv_type"].ToString() == "D")
            {
                exportDtl.ShipBNo = null;
                exportDtl.ShipBDt = null;
                exportDtl.Port = null;
                exportDtl.RefClm = null;
                exportDtl.ForCur = null;
                exportDtl.CntCode = null;
                exportDtl.ExpDuty = null;
            }
            else
            {
                exportDtl.ShipBNo = ds.Tables[8].Rows[0]["sh_no"].ToString();
                exportDtl.ShipBDt = ds.Tables[8].Rows[0]["sh_date"].ToString();
                exportDtl.Port = null;
                exportDtl.RefClm = null;
                exportDtl.ForCur = ds.Tables[8].Rows[0]["curr_name"].ToString();
                exportDtl.CntCode = ds.Tables[8].Rows[0]["cust_cntry"].ToString();
                exportDtl.ExpDuty = null;
            }
            return exportDtl;
        }
        // Eway - Bill Details
        private EwbDtls GetEWBDetails(DataSet ds)
        {
            var ewbDetails = new EwbDtls();
            ewbDetails.TransId = "";
            ewbDetails.TransName = "";
            ewbDetails.TransMode = "";
            ewbDetails.Distance = "";
            ewbDetails.TransDocDt = "";
            ewbDetails.TransDocNo = "";
            ewbDetails.VehNo = "";
            ewbDetails.VehType = "";
            if (ds.Tables.Contains("Table9"))
            {
                ewbDetails.TransId = ds.Tables[9].Rows[0]["TransId"].ToString();
                ewbDetails.TransName = ds.Tables[9].Rows[0]["TransName"].ToString();
                ewbDetails.TransMode = ds.Tables[9].Rows[0]["TransMode"].ToString();
                ewbDetails.Distance = ds.Tables[9].Rows[0]["Distance"].ToString();
                ewbDetails.TransDocDt = ds.Tables[9].Rows[0]["TransDocDt"].ToString();
                ewbDetails.TransDocNo = ds.Tables[9].Rows[0]["TransDocNo"].ToString();
                ewbDetails.VehNo = ds.Tables[9].Rows[0]["VehNo"].ToString();
                ewbDetails.VehType = ds.Tables[9].Rows[0]["VehType"].ToString();
            }
            return ewbDetails;
        }
        /////////////////////////////////////////////
        //      Prepare data to send on GET  END  ///
        /////////////////////////////////////////////

        //Request from javascript method to send Envoice/Eway-Bill Data on GST portal
        // Submit data on GST api
        
        public string SubmitOnGstApi(string invType, string dataType, string invNo, string invDt)
        {
            try
            {
                //SI - Sale Invoice, SSI - Service Sale Invoice
                //SCI - Scrap Sale Invoice Added by Suraj Maurya on 02-08-2024
                if (dataType == "SI" || dataType == "SSI" || dataType == "SCI" || dataType == "IBS" || dataType == "SRT")
                {
                    return PostEnvoiceData(invType, dataType, invNo, invDt);
                }
                // SC - Sale Cancelled, SSC - Service Sale Cancelled
                else if (dataType == "SC" || dataType == "SSC" || dataType == "SCC" || dataType == "IBSC")
                {
                    string cancelRequestBody = GetCancelRequestBody(invType, invNo, invDt, dataType);

                    return CancelIrnEinvOrEwb(invType, cancelRequestBody, dataType, invNo, invDt);
                }
                // Sale Register
                else if (dataType == "SR")
                {
                    return PostEnvoiceData(invType, dataType, invNo, invDt);
                }
                else
                {
                    return "Invalid Data Type";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
        }
        //Generate cancel request body
        public string GetCancelRequestBody(string invType, string invNo, string invDt,string dataType)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataSet ds = new DataSet();
            if (dataType == "SC" || dataType == "SCC" || dataType == "IBSC")
            {
                ds = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
            }
            else if (dataType == "SSC")
            {
                ds = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDt);
            }
            if (ds.Tables.Count == 0)
            {
                return "Invalid Details";
            }
            var cancelReqBody = new List<CancelInvIrn>();
            cancelReqBody.Add(new CancelInvIrn
            {
                supplyType = "Outward",
                transactionType = "INV",
                invoiceNumber = ds.Tables[0].Rows[0]["No"].ToString(),
                invoiceDate = ds.Tables[0].Rows[0]["Dt"].ToString().Replace("-", "/"),
                reasonforEWBCancel = "1",
                reasonforIRNCancel = "1",
                ewbRemarks = "No Remark",
                irnRemarks = "No Remark"
            });
            string bodyData = JsonConvert.SerializeObject(cancelReqBody);
            return bodyData;
        }
        // Check E-invoice or EWB status
        public string EInvoiceEWBStatusCheck(string requestFor, string invNo, string invDt, string returnYear, string returnMonth, string invType)
        {
            #region Code wrote by - Sanjay Prasad, Dated - 06-10-2023, reason- Get EINV status after posting data on GST API (Used in Method - PostEnvoiceData) 
            #endregion
            string bearerToken = GenerateBearerToken();
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            string body = "/process/v1/einvoiceewbv1/reports/IRN-Response?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=INV&return_year=" + returnYear + "&return_month=" + returnMonth + "";
            if (requestFor == "SRT")/* Added by Suraj Maurya on 11-04-2025 */
            {
                body = "/process/v1/einvoiceewbv1/reports/IRN-Response?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=CRN&return_year=" + returnYear + "&return_month=" + returnMonth + "";
            }
            var options = new RestClientOptions("https://api.taxilla.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(body, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + bearerToken + "");
            request.AddHeader("app-id", "envoice");
            DateTime RequestTimeStamp = DateTime.Now;
            RestResponse response = client.Execute(request);
            DateTime ResponseTimeStamp = DateTime.Now;
            var content = response.Content;
            //content = "{'success':'true','message':'IRN Generated SuccessFully','result':{'AckNo':'132310046254531','AckDt':'2023-10-30 11:50:29','Irn':'d198da4acd645c482890843cfd7a033306fccdc3d8829429d4566a0fc403ddd1','SignedInvoice':'eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiQWNrTm9cIjoxMzIzMTAwNDYyNTQ1MzEsXCJBY2tEdFwiOlwiMjAyMy0xMC0zMCAxMTo1MDoyOVwiLFwiSXJuXCI6XCJkMTk4ZGE0YWNkNjQ1YzQ4Mjg5MDg0M2NmZDdhMDMzMzA2ZmNjZGMzZDg4Mjk0MjlkNDU2NmEwZmM0MDNkZGQxXCIsXCJWZXJzaW9uXCI6XCIxLjFcIixcIlRyYW5EdGxzXCI6e1wiVGF4U2NoXCI6XCJHU1RcIixcIlN1cFR5cFwiOlwiRVhQV1BcIixcIlJlZ1JldlwiOlwiTlwiLFwiSWdzdE9uSW50cmFcIjpcIk5cIn0sXCJEb2NEdGxzXCI6e1wiVHlwXCI6XCJJTlZcIixcIk5vXCI6XCJGTDEwMjNDSTAwMDAwMDhcIixcIkR0XCI6XCIzMC8xMC8yMDIzXCJ9LFwiU2VsbGVyRHRsc1wiOntcIkdzdGluXCI6XCIwMkFNQlBHNzc3M00wMDJcIixcIkxnbE5tXCI6XCJFYXNlTXlHU1RcIixcIkFkZHIxXCI6XCJIaW1hY2hhbCBQcmFkZXNoXCIsXCJMb2NcIjpcIkhpbWFjaGFsIFByYWRlc2hcIixcIlBpblwiOjE3NTAzNCxcIlN0Y2RcIjpcIjAyXCIsXCJQaFwiOlwiODk3OTQ5MjIyOVwiLFwiRW1cIjpcImZsZXhvQGdtYWlsLmNvbVwifSxcIkJ1eWVyRHRsc1wiOntcIkdzdGluXCI6XCJVUlBcIixcIkxnbE5tXCI6XCJEYW4gSW5jXCIsXCJQb3NcIjpcIjk2XCIsXCJBZGRyMVwiOlwiQy00MiBRdWVlbiBSb2FkXCIsXCJMb2NcIjpcIkMtNDIgUXVlZW4gUm9hZFwiLFwiUGluXCI6OTk5OTk5LFwiU3RjZFwiOlwiOTZcIn0sXCJJdGVtTGlzdFwiOlt7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiMVwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQVJUSUNMRSAxMTA2IEJMVUUgLyBHUkVFTlwiLFwiSHNuQ2RcIjpcIjY0MDI5OTkwXCIsXCJRdHlcIjoxMCxcIkZyZWVRdHlcIjowLFwiVW5pdFwiOlwiUFJTXCIsXCJVbml0UHJpY2VcIjoxODkyNCxcIlRvdEFtdFwiOjE4OTI0MCxcIkRpc2NvdW50XCI6MCxcIlByZVRheFZhbFwiOjE4OTI0MCxcIkFzc0FtdFwiOjE4OTI0MCxcIkdzdFJ0XCI6MTIsXCJJZ3N0QW10XCI6MjI3MDguOCxcIkNnc3RBbXRcIjowLFwiU2dzdEFtdFwiOjAsXCJDZXNSdFwiOjAsXCJDZXNBbXRcIjowLFwiQ2VzTm9uQWR2bEFtdFwiOjAsXCJTdGF0ZUNlc1J0XCI6MCxcIlN0YXRlQ2VzQW10XCI6MCxcIlN0YXRlQ2VzTm9uQWR2bEFtdFwiOjAsXCJPdGhDaHJnXCI6MCxcIlRvdEl0ZW1WYWxcIjoyMTE5NDguOCxcIkJjaER0bHNcIjp7XCJObVwiOlwiMDAwMDFcIn19LHtcIkl0ZW1Ob1wiOjAsXCJTbE5vXCI6XCIyXCIsXCJJc1NlcnZjXCI6XCJOXCIsXCJQcmREZXNjXCI6XCJBUlRJQ0xFIDExMDUgR1JFRU4gLyBCTFVFXCIsXCJIc25DZFwiOlwiNjQwMjk5OTBcIixcIlF0eVwiOjEwLFwiRnJlZVF0eVwiOjAsXCJVbml0XCI6XCJQUlNcIixcIlVuaXRQcmljZVwiOjE1MTI0LFwiVG90QW10XCI6MTUxMjQwLFwiRGlzY291bnRcIjowLFwiUHJlVGF4VmFsXCI6MTUxMjQwLFwiQXNzQW10XCI6MTUxMjQwLFwiR3N0UnRcIjoxMixcIklnc3RBbXRcIjoxODE0OC44LFwiQ2dzdEFtdFwiOjAsXCJTZ3N0QW10XCI6MCxcIkNlc1J0XCI6MCxcIkNlc0FtdFwiOjAsXCJDZXNOb25BZHZsQW10XCI6MCxcIlN0YXRlQ2VzUnRcIjowLFwiU3RhdGVDZXNBbXRcIjowLFwiU3RhdGVDZXNOb25BZHZsQW10XCI6MCxcIk90aENocmdcIjowLFwiVG90SXRlbVZhbFwiOjE2OTM4OC44LFwiQmNoRHRsc1wiOntcIk5tXCI6XCIyMTA3MjAyMzE0XCJ9fSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiM1wiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQVJUSUNMRSAxMTA3IFlFTExPVyAvIEJMVUVcIixcIkhzbkNkXCI6XCI2NDAyOTk5MFwiLFwiUXR5XCI6MTAsXCJGcmVlUXR5XCI6MCxcIlVuaXRcIjpcIlBSU1wiLFwiVW5pdFByaWNlXCI6MjY1MjQsXCJUb3RBbXRcIjoyNjUyNDAsXCJEaXNjb3VudFwiOjAsXCJQcmVUYXhWYWxcIjoyNjUyNDAsXCJBc3NBbXRcIjoyNjUyNDAsXCJHc3RSdFwiOjEyLFwiSWdzdEFtdFwiOjMxODI4LjgsXCJDZ3N0QW10XCI6MCxcIlNnc3RBbXRcIjowLFwiQ2VzUnRcIjowLFwiQ2VzQW10XCI6MCxcIkNlc05vbkFkdmxBbXRcIjowLFwiU3RhdGVDZXNSdFwiOjAsXCJTdGF0ZUNlc0FtdFwiOjAsXCJTdGF0ZUNlc05vbkFkdmxBbXRcIjowLFwiT3RoQ2hyZ1wiOjAsXCJUb3RJdGVtVmFsXCI6Mjk3MDY4LjgsXCJCY2hEdGxzXCI6e1wiTm1cIjpcIjAwMDAwMlwifX1dLFwiVmFsRHRsc1wiOntcIkFzc1ZhbFwiOjYwNTcyMCxcIkNnc3RWYWxcIjowLFwiU2dzdFZhbFwiOjAsXCJJZ3N0VmFsXCI6NzI2ODYuNCxcIkNlc1ZhbFwiOjAsXCJTdENlc1ZhbFwiOjAsXCJEaXNjb3VudFwiOjAsXCJPdGhDaHJnXCI6MCxcIlJuZE9mZkFtdFwiOjAsXCJUb3RJbnZWYWxcIjo2Nzg0MDYuNCxcIlRvdEludlZhbEZjXCI6MH0sXCJQYXlEdGxzXCI6e1wiQ3JEYXlcIjowLFwiUGFpZEFtdFwiOjAsXCJQYXltdER1ZVwiOjB9LFwiRXhwRHRsc1wiOntcIlNoaXBCTm9cIjpcIkZMLzEwLzIzL0VTSDAwMDAwNDZcIixcIlNoaXBCRHRcIjpcIjMwLzEwLzIwMjNcIixcIkZvckN1clwiOlwiVVNEXCIsXCJDbnRDb2RlXCI6XCJVU1wifSxcIkV3YkR0bHNcIjp7XCJEaXN0YW5jZVwiOjB9fSIsImlzcyI6Ik5JQyBTYW5kYm94In0.URysTJQ8UBL4KMEmGjuMXweKXgb5nIV_gh29mDGpD5dyq6XhDKMFiyZKFxoE0sbjPxcVB_73y0pL39nRPYj0RuYn2tG-vRaWlXD73Is66D1w6-fJ-W3AphKenJLalbRm-RA382FCt_I69Xgo4nQbBSGpp6E_fMbhj1q0Mcq0WekohcCgy7gV_9HcEO6CryDZtrE8ffTMvzTTfK1ygPJFsE1wp39KtJMIRjg-NgzBhgS_2lz7QPGn7SLdK_ENCIg-OQallYQ0fkRmI0esyGpiFytno9gnd21eAeFgaxQfofgG0I_Jpbxc04MTi0X0SdhuDGj43ulFjdJ95bMjUIlYvg','SignedQRCode':'eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjAyQU1CUEc3NzczTTAwMlwiLFwiQnV5ZXJHc3RpblwiOlwiVVJQXCIsXCJEb2NOb1wiOlwiRkwxMDIzQ0kwMDAwMDA4XCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjMwLzEwLzIwMjNcIixcIlRvdEludlZhbFwiOjY3ODQwNi40LFwiSXRlbUNudFwiOjMsXCJNYWluSHNuQ29kZVwiOlwiNjQwMjk5OTBcIixcIklyblwiOlwiZDE5OGRhNGFjZDY0NWM0ODI4OTA4NDNjZmQ3YTAzMzMwNmZjY2RjM2Q4ODI5NDI5ZDQ1NjZhMGZjNDAzZGRkMVwiLFwiSXJuRHRcIjpcIjIwMjMtMTAtMzAgMTE6NTA6MjlcIn0iLCJpc3MiOiJOSUMgU2FuZGJveCJ9.YatyQZmsjsmMZVZvjHT2gwUQHKGGT5gPO7xh661iQaNn3pRhvUSBjqN4sp8XA7wd0dhsvLJ9eyZPCUL23Mt0gqcqfoXSiCO3chm3Gpa14hSyXymVGaC5oSXsKnRgVphthcNQzLTVjbawRKlMSRQ0VYiB2je9B21JW65O49O4Y4KOC_towbmQRGnP5m8hcfOXTcbgjXE_9yflsrpGeaVO3Hv70OlfbNE4RoL3nnH8ivdaGy5cWMD33uy3TwRcNRaovuCvFa8xLTZGgE0Tkf6AIwyccPaomqS1CPGHPvU5MyKpnH9d9rJZRcB0j6gk-r6PIXnaNTu6p8SY6jldmzhh9A','Status':'ACT','Remarks':'[4019 : Provide Transporter ID in order to generate Part A of e-Way Bill]'}}";

            //{"success":"true","message":"IRN Generated SuccessFully","result":{"AckNo":"132310046064196","AckDt":"2023-10-03 15:53:59","Irn":"0fc45b23b9dcf00909433c2f9dd6d9dc7f7dcfab878562fea2ba3b97d5adeaac","SignedInvoice":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiQWNrTm9cIjoxMzIzMTAwNDYwNjQxOTYsXCJBY2tEdFwiOlwiMjAyMy0xMC0wMyAxNTo1Mzo1OVwiLFwiSXJuXCI6XCIwZmM0NWIyM2I5ZGNmMDA5MDk0MzNjMmY5ZGQ2ZDlkYzdmN2RjZmFiODc4NTYyZmVhMmJhM2I5N2Q1YWRlYWFjXCIsXCJWZXJzaW9uXCI6XCIxLjFcIixcIlRyYW5EdGxzXCI6e1wiVGF4U2NoXCI6XCJHU1RcIixcIlN1cFR5cFwiOlwiQjJCXCIsXCJSZWdSZXZcIjpcIk5cIixcIklnc3RPbkludHJhXCI6XCJOXCJ9LFwiRG9jRHRsc1wiOntcIlR5cFwiOlwiSU5WXCIsXCJOb1wiOlwiQjEwODIzRFNJMDAwMDA2NVwiLFwiRHRcIjpcIjEwLzA4LzIwMjNcIn0sXCJTZWxsZXJEdGxzXCI6e1wiR3N0aW5cIjpcIjAyQU1CUEc3NzczTTAwMlwiLFwiTGdsTm1cIjpcIkVhc2VNeUdTVFwiLFwiQWRkcjFcIjpcIkhpbWFjaGFsIFByYWRlc2hcIixcIkxvY1wiOlwiSGltYWNoYWwgUHJhZGVzaFwiLFwiUGluXCI6MTc1MDM0LFwiU3RjZFwiOlwiMDJcIixcIlBoXCI6XCI1ODU4MjgyNTI1XCIsXCJFbVwiOlwiYWxhc2thQGdtYWlsLmNvbVwifSxcIkJ1eWVyRHRsc1wiOntcIkdzdGluXCI6XCIwOUFBQUZBNzIyMlExWjdcIixcIkxnbE5tXCI6XCJBbGFza2EgQnJhbmNoIDFcIixcIlBvc1wiOlwiMDlcIixcIkFkZHIxXCI6XCJELTIwIFN1cmFqcHVyXCIsXCJMb2NcIjpcIkdyZWF0ZXIgTm9pZGEgVXR0YXIgUHJhZGVzaCBJbmRpYVwiLFwiUGluXCI6MjAxMzEwLFwiUGhcIjpcIjU4NTgyODI1MjVcIixcIkVtXCI6XCJhbGFza2FAZ21haWwuY29tXCIsXCJTdGNkXCI6XCIwOVwifSxcIkRpc3BEdGxzXCI6e1wiTm1cIjpcIkFsYXNrYSBCcmFuY2ggMVwiLFwiQWRkcjFcIjpcIkQtMjAgU3VyYWpwdXJcIixcIkxvY1wiOlwiR3JlYXRlciBOb2lkYSBVdHRhciBQcmFkZXNoIEluZGlhXCIsXCJQaW5cIjoyMDEzMTAsXCJTdGNkXCI6XCIwOVwifSxcIlNoaXBEdGxzXCI6e1wiTGdsTm1cIjpcIkFCQyBMdGQuXCIsXCJBZGRyMVwiOlwiaXNod2FyZ2FuZ2lcIixcIkxvY1wiOlwiaXNod2FyZ2FuZ2lcIixcIlBpblwiOjIyMTAwMSxcIlN0Y2RcIjpcIjA5XCJ9LFwiSXRlbUxpc3RcIjpbe1wiSXRlbU5vXCI6MCxcIlNsTm9cIjpcIjFcIixcIklzU2VydmNcIjpcIk5cIixcIlByZERlc2NcIjpcIjEwMjFcIixcIkhzbkNkXCI6XCIzOTI2OTA5OVwiLFwiUXR5XCI6MzAsXCJGcmVlUXR5XCI6MCxcIlVuaXRcIjpcIlBDU1wiLFwiVW5pdFByaWNlXCI6MTAwLFwiVG90QW10XCI6MzAwMCxcIkRpc2NvdW50XCI6MCxcIlByZVRheFZhbFwiOjMwMDAsXCJBc3NBbXRcIjozMDAwLFwiR3N0UnRcIjoxMixcIklnc3RBbXRcIjozNjAsXCJDZ3N0QW10XCI6MCxcIlNnc3RBbXRcIjowLFwiQ2VzUnRcIjowLFwiQ2VzQW10XCI6MCxcIkNlc05vbkFkdmxBbXRcIjowLFwiU3RhdGVDZXNSdFwiOjAsXCJTdGF0ZUNlc0FtdFwiOjAsXCJTdGF0ZUNlc05vbkFkdmxBbXRcIjowLFwiT3RoQ2hyZ1wiOjAsXCJUb3RJdGVtVmFsXCI6MzM2MCxcIkJjaER0bHNcIjp7XCJObVwiOlwiMDAyXCJ9fSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiMlwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiMTE4MlwiLFwiSHNuQ2RcIjpcIjM5MTk5MDEwXCIsXCJRdHlcIjoyMCxcIkZyZWVRdHlcIjowLFwiVW5pdFwiOlwiUENTXCIsXCJVbml0UHJpY2VcIjoxMDAsXCJUb3RBbXRcIjoyMDAwLFwiRGlzY291bnRcIjowLFwiUHJlVGF4VmFsXCI6MjAwMCxcIkFzc0FtdFwiOjIwMDAsXCJHc3RSdFwiOjI4LFwiSWdzdEFtdFwiOjU2MCxcIkNnc3RBbXRcIjowLFwiU2dzdEFtdFwiOjAsXCJDZXNSdFwiOjAsXCJDZXNBbXRcIjowLFwiQ2VzTm9uQWR2bEFtdFwiOjAsXCJTdGF0ZUNlc1J0XCI6MCxcIlN0YXRlQ2VzQW10XCI6MCxcIlN0YXRlQ2VzTm9uQWR2bEFtdFwiOjAsXCJPdGhDaHJnXCI6MCxcIlRvdEl0ZW1WYWxcIjoyNTYwLFwiQmNoRHRsc1wiOntcIk5tXCI6XCI0NTYzMjU0M1wifX1dLFwiVmFsRHRsc1wiOntcIkFzc1ZhbFwiOjUwMDAsXCJDZ3N0VmFsXCI6MCxcIlNnc3RWYWxcIjowLFwiSWdzdFZhbFwiOjkyMCxcIkNlc1ZhbFwiOjAsXCJTdENlc1ZhbFwiOjAsXCJEaXNjb3VudFwiOjAsXCJPdGhDaHJnXCI6MCxcIlJuZE9mZkFtdFwiOjAsXCJUb3RJbnZWYWxcIjo1OTIwLFwiVG90SW52VmFsRmNcIjowfSxcIlBheUR0bHNcIjp7XCJDckRheVwiOjAsXCJQYWlkQW10XCI6MCxcIlBheW10RHVlXCI6MH0sXCJFeHBEdGxzXCI6e30sXCJFd2JEdGxzXCI6e1wiRGlzdGFuY2VcIjowfX0iLCJpc3MiOiJOSUMgU2FuZGJveCJ9.iKgXWvmkeal_yAkWhp0elVqjqPsRgMCYYTb72Upf-qpoxglBmUEyZXvz4KldB7fDAOUSGcOMvqjZewLPlLXD25Rc8MsjD81SQZXbaaOB_qPFIHA5qiU_YNYrrfTRr69AXtxrPnaX6wgfrhUnfnN29WXyvVzwHwRDlt45hG_9Tej78ftO4nW0ydcFwe960vSL0m-ED8CanUDwzsv4wJohLo34TEBbHYhmxDYfUx0diWKdrnim7raBhUW0X9QG0BytCRnEES3sfovj_JMsjIu7x7TANQiazTq2GkgP772TCc3_qkWwOg7KPmKgQ2rpyKfbZtjWo45NhgJBmOow5k-fng","SignedQRCode":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjAyQU1CUEc3NzczTTAwMlwiLFwiQnV5ZXJHc3RpblwiOlwiMDlBQUFGQTcyMjJRMVo3XCIsXCJEb2NOb1wiOlwiQjEwODIzRFNJMDAwMDA2NVwiLFwiRG9jVHlwXCI6XCJJTlZcIixcIkRvY0R0XCI6XCIxMC8wOC8yMDIzXCIsXCJUb3RJbnZWYWxcIjo1OTIwLFwiSXRlbUNudFwiOjIsXCJNYWluSHNuQ29kZVwiOlwiMzkyNjkwOTlcIixcIklyblwiOlwiMGZjNDViMjNiOWRjZjAwOTA5NDMzYzJmOWRkNmQ5ZGM3ZjdkY2ZhYjg3ODU2MmZlYTJiYTNiOTdkNWFkZWFhY1wiLFwiSXJuRHRcIjpcIjIwMjMtMTAtMDMgMTU6NTM6NTlcIn0iLCJpc3MiOiJOSUMgU2FuZGJveCJ9.FVHatFgfRFY6e82hOXS-zT4NAPgWQUblYL4gFBR16tlTpyHqUaqb_wrxIbnHgLXcj0iU5Va_sF8Xacz9Cf5in2g-w6RLUfOfKiUgXYMlA2rlGRruUiP2DLd_pEIMoEvExk5qhJ7EOiJgrE0y3EyAezKS0MZIK5Afw7iFY_ZMjFmsp8ohA_z6qFAe5YhnedujzE3mebYRqwC9AH7lYaK-z9ZYH-oVNynGDTqxW2y1bPjrte51W3Zm4ycpxzFgEA5F4uWWs12yXMaaZpnKq8AGFpo_AQsoxfnF-s6-FzmcvzQeH6XYwAlvNNaa51tMxSPdXQ9SyPzO2I58JJQAI1w_Rw","Status":"ACT","Remarks":"[4019 : Provide Transporter ID in order to generate Part A of e-Way Bill]"}}
            //content = "{'success':'true','message':'IRN Generated SuccessFully','result':{'AckNo':'132310046241908','AckDt':'2023-10-27 13:29:01','Irn':'d3bf7597fcd2aa559191b670572b415191d0f656f396cabc476b8e0437c7b658','SignedInvoice':'eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiQWNrTm9cIjoxMzIzMTAwNDYyNDE5MDgsXCJBY2tEdFwiOlwiMjAyMy0xMC0yNyAxMzoyOTowMVwiLFwiSXJuXCI6XCJkM2JmNzU5N2ZjZDJhYTU1OTE5MWI2NzA1NzJiNDE1MTkxZDBmNjU2ZjM5NmNhYmM0NzZiOGUwNDM3YzdiNjU4XCIsXCJWZXJzaW9uXCI6XCIxLjFcIixcIlRyYW5EdGxzXCI6e1wiVGF4U2NoXCI6XCJHU1RcIixcIlN1cFR5cFwiOlwiRVhQV1BcIixcIlJlZ1JldlwiOlwiTlwiLFwiSWdzdE9uSW50cmFcIjpcIk5cIn0sXCJEb2NEdGxzXCI6e1wiVHlwXCI6XCJJTlZcIixcIk5vXCI6XCJGTDEwMjNDSTAwMDAwMTFcIixcIkR0XCI6XCIyNi8xMC8yMDIzXCJ9LFwiU2VsbGVyRHRsc1wiOntcIkdzdGluXCI6XCIwMkFNQlBHNzc3M00wMDJcIixcIkxnbE5tXCI6XCJFYXNlTXlHU1RcIixcIkFkZHIxXCI6XCJIaW1hY2hhbCBQcmFkZXNoXCIsXCJMb2NcIjpcIkhpbWFjaGFsIFByYWRlc2hcIixcIlBpblwiOjE3NTAzNCxcIlN0Y2RcIjpcIjAyXCIsXCJQaFwiOlwiODk3OTQ5MjIyOVwiLFwiRW1cIjpcImZsZXhvQGdtYWlsLmNvbVwifSxcIkJ1eWVyRHRsc1wiOntcIkdzdGluXCI6XCJVUlBcIixcIkxnbE5tXCI6XCJEYW4gSW5jXCIsXCJQb3NcIjpcIjk2XCIsXCJBZGRyMVwiOlwiQy00MiBRdWVlbiBSb2FkXCIsXCJMb2NcIjpcIkMtNDIgUXVlZW4gUm9hZFwiLFwiUGluXCI6OTk5OTk5LFwiU3RjZFwiOlwiOTZcIn0sXCJJdGVtTGlzdFwiOlt7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiMVwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQVJUSUNMRSAxMTA2IEJMVUUgLyBHUkVFTlwiLFwiSHNuQ2RcIjpcIjY0MDI5OTkwXCIsXCJRdHlcIjozLFwiRnJlZVF0eVwiOjAsXCJVbml0XCI6XCJQUlNcIixcIlVuaXRQcmljZVwiOjc4MzIxLjYsXCJUb3RBbXRcIjoyMzQ5NjQuOCxcIkRpc2NvdW50XCI6MCxcIlByZVRheFZhbFwiOjIzNDk2NC44LFwiQXNzQW10XCI6MjM0OTY0LjgsXCJHc3RSdFwiOjEyLFwiSWdzdEFtdFwiOjI4MTk1Ljc4LFwiQ2dzdEFtdFwiOjAsXCJTZ3N0QW10XCI6MCxcIkNlc1J0XCI6MCxcIkNlc0FtdFwiOjAsXCJDZXNOb25BZHZsQW10XCI6MCxcIlN0YXRlQ2VzUnRcIjowLFwiU3RhdGVDZXNBbXRcIjowLFwiU3RhdGVDZXNOb25BZHZsQW10XCI6MCxcIk90aENocmdcIjowLFwiVG90SXRlbVZhbFwiOjI2MzE2MC41OCxcIkJjaER0bHNcIjp7XCJObVwiOlwiMDAwMDFcIn19LHtcIkl0ZW1Ob1wiOjAsXCJTbE5vXCI6XCIyXCIsXCJJc1NlcnZjXCI6XCJOXCIsXCJQcmREZXNjXCI6XCJBUlRJQ0xFIDExMDUgR1JFRU4gLyBCTFVFXCIsXCJIc25DZFwiOlwiNjQwMjk5OTBcIixcIlF0eVwiOjUsXCJGcmVlUXR5XCI6MCxcIlVuaXRcIjpcIlBSU1wiLFwiVW5pdFByaWNlXCI6OTAwODEuNixcIlRvdEFtdFwiOjQ1MDQwOCxcIkRpc2NvdW50XCI6MCxcIlByZVRheFZhbFwiOjQ1MDQwOCxcIkFzc0FtdFwiOjQ1MDQwOCxcIkdzdFJ0XCI6MTIsXCJJZ3N0QW10XCI6NTQwNDguOTYsXCJDZ3N0QW10XCI6MCxcIlNnc3RBbXRcIjowLFwiQ2VzUnRcIjowLFwiQ2VzQW10XCI6MCxcIkNlc05vbkFkdmxBbXRcIjowLFwiU3RhdGVDZXNSdFwiOjAsXCJTdGF0ZUNlc0FtdFwiOjAsXCJTdGF0ZUNlc05vbkFkdmxBbXRcIjowLFwiT3RoQ2hyZ1wiOjAsXCJUb3RJdGVtVmFsXCI6NTA0NDU2Ljk2LFwiQmNoRHRsc1wiOntcIk5tXCI6XCIyMTA3MjAyMzE0XCJ9fSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiM1wiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQVJUSUNMRSAxMTA3IFlFTExPVyAvIEJMVUVcIixcIkhzbkNkXCI6XCI2NDAyOTk5MFwiLFwiUXR5XCI6NSxcIkZyZWVRdHlcIjowLFwiVW5pdFwiOlwiUFJTXCIsXCJVbml0UHJpY2VcIjo1ODcyMS42LFwiVG90QW10XCI6MjkzNjA4LFwiRGlzY291bnRcIjowLFwiUHJlVGF4VmFsXCI6MjkzNjA4LFwiQXNzQW10XCI6MjkzNjA4LFwiR3N0UnRcIjoxMixcIklnc3RBbXRcIjozNTIzMi45NixcIkNnc3RBbXRcIjowLFwiU2dzdEFtdFwiOjAsXCJDZXNSdFwiOjAsXCJDZXNBbXRcIjowLFwiQ2VzTm9uQWR2bEFtdFwiOjAsXCJTdGF0ZUNlc1J0XCI6MCxcIlN0YXRlQ2VzQW10XCI6MCxcIlN0YXRlQ2VzTm9uQWR2bEFtdFwiOjAsXCJPdGhDaHJnXCI6MCxcIlRvdEl0ZW1WYWxcIjozMjg4NDAuOTYsXCJCY2hEdGxzXCI6e1wiTm1cIjpcIjAwMDAwMlwifX1dLFwiVmFsRHRsc1wiOntcIkFzc1ZhbFwiOjk3ODk4MC44LFwiQ2dzdFZhbFwiOjAsXCJTZ3N0VmFsXCI6MCxcIklnc3RWYWxcIjoxMTc0NzcuNyxcIkNlc1ZhbFwiOjAsXCJTdENlc1ZhbFwiOjAsXCJEaXNjb3VudFwiOjAsXCJPdGhDaHJnXCI6MCxcIlJuZE9mZkFtdFwiOjAsXCJUb3RJbnZWYWxcIjoxMDk2NDU4LjUsXCJUb3RJbnZWYWxGY1wiOjB9LFwiUGF5RHRsc1wiOntcIkNyRGF5XCI6MCxcIlBhaWRBbXRcIjowLFwiUGF5bXREdWVcIjowfSxcIkV4cER0bHNcIjp7XCJTaGlwQk5vXCI6XCJGTC8xMC8yMy9FU0gwMDAwMDI1XCIsXCJTaGlwQkR0XCI6XCIyNi8xMC8yMDIzXCIsXCJGb3JDdXJcIjpcIlVTRFwiLFwiQ250Q29kZVwiOlwiVVNcIn0sXCJFd2JEdGxzXCI6e1wiRGlzdGFuY2VcIjowfX0iLCJpc3MiOiJOSUMgU2FuZGJveCJ9.ZXXTY8IGms3CqnIzU8OQ8ErYrWEGjv8yclm5aN5FHUglh0t_kE7XRyLbGn2Ja-w7JBLNSgVQ_qRi4Jiemc_K4JjDS3WOiomS8tRTGMlmNKneAhGXTyHYi4kAxft8y-m-3cs-ixCMrswDIPFvc4oZt1ST1fT5MA_bhoyrihJZn_nfQ-xqlBUzUsjZAm4f1yi1BiI7bwizMCAUgdG4XVw2436rLmUs6fR-vow_uQsyJ1ajUEZvf0HnOxf6l4NEFiHi2-ph_NzQxdE8QSJFh7NI3t3mHC7Rc30EC510m1bumTPZ290LWqHj5Cq0QMa44He3WtJOj3vr5ptbt1ImYol5ug','SignedQRCode':'eyJhbGciOiJSUzI1NiIsImtpZCI6IjE1MTNCODIxRUU0NkM3NDlBNjNCODZFMzE4QkY3MTEwOTkyODdEMUYiLCJ4NXQiOiJGUk80SWU1R3gwbW1PNGJqR0w5eEVKa29mUjgiLCJ0eXAiOiJKV1QifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjAyQU1CUEc3NzczTTAwMlwiLFwiQnV5ZXJHc3RpblwiOlwiVVJQXCIsXCJEb2NOb1wiOlwiRkwxMDIzQ0kwMDAwMDExXCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjI2LzEwLzIwMjNcIixcIlRvdEludlZhbFwiOjEwOTY0NTguNSxcIkl0ZW1DbnRcIjozLFwiTWFpbkhzbkNvZGVcIjpcIjY0MDI5OTkwXCIsXCJJcm5cIjpcImQzYmY3NTk3ZmNkMmFhNTU5MTkxYjY3MDU3MmI0MTUxOTFkMGY2NTZmMzk2Y2FiYzQ3NmI4ZTA0MzdjN2I2NThcIixcIklybkR0XCI6XCIyMDIzLTEwLTI3IDEzOjI5OjAxXCJ9IiwiaXNzIjoiTklDIFNhbmRib3gifQ.EfQMmt3R6JvP4stuW6tJ1UtMlyqoXXTSyPwbBw0pATZhXzo2Tmn-S_fkI6o2zBgjjqNyVxAqTA4eU-DNobF04DcxHXTMKCljNC47_H8fzO1aC1OpayYMmNcbUtnKqvLHGVK7Z3ta3J8snJ1GwmNGYd5maXHMyhRhJrRIUAXqAED-duUfiWCgyIYZPMmJPkDs_zKfQK8MOCm12Dqxz_C25dOaBIyfTZrSVAqD71956NCAGdBexQUWg6CjPnIHSkuUoUFR8MGVqZJVQHCTrIz8AE0hDj4coOGqUw2cQ7NNYuicpoMs_HEQSRohzqLo-2ZCK4QOKMu6ijnu6S8R7XlRdQ','Status':'ACT','Remarks':'[4019 : Provide Transporter ID in order to generate Part A of e-Way Bill]'}}";

            //Generate Api logs in DB gst$api$request$log
            //cancel invoice response 
            //{'success':'true','message':'E-Invoice is cancelled successfully','result':{'Irn':'a608ef326eb07c05a9111b31fb35d4da1d4f51a1d7a9020e45b2568290b957ca','CancelDate':'2023-10-31 16:12:00'}}
            _gstApi.SaveApiRequestLogs(body, content, RequestTimeStamp, ResponseTimeStamp, "Posted", compId, brId, invNo, invDt, "Status Check Request Raised");
            JObject obj = JObject.Parse(content);
            if (obj.ContainsKey("message") || obj.ContainsKey("msg"))
            {
                string msg = "";
                if (obj.ContainsKey("message"))
                    msg = obj["message"].ToString();
                else
                    msg = obj["msg"].ToString();
                if (msg == "IRN Generated SuccessFully" || msg.Contains("already generated") || msg.Contains("Duplicate IRN") || msg.Contains("EwayBill is already generated"))
                {
                    //Getting Values from Json Node
                    string ackNo = obj["result"]["AckNo"].ToString();
                    string ackDt = obj["result"]["AckDt"].ToString();
                    string irnNo = obj["result"]["Irn"].ToString();
                    string signedQrCode = obj["result"]["SignedQRCode"].ToString();
                    string status = "S"; // obj["result"]["Status"].ToString();
                                         //Save in EwbNo, EwbAckNo, EwbAckDt if status check for (EWB(E-Way Bill))
                    if (requestFor == "EWB")
                    {
                        if (content.Contains("EwbNo"))
                        {
                            //Replace Values if checking status for eway bill 
                            irnNo = obj["result"]["EwbNo"].ToString();
                            ackDt = obj["result"]["EwbDt"].ToString();
                            //ackNo = obj["result"]["EwbValidTill"].ToString();
                            if (((JObject)obj["result"]).ContainsKey("EwbValidTill"))
                            {
                                ackNo = obj["result"]["EwbValidTill"].ToString();
                            }
                            else
                            {
                                ackNo = "";
                            }
                        }
                        else
                        {
                            return msg;
                        }
                    }
                    // Insert Docs to Invoice Attachment
                    string ewbSummaryPdfPath = "", ewbSummaryPdfName = "", ewbDetailsPdfPath = "", ewbDetailsPdfName = "",
                        EinvIrnPdfPath = "", EinvIrnPdfName = "", checkAttachment="";
                    if (requestFor.ToUpper() == "EWB")
                    {
                        //ewb summary
                        checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath.Replace("Attachment", "").ToString(), "A_EWBS");
                        if (checkAttachment != "Duplicate")
                        {
                            string[] ewbSummary = Task.Run(async () => await GetPdfEwbSummaryIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                            ewbSummaryPdfName = ewbSummary[0].ToString();
                            ewbSummaryPdfPath = ewbSummary[1].ToString();
                            if (ewbSummaryPdfName != "N/A")
                            {
                                _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath.Replace("Attachment", "").ToString(), "A_EWBS");

                            }
                        }

                        //ewb details
                        checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath.Replace("Attachment", "").ToString(), "A_EWBD");
                        if (checkAttachment != "Duplicate")
                        {
                            string[] ewbDetails = Task.Run(async () => await GetPdfEwbDetailsIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                            ewbDetailsPdfName = ewbDetails[0].ToString();
                            ewbDetailsPdfPath = ewbDetails[1].ToString();
                            if (ewbDetailsPdfName != "N/A")
                            {
                                _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbDetailsPdfName, ewbDetailsPdfPath.Replace("Attachment", "").ToString(), "A_EWBD");
                            }
                        }
                            
                    }
                    else
                    {
                        if (requestFor.ToUpper() != "SC" || requestFor.ToUpper() != "SSC" || requestFor.ToUpper() != "SCC")
                        {
                            //einv irn 
                            checkAttachment = _gstApi.CheckGstInvAttachmentDetails(((requestFor == "SSI" || requestFor == "SSC") ? "SINV" : invType), compId, brId, invNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV");
                            if (checkAttachment != "Duplicate")
                            {
                                string[] einvPdf = Task.Run(async () => await GetPdfEinvIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                                EinvIrnPdfName = einvPdf[0].ToString();
                                EinvIrnPdfPath = einvPdf[1].ToString();
                                if (EinvIrnPdfName != "N/A")
                                {
                                    _gstApi.UpdateGstInvAttachmentDetails(((requestFor == "SSI" || requestFor == "SSC") ? "SINV" : invType), compId, brId, invNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV");
                                }
                            }
                        }
                    }

                    //Update EINV/EWB details on invoice table table
                    int result = _gstApi.UpdateGstInvApiDetails(requestFor, compId, brId, invNo, invDt, ackNo, ackDt, irnNo, status, invType, signedQrCode);
                    return obj["message"].ToString();
                }
                else if (msg.Contains("E-Invoice is cancelled successfully"))
                {
                    string irnNo = obj["result"]["Irn"].ToString();
                    string CancelDate = obj["result"]["CancelDate"].ToString();
                    string status = "S";
                    // Insert Docs to Invoice Attachment
                    string ewbSummaryPdfPath = "", ewbSummaryPdfName = "", ewbDetailsPdfPath = "", ewbDetailsPdfName = "",
                        EinvIrnPdfPath = "", EinvIrnPdfName = "", checkAttachment="";
                    if (requestFor.ToUpper() == "EWB")
                    {
                        //ewb summary
                        checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath, "A_EWBS_C");
                        if (checkAttachment != "Duplicate")
                        {
                            string[] ewbSummary = Task.Run(async () => await GetPdfEwbSummaryIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                            ewbSummaryPdfName = ewbSummary[0].ToString();
                            ewbSummaryPdfPath = ewbSummary[1].ToString();
                            _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath, "A_EWBS_C");
                        }
                        //ewb details
                        checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbDetailsPdfName, ewbDetailsPdfPath.Replace("Attachment", "").ToString(), "A_EWBD_C");
                        if (checkAttachment != "Duplicate")
                        {
                            string[] ewbDetails = Task.Run(async () => await GetPdfEwbDetailsIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                            ewbDetailsPdfName = ewbDetails[0].ToString();
                            ewbDetailsPdfPath = ewbDetails[1].ToString();
                            _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, invNo, invDt, ewbDetailsPdfName, ewbDetailsPdfPath.Replace("Attachment", "").ToString(), "A_EWBD_C");
                        }
                    }
                    else
                    {
                        if (requestFor.ToUpper() != "SC" || requestFor.ToUpper() != "SC")
                        {
                            //einv irn 
                            checkAttachment = _gstApi.CheckGstInvAttachmentDetails((requestFor == "SSI" ? "SINV" : invType), compId, brId, invNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV_C");
                            if (checkAttachment != "Duplicate")
                            {
                                string[] einvPdf = Task.Run(async () => await GetPdfEinvIRN(invType, invNo, returnYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                                EinvIrnPdfName = einvPdf[0].ToString();
                                EinvIrnPdfPath = einvPdf[1].ToString();
                                _gstApi.UpdateGstInvAttachmentDetails((requestFor == "SSI" ? "SINV" : invType), compId, brId, invNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV_C");
                            }
                        }
                    }

                    int result = _gstApi.UpdateGstInvApiDetails(requestFor, compId, brId, invNo, invDt, "N/A", CancelDate, irnNo, status, invType, "");
                    return msg;
                }
                else if (msg.ToLower().Contains("inprogress"))
                {
                    // implement recursion (status recheck again) if status check is in progress
                    return EInvoiceEWBStatusCheck(requestFor, invNo, invDt, returnYear, returnMonth, invType);
                }
                else
                {
                    _gstApi.UpdateGstInvApiDetails(requestFor, compId, brId, invNo, invDt, "N/A", null, "N/A", "F", invType, "");
                    _gstApi.SaveApiRequestLogs(body, content, RequestTimeStamp, ResponseTimeStamp, "ERROR", compId, brId, invNo, invDt, msg);
                    return msg;
                }
            }
            else
            {
                _gstApi.UpdateGstInvApiDetails(requestFor, compId, brId, invNo, invDt, "N/A", null, "N/A", "F", invType, "");
                _gstApi.SaveApiRequestLogs(body, content, RequestTimeStamp, ResponseTimeStamp, "ERROR", compId, brId, invNo, invDt, "error on status check");
                return "error on status check";
            }
        }
        //Generate bearer token for every request
        public string GenerateBearerToken()
        {
            // Getting client id and client secret key stored in web.config XML file
            //string clientId = ConfigurationManager.AppSettings["GstApiClientId"].ToString();
            //string clientSecretKey = ConfigurationManager.AppSettings["GstApiClientSecret"].ToString();

            string clientId = string.Empty;
            string clientSecretKey = string.Empty;
            // change by prem on 28-11-2023 get client details in database
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataTable clientdtl = _gstApi.GetApiClientDetails(compId, brId);
            if (clientdtl.Rows.Count > 0)
            {
                clientId = clientdtl.Rows[0]["client_id"].ToString();
                clientSecretKey = clientdtl.Rows[0]["client_secret_id"].ToString();
            }
            var client = new RestClient("https://api.taxilla.com");
            //Set HTTP request type
            var request = new RestRequest("/oauth2/v1/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //Default UAT Credientials
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecretKey);
            //Our Credientials for UAT
            //request.AddParameter("client_id", "ENREP001");
            //request.AddParameter("client_secret", "ENREP001");
            request.AddParameter("grant_type", "client_credentials");
            DateTime requestTimeStamp = DateTime.Now;
            RestResponse response = client.Execute(request);
            var content = response.Content;
            DateTime responseTimeStamp = DateTime.Now;
            //Generate Api logs in DB gst$api$request$log
            _gstApi.SaveApiRequestLogs("Bearer Token Request Initiated", content, requestTimeStamp, responseTimeStamp, "Posted", "0", "0", "", "", "No Error on generate bearer token");
            JObject obj = JObject.Parse(content);
            if (obj.ContainsKey("access_token"))
            {
                //Return Bearer token if token generated successfuly
                string token = obj["access_token"].ToString();
                return token;
            }
            else
            {
                //Generate Api Logs
                return "error";
            }
        }
        //Post EWAY BILL
        public string PostEnvoiceData(string invType, string dataType, string invNo, string invDt)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            string token = GenerateBearerToken();
            if (token == "error")
                return "failed";
            string body = "";
            if (dataType == "SSI")
            {
                // Getting service sale invoice data
                body = ServiceInvoiceGstApiBody(invNo, invDt);
            }
            else
            {
                // Getting sale invoice data
                // or Getting Scrap Sale Invoice data
                body = InvoiceGstApiBody(invType, invNo, invDt);
            }
            //Return request as iinvalid data if request body not generated
            if (body == null)
                return "Invalid data";
            //passing api base URL inside RestClient constr
            var client = new RestClient("https://api.taxilla.com");
            var request = new RestRequest("/process/v1/einvoiceewbv1?transformationName=IRP_JSON_Upload&autoExecuteRules=true", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            //Setting authorization bearer token
            request.AddHeader("Authorization", "Bearer " + token + "");
            request.AddHeader("app-id", "envoice");
            //Format request body as pretty json
            request.AddStringBody(body, DataFormat.Json);
            DateTime requestTimeStamp = DateTime.Now;
            RestResponse response = client.Execute(request);
            //Hold response content in a variable to get Key Value pairs
            var content = response.Content;
            DateTime responseTimeStamp = DateTime.Now;
            // Parse data using JObject (provided by newtonsoft.json)
            JObject obj = JObject.Parse(content);
            //Generate Api logs in DB gst$api$request$log
            _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "Posted", compId, brId, invNo, invDt, "E-INV Request Raised");

            if (obj.ContainsKey("msg"))
            {
                if (obj["msg"].ToString() == "Request created successfully")
                {
                    
                    DataSet invDetails;
                    if (dataType == "SSI")
                        invDetails = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDt);
                    else
                        invDetails = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                    // post status check api
                    string gstInvNo = invDetails.Tables[0].Rows[0]["No"].ToString();
                    string finYear = invDetails.Tables[0].Rows[0]["FinYear"].ToString();
                    string returnMonth = invDetails.Tables[0].Rows[0]["ReturnMonth"].ToString();
                    if (returnMonth.Length == 1)
                        returnMonth = "0" + returnMonth;
                    System.Threading.Thread.Sleep(15000);/* wait for 15 sec before get request */
                    //Request created successfully. post status check api request
                    return EInvoiceEWBStatusCheck(dataType, gstInvNo, invDt, finYear, returnMonth, invType);
                }
                else
                {
                    //Generate api logs and retrive error msg thrown by api response
                    _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "ERROR", compId, brId, invNo, invDt, obj["msg"].ToString());
                    return obj["msg"].ToString();
                }
            }
            else
            {
                //Generate logs and return as error
                _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "ERROR", compId, brId, invNo, invDt, "Error on einvoice api");
                return "error on envoice api";
            }
        }
        //Post EWAY Bill
        public string PostEWayBill(string invType, string invNo, string invDt)
        {
            //Generate EWAY bill after IRN generated successfully
            string bearerToken = GenerateBearerToken();
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();

            var client = new RestClient("https://api.taxilla.com");

            var request = new RestRequest("/process/v1/einvoiceewbv1?transformationName=Generate_eWaybill_Json_Upload&autoExecuteRules=true", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + bearerToken + "");
            request.AddHeader("app-id", "envoice");
            var body = EWBGstApiBody(invType, invNo, invDt);
            request.AddStringBody(body, DataFormat.Json);
            DateTime requestTimeStamp = DateTime.Now;
            RestResponse response = client.Execute(request);
            var content = response.Content;
            DateTime responseTimeStamp = DateTime.Now;
            //Generate Api logs in DB gst$api$request$log
            _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "Posted", compId, brId, invNo, invDt, "EWB Request Raised");
            JObject obj = JObject.Parse(content);
            if (obj.ContainsKey("msg"))
            {
                string apiMsg = obj["msg"].ToString();
                if (apiMsg == "Request created successfully")
                {
                    DataSet invDetails = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                    // post status check api
                    string gstInvNo = invDetails.Tables[0].Rows[0]["No"].ToString();
                    string finYear = invDetails.Tables[0].Rows[0]["FinYear"].ToString();
                    string returnMonth = invDetails.Tables[0].Rows[0]["ReturnMonth"].ToString();
                    System.Threading.Thread.Sleep(15000);//delay for 15 sec to check status;
                    return EInvoiceEWBStatusCheck("EWB", gstInvNo, invDt, finYear, returnMonth, invType);
                }
                else
                {
                    _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "ERROR", compId, brId, invNo, invDt, apiMsg);
                    return apiMsg;
                }
            }
            else
            {
                _gstApi.SaveApiRequestLogs(body, content, requestTimeStamp, responseTimeStamp, "ERROR", compId, brId, invNo, invDt, "error on ewb api");
                return "error on ewb api";
            }
        }
        //Cancel IRN 
        public string CancelIrnEinvOrEwb(string invType, string cancelRequestbody, string dataType, string invNo, string invDt)
        {
            // Cancel request for cancelled invoices if IRN generated by GST api
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            var options = new RestClientOptions("https://api.taxilla.com")
            {
                MaxTimeout = -1,
            };
            string bearerToken = GenerateBearerToken();
            var client = new RestClient(options);

            var request = new RestRequest("/process/v1/einvoiceewbv1?transformationName=IRP_JSON_Upload-CancelEWB&autoExecuteRules=true", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            request.AddHeader("app-id", "envoice");
            request.AddStringBody(cancelRequestbody, DataFormat.Json);
            DateTime requestTimeStamp = DateTime.Now;
            RestResponse response = client.Execute(request);
            var content = response.Content;
            // content = "{'msg':'Request created successfully','response':{'requestId':'172748a0-6998-11ee-8341-0fa67dffc4f7'}}";
            DateTime responsetimeStamp = DateTime.Now;
            //Generate Api logs in DB gst$api$request$log
            _gstApi.SaveApiRequestLogs(cancelRequestbody, content, requestTimeStamp, responsetimeStamp, "Posted", compId, brId, invNo, invDt, "No Error");
            JObject obj = JObject.Parse(content);
            if (obj.ContainsKey("message") || obj.ContainsKey("msg"))
            {
                string msg = "";
                if (obj.ContainsKey("message"))
                    msg = obj["message"].ToString();
                else
                    msg = obj["msg"].ToString();
                if (obj["msg"].ToString() == "Request created successfully")
                {
                    //Check Einv Status on status check api
                    DataSet invDetails = new DataSet();// _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                    if (dataType == "SSC")
                    {
                        invDetails = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDt);
                    }
                    else
                    {
                        invDetails = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
                    }
                    
                    // post status check api
                    string gstInvNo = invDetails.Tables[0].Rows[0]["No"].ToString();
                    string finYear = invDetails.Tables[0].Rows[0]["FinYear"].ToString();
                    string returnMonth = invDetails.Tables[0].Rows[0]["ReturnMonth"].ToString();
                    if (returnMonth.Length == 1)
                        returnMonth = "0" + returnMonth;
                    System.Threading.Thread.Sleep(15000);
                    return EInvoiceEWBStatusCheck(dataType, gstInvNo, invDt, finYear, returnMonth, invType);
                }
                else
                {
                    _gstApi.SaveApiRequestLogs(cancelRequestbody, content, requestTimeStamp, responsetimeStamp, "ERROR", compId, brId, invNo, invDt, msg);
                    return msg;
                }
            }
            else
            {
                _gstApi.SaveApiRequestLogs(cancelRequestbody, content, requestTimeStamp, responsetimeStamp, "ERROR", compId, brId, invNo, invDt, "Error on cancel invoice api");
                return "error on cancel invoice api";
            }
        }
        //Generate PDF file Name
        public string GenerateFileName(string filetype, string invNo)
        {
            // Generate file name to save IRN/EWB PDF
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            string fileName = filetype + "_" + invNo.Replace("/","-") + DateTime.Now.ToString("yyyyMMddHHmmss");
            return fileName;
        }
        //IRN PDF
        public async Task<string> GetPdfEinvIRN(string invType, string invNo, string returnYear, string returnMonth)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                string token = GenerateBearerToken();
                string body = "https://api.taxilla.com/process/v1/einvoiceewbv1/reports/eInvoice?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=INV&return_year=" + returnYear + "&return_month=" + returnMonth;
                if(invType=="SRT")
                    body = "https://api.taxilla.com/process/v1/einvoiceewbv1/reports/eInvoice?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=CRN&return_year=" + returnYear + "&return_month=" + returnMonth;
                var request = new HttpRequestMessage(HttpMethod.Get, body);
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Headers.Add("app-id", "envoice");
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
                DateTime requestTimeStamp = DateTime.Now;
                var response = await client.SendAsync(request);
                DateTime responseTimeStamp = DateTime.Now;
                var content1 = response;
                _gstApi.SaveApiRequestLogs(body, content1.ToString(), requestTimeStamp, responseTimeStamp, "Posted", compId, brId, invNo, DateTime.Now.ToString("yyyy-MM-dd"), "EnvoiceIRN PDF");
                response.EnsureSuccessStatusCode();
                //await response.Content.ReadAsStreamAsync();

                var cntnt = await response.Content.ReadAsStreamAsync();
                string folderName = "";
                if (invType.ToUpper() == "INV")
                    folderName = "SaleInvoice/";
                else if (invType.ToUpper() == "SCINV")
                    folderName = "ScrapSaleInvoice/";
                else if (invType.ToUpper() == "IBS")
                    folderName = "InterBranchSale/";
                else if (invType.ToUpper() == "SRT")
                    folderName = "SalesReturn/";
                else
                    folderName = "CustomInvoice/";
                string path = Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);
                string folderPath = path + ("..\\Attachment\\");

                bool exists = System.IO.Directory.Exists(folderPath + folderName);
                if (!exists)
                    System.IO.Directory.CreateDirectory(folderPath + folderName);
                string fileName = GenerateFileName("EInvoice", invNo);
                string filePath = path + ("..\\Attachment\\") + folderName + fileName + ".pdf";
                //FileStream stream = new FileStream(filePath, FileMode.CreateNew);
                //// Create a StreamWriter from FileStream
                //using (StreamWriter writer = new StreamWriter(stream))
                //{
                //    writer.Write(await response.Content.ReadAsStreamAsync());
                //}
                MemoryStream strm = cntnt as MemoryStream;
                byte[] arr = strm.ToArray();
                System.IO.File.WriteAllBytes(filePath, arr);
                //Response.Write(await response.Content.ReadAsStringAsync());
                return fileName + "&" + filePath;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "N/A&N/A";
            }
        }
        //Eway Bill Summary
        public async Task<string> GetPdfEwbSummaryIRN(string invType, string invNo, string returnYear, string returnMonth)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                string token = GenerateBearerToken();
                string body = "https://api.taxilla.com/process/v1/einvoiceewbv1/reports/e-WayBill-Summary?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=INV&return_year=" + returnYear + "&return_month=" + returnMonth;
                var request = new HttpRequestMessage(HttpMethod.Get, body);
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Headers.Add("app-id", "envoice");
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                request.Content = content;
                DateTime requestTimeStamp = DateTime.Now;
                var response = await client.SendAsync(request);
                DateTime responseTimeStamp = DateTime.Now;
                var content1 = response;
                _gstApi.SaveApiRequestLogs(body, content1.ToString(), requestTimeStamp, responseTimeStamp, "Posted", compId, brId, invNo, DateTime.Now.ToString("yyyy-MM-dd"), "EWB Summary PDF");
                response.EnsureSuccessStatusCode();
                var cntnt = await response.Content.ReadAsStreamAsync();
                string folderName = "";
                if (invType.ToUpper() == "INV")
                    folderName = "SaleInvoice/";
                else if (invType.ToUpper() == "SCINV")
                    folderName = "ScrapSaleInvoice/";
                else
                    folderName = "CustomInvoice/";

                string path = Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);
                string folderPath = path + ("..\\Attachment\\");

                bool exists = System.IO.Directory.Exists(folderPath + folderName);
                if (!exists)
                    System.IO.Directory.CreateDirectory(folderPath + folderName);
                string fileName = GenerateFileName("EwbSummary", invNo);
                string filePath = path + ("..\\Attachment\\") + folderName + fileName + ".pdf";
                //FileStream stream = new FileStream(filePath, FileMode.CreateNew);
                //// Create a StreamWriter from FileStream
                //using (StreamWriter writer = new StreamWriter(stream))
                //{
                //    writer.Write(await response.Content.ReadAsStreamAsync());
                //}
                MemoryStream strm = cntnt as MemoryStream;
                byte[] arr = strm.ToArray();
                System.IO.File.WriteAllBytes(filePath, arr);
                //Response.Write(await response.Content.ReadAsStringAsync());
                return fileName + "&" + filePath;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "N/A&N/A";
            }
        }
        //EWAY Bill Details
        public async Task<string> GetPdfEwbDetailsIRN(string invType, string invNo, string returnYear, string returnMonth)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                string token = GenerateBearerToken();
                string body = "https://api.taxilla.com/process/v1/einvoiceewbv1/reports/e-WayBill-Detailed?transaction_id=" + invNo + "&supply_type=Outward&transaction_type=INV&return_year=" + returnYear + "&return_month=" + returnMonth;
                var request = new HttpRequestMessage(HttpMethod.Get, body);
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Headers.Add("app-id", "envoice");
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                request.Content = content;
                DateTime requestTimeStamp = DateTime.Now;
                var response = await client.SendAsync(request);
                DateTime responseTimeStamp = DateTime.Now;
                var content1 = response;
                _gstApi.SaveApiRequestLogs(body, content1.ToString(), requestTimeStamp, responseTimeStamp, "Posted", compId, brId, invNo, DateTime.Now.ToString("yyyy-MM-dd"), "EWB Details PDF");
                response.EnsureSuccessStatusCode();

                var cntnt = await response.Content.ReadAsStreamAsync();
                string folderName = "";
                if (invType.ToUpper() == "INV")
                    folderName = "SaleInvoice/";
                else if (invType.ToUpper() == "SCINV")
                    folderName = "ScrapSaleInvoice/";
                else
                    folderName = "CustomInvoice/";

                string path = Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);
                string folderPath = path + ("..\\Attachment\\");

                bool exists = System.IO.Directory.Exists(folderPath + folderName);
                if (!exists)
                    System.IO.Directory.CreateDirectory(folderPath + folderName);
                string fileName = GenerateFileName("EwbDetails", invNo);
                string filePath = path + ("..\\Attachment\\") + folderName + fileName + ".pdf";
                //FileStream stream = new FileStream(filePath, FileMode.CreateNew);
                //// Create a StreamWriter from FileStream
                //using (StreamWriter writer = new StreamWriter(stream))
                //{
                //    writer.Write(await response.Content.ReadAsStreamAsync());
                //}
                MemoryStream strm = cntnt as MemoryStream;
                byte[] arr = strm.ToArray();
                System.IO.File.WriteAllBytes(filePath, arr);
                //Response.Write(await response.Content.ReadAsStringAsync());
                return fileName + "&" + filePath;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "N/A&N/A";
            }
        }
        //Get api errors to show on report
        public ActionResult GetGstApiErrorDetails(string invNo, string cstInvNo)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();

                DataTable dtErrors = new DataTable();
                dtErrors.Columns.Add("srNo", typeof(int));
                dtErrors.Columns.Add("error", typeof(string));

                DataTable dt = _gstApi.GetApiErrorDetails(compId, brId, invNo.Replace("/", ""));
                if (dt.Rows.Count == 0)
                {
                    if (!string.IsNullOrEmpty(cstInvNo))
                    {
                        invNo = cstInvNo;
                    }
                    //in case of no data found(for custom invoice) replace invno with custom invno
                    //and refill datatable
                    dt = _gstApi.GetApiErrorDetails(compId, brId, invNo.Replace("/", ""));
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JObject obj1 = JObject.Parse(dt.Rows[i]["api_resp"].ToString());

                    if (obj1.ContainsKey("errors"))
                    {
                        JObject err1 = JObject.Parse(obj1["errors"].ToString());
                        DataRow dtr = dtErrors.NewRow();
                        var Error = err1["errors"].ToString();
                        dtr["srNo"] = i;
                        dtr["error"] = Error;
                        //if (err1.ToString() != "[]")
                        if (Error != "[]")
                        {
                            dtErrors.Rows.Add(dtr);
                        }
                        if (err1.ContainsKey("entities"))
                        {
                            var entities = JArray.Parse(err1["entities"].ToString());
                            foreach (JObject objent in entities)
                            {
                                if (objent.ContainsKey("records"))
                                {
                                    var records = JArray.Parse(objent["records"].ToString());
                                    var err2 = records;
                                    foreach (JObject objrec in err2)
                                    {
                                        if (objrec.ContainsKey("errors"))
                                        {
                                            var recErr = JArray.Parse(objrec["errors"].ToString()).ToString();
                                            dtr = dtErrors.NewRow();
                                            dtr["srNo"] = i;
                                            dtr["error"] = recErr;
                                            if (recErr.ToString() != "[]")
                                            {
                                                dtErrors.Rows.Add(dtr);
                                            }
                                        }
                                        /* ------------------Added by Suraj Maurya on 22-04-2025 to Get error from entities>records>fields---------------- */
                                        if (objrec.ContainsKey("fields"))
                                        {
                                            var fields = JArray.Parse(objrec["fields"].ToString());
                                            foreach (JObject objfld in fields)
                                            {
                                                if (objfld.ContainsKey("errors"))
                                                {
                                                    var fldsErr = JArray.Parse(objfld["errors"].ToString()).ToString();
                                                    dtr = dtErrors.NewRow();
                                                    dtr["srNo"] = i;
                                                    dtr["error"] = fldsErr;
                                                    if (fldsErr.ToString() != "[]")
                                                    {
                                                        dtErrors.Rows.Add(dtr);
                                                    }
                                                }
                                            }
                                              
                                        }
                                        if (objrec.ContainsKey("entities"))
                                        {
                                            var recEntities = JArray.Parse(objrec["entities"].ToString());
                                            var recEnt = recEntities;
                                            foreach (JObject recEntt in recEnt)
                                            {
                                                if (recEntt.ContainsKey("records"))
                                                {
                                                    var recEnt2 = JArray.Parse(recEntt["records"].ToString());
                                                    var RecEntRecErr = recEnt2;
                                                    foreach (JObject recentrec in RecEntRecErr)
                                                    {
                                                        if (recentrec.ContainsKey("errors"))
                                                        {
                                                            var recErr3 = JArray.Parse(recentrec["errors"].ToString());
                                                            dtr = dtErrors.NewRow();
                                                            dtr["srNo"] = i;
                                                            dtr["error"] = recErr3;
                                                            if (recErr3.ToString() != "[]")
                                                            {
                                                                dtErrors.Rows.Add(dtr);
                                                            }
                                                        }
                                                        if (recentrec.ContainsKey("fields"))
                                                        {
                                                            var fieldsError = JArray.Parse(recentrec["fields"].ToString());
                                                            foreach (JObject ferr in fieldsError)
                                                            {
                                                                if (ferr.ContainsKey("errors"))
                                                                {
                                                                    var lastError = JArray.Parse(ferr["errors"].ToString());
                                                                    dtr = dtErrors.NewRow();
                                                                    dtr["srNo"] = i;
                                                                    dtr["error"] = lastError;
                                                                    if (lastError.ToString() != "[]")
                                                                    {
                                                                        dtErrors.Rows.Add(dtr);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                ViewBag.ApiErrorDetails = dtErrors;
                return PartialView("~/Areas/Common/Views/Cmn_PartialErrorDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        //Get PDF documents
        public string GetGstApiDocsDetails(string invType, string invNo, string invDt, string docName, string dataType)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataTable dt = _gstApi.GetApiDocsDetails(((dataType == "SSI"|| dataType == "SSC") ?"SINV":invType), compId, brId, invNo, invDt, docName);
                string outPut = "";
                if (dt.Rows.Count > 0)
                {
                    //string[] sep = new string[] { "//" };
                    //string[] path = Request.Url.AbsoluteUri.Split(sep, StringSplitOptions.None);
                    //string[] drivepath = path[1].ToString().Split('/');
                    //string baseUrl = path[0].ToString() + "//" + drivepath[0].ToString();

                    /*---------------------------Commented by Suraj on 12-08-2024---------------------------*/
                    //string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                    //string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                    //if (Request.Url.Host == localIp)
                    //    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                    //string replaceText = "";
                    //if (dt.Rows[0]["doc_path"].ToString().ToLower().Contains("attachment"))
                    //    replaceText = "Attachment";
                    //else
                    //    replaceText = "EnRepMobileWeb";
                    //string[] op = dt.Rows[0]["doc_path"].ToString().Split(new string[] { replaceText }, StringSplitOptions.None);
                    //string url = serverUrl + "/" + op[1].ToString().Replace("\\", "/");

                    //outPut = url;
                    /*---------------------------Commented by Suraj on 12-08-2024 End---------------------------*/

                    /*---------------------------Added by Suraj on 12-08-2024---------------------------*/
                    var AFP = dt.Rows[0]["doc_path"].ToString().Split('\\');
                    var filePath = dt.Rows[0]["doc_path"].ToString().Replace("\"", "/").Replace("Attachment", "");
                    string[] fpath = filePath.Split('.');
                    string Scheme = System.Web.HttpContext.Current.Request.Url.Scheme.ToLower();
                    string Host = System.Web.HttpContext.Current.Request.Url.Host.ToLower();
                    string LocalServerURL = "";
                    string localsysip = ConfigurationManager.AppSettings["LocalServerip"];
                    if (Host == localsysip)
                        LocalServerURL = ConfigurationManager.AppSettings["LocalServerURL"];
                    else if (Host == "localhost")
                        LocalServerURL = ConfigurationManager.AppSettings["LocalServerURL"];
                    else
                        LocalServerURL = ConfigurationManager.AppSettings["LiveServerURL"];

                    string Port = System.Web.HttpContext.Current.Request.Url.Port.ToString().ToLower();
                    string imgstr = "";
                    if (AFP.Length > 1)
                        imgstr = LocalServerURL + AFP[AFP.Length - 2] + "/" + AFP[AFP.Length - 1];
                    else
                        imgstr = Scheme + "://" + Host + ":" + Port + "/" + "Attachment" + "/" + "Error" + "/" + "ErrorInPath.jpg";
                    
                    outPut = imgstr;
                    /*---------------------------Added by Suraj on 12-08-2024 End---------------------------*/
                }
                else
                {
                    // Recheck Status and generate docs 
                    RePostStatusCheck(compId, brId, invType, dataType, invNo, invDt, docName);
                    outPut = "";
                }
                return outPut;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        // Re-Post Request if PDF not generated
        public void RePostStatusCheck(string compId, string brId, string invType, string dataType, string invNo, string invDt, string docName)
        {
            DataSet invDetails;
            if (dataType == "SSI")
                invDetails = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDt);
            else
                invDetails = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDt, invType);
            // post status check api
            string gstInvNo = invDetails.Tables[0].Rows[0]["No"].ToString();
            string finYear = invDetails.Tables[0].Rows[0]["FinYear"].ToString();
            string returnMonth = invDetails.Tables[0].Rows[0]["ReturnMonth"].ToString();
            if (returnMonth.Length == 1)
                returnMonth = "0" + returnMonth;
            //System.Threading.Thread.Sleep(5000);
            // return EInvoiceEWBStatusCheck(dataType, gstInvNo, invDt, finYear, returnMonth, invType);
            string ewbSummaryPdfPath = "", ewbSummaryPdfName = "", ewbDetailsPdfPath = "", ewbDetailsPdfName = "",
                     EinvIrnPdfPath = "", EinvIrnPdfName = "", checkAttachment = "";
            if (docName != "EInvoice")
            {
                //ewb summary
                checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, gstInvNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath.Replace("Attachment", "").ToString(), "A_EWBS");
                if (checkAttachment != "Duplicate")
                {
                    string[] ewbSummary = Task.Run(async () => await GetPdfEwbSummaryIRN(invType, gstInvNo, finYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                    ewbSummaryPdfName = ewbSummary[0].ToString();
                    ewbSummaryPdfPath = ewbSummary[1].ToString();
                    if (ewbSummaryPdfName.ToUpper() != "N/A")
                    {
                        _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, gstInvNo, invDt, ewbSummaryPdfName, ewbSummaryPdfPath.Replace("Attachment", "").ToString(), "A_EWBS");
                    }
                }
                //ewb details
                checkAttachment = _gstApi.CheckGstInvAttachmentDetails(invType, compId, brId, gstInvNo, invDt, ewbDetailsPdfName, ewbDetailsPdfPath.Replace("Attachment", "").ToString(), "A_EWBD");
                if (checkAttachment != "Duplicate")
                {
                    string[] ewbDetails = Task.Run(async () => await GetPdfEwbDetailsIRN(invType, gstInvNo, finYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                    ewbDetailsPdfName = ewbDetails[0].ToString();
                    ewbDetailsPdfPath = ewbDetails[1].ToString();
                    if (ewbDetailsPdfName != "N/A")
                    {
                        _gstApi.UpdateGstInvAttachmentDetails(invType, compId, brId, gstInvNo, invDt, ewbDetailsPdfName, ewbDetailsPdfPath.Replace("Attachment", "").ToString(), "A_EWBD");
                    }
                }
            }
            else
            {
                //einv irn 
                checkAttachment = _gstApi.CheckGstInvAttachmentDetails((dataType == "SSI" ? "SINV" : invType), compId, brId, gstInvNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV");
                if (checkAttachment != "Duplicate")
                {
                    string[] einvPdf = Task.Run(async () => await GetPdfEinvIRN(invType, gstInvNo, finYear, returnMonth)).ConfigureAwait(false).GetAwaiter().GetResult().Split('&');
                    EinvIrnPdfName = einvPdf[0].ToString();
                    EinvIrnPdfPath = einvPdf[1].ToString();
                    if (EinvIrnPdfName.ToUpper() != "N/A")
                    {
                        _gstApi.UpdateGstInvAttachmentDetails((dataType == "SSI" ? "SINV" : invType), compId, brId, gstInvNo, invDt, EinvIrnPdfName, EinvIrnPdfPath.Replace("Attachment", "").ToString(), "A_EINV");
                    }
                }
            }
        }
        //Generate PDF File
        //public FileResult ExportInvoiceToPdf(string invType, string invNo, string invDt)
        //{
        //    string docId = "";
        //    if (invType == "INV")
        //        docId = "105103140";
        //    if (invType == "CSTM")
        //        docId = "105103145125";
        //    DomesticSaleInvoice_Model _model = new DomesticSaleInvoice_Model();
        //    _model.DocumentMenuId = docId;
        //    _model.inv_no = invNo;
        //    _model.inv_dt = invDt;


        //    if (invType == "INV")
        //        return GenratePdfFile(_model);
        //    else
        //        return GenrateCustomPdfFile(invNo, invDt);
        //}
        ////Generate PDF file
        //public FileResult GenratePdfFile(DomesticSaleInvoice_Model _model)
        //{
        //    PrintOptionsList ProntOption = new PrintOptionsList();
        //    if (_model.HdnPrintOptons == "Y")
        //    {
        //        ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
        //        ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
        //        ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
        //    }
        //    else
        //    {
        //        ProntOption.PrtOpt_catlog_number = true;
        //        ProntOption.PrtOpt_item_code = true;
        //        ProntOption.PrtOpt_item_desc = true;
        //    }
        //    return File(GetPdfData(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        //}
        ////Generate Custom Invoice PDF file
        //public FileResult GenrateCustomPdfFile(string invNo, string invDt)
        //{
        //    try
        //    {
        //        var data = GetCustomPdfData(invNo, invDt.Substring(0, 10));
        //        if (data != null)
        //            return File(data, "application/pdf", /*ViewBag.Title.Replace(" ", "").*/"ExportInvoice" + ".pdf");
        //        else
        //            return File("ErrorPage", "application/pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        // return null;
        //        throw ex;
        //    }

        //}
        ////Get Sale Invoice data in byte array to print
        //public byte[] GetPdfData(string docId, string invNo, string invDt, PrintOptionsList ProntOption)
        //{
        //    StringReader reader = null;
        //    Document pdfDoc = null;
        //    PdfWriter writer = null;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            compId = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            brId = Session["BranchId"].ToString();
        //        }
        //        string inv_type = "";
        //        string ReportType = "common";
        //        if (docId == "105103140")
        //        {
        //            inv_type = "SI";
        //        }
        //        if (docId == "105103145125")
        //        {
        //            inv_type = "CI";
        //        }
        //        DataSet Details = _LocalSalesInvoice_ISERVICES.GetSalesInvoiceDeatilsForPrint(compId, brId, invNo, invDt.Substring(0, 10), inv_type);
        //        ViewBag.PageName = "SI";
        //        string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();

        //        ViewBag.Details = Details;
        //        ViewBag.InvoiceTo = "";
        //        ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
        //        ViewBag.ProntOption = ProntOption;
        //        string htmlcontent = "";
        //        if (invType == "D")
        //        {
        //            ViewBag.Title = "Sales Invoice";
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoicePrint.cshtml"));
        //        }
        //        else
        //        {
        //            ViewBag.Title = "Commercial Invoice";
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
        //        }

        //        using (MemoryStream stream = new System.IO.MemoryStream())
        //        {
        //            reader = new StringReader(htmlcontent);
        //            if (ReportType == "common")
        //            {
        //                if (inv_type == "SI")
        //                {
        //                    pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        //                }
        //                else
        //                {
        //                    pdfDoc = new Document(PageSize.A4, 20f, 20f, 70f, 90f);
        //                }
        //            }

        //            writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
        //            pdfDoc.Close();
        //            Byte[] bytes = stream.ToArray();
        //            bytes = HeaderFooterPagination(bytes, Details, ReportType, inv_type);
        //            return bytes.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}
        //private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string inv_type)
        //{
        //    var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
        //    var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font font = new Font(bf, 9, Font.NORMAL);
        //    Font fontIRN = new Font(bf, 5, Font.NORMAL);
        //    Font font1 = new Font(bf, 8, Font.NORMAL);
        //    Font fontb = new Font(bf, 9, Font.NORMAL);
        //    fontb.SetStyle("bold");
        //    Font fonttitle = new Font(bf, 13, Font.BOLD);
        //    fonttitle.SetStyle("underline");
        //    string logo = Server.MapPath(Details.Tables[0].Rows[0]["logo"].ToString());
        //    string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
        //    //string QR = "";
        //    string irnNo = "IRN - " + Details.Tables[0].Rows[0]["gst_irn_no"].ToString();
        //    string draftImage = Server.MapPath("~/Content/Images/draft.png");

        //    using (var reader1 = new PdfReader(bytes))
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            using (var stamper = new PdfStamper(reader1, ms))
        //            {
        //                if (ReportType == "common")
        //                {
        //                    if (inv_type == "SI")
        //                    {
        //                        var draftimg = Image.GetInstance(draftImage);
        //                        draftimg.SetAbsolutePosition(20, 220);
        //                        draftimg.ScaleAbsolute(550f, 600f);

        //                        var qrCode = Image.GetInstance(QR);
        //                        qrCode.SetAbsolutePosition(460, 670);
        //                        qrCode.ScaleAbsolute(100f, 95f);

        //                        int PageCount = reader1.NumberOfPages;
        //                        for (int i = 1; i <= PageCount; i++)
        //                        {
        //                            var content = stamper.GetUnderContent(i);
        //                            if (docstatus == "D" || docstatus == "F")
        //                            {
        //                                content.AddImage(draftimg);
        //                            }
        //                            if (i == 1)
        //                                content.AddImage(qrCode);
        //                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                            Phrase irn = new Phrase(String.Format(irnNo, i, PageCount), fontIRN);
        //                            if (i == 1)
        //                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, irn, 560, 665, 0);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var image = Image.GetInstance(logo);
        //                        image.SetAbsolutePosition(31, 794);
        //                        image.ScaleAbsolute(68f, 15f);

        //                        var draftimg = Image.GetInstance(draftImage);
        //                        draftimg.SetAbsolutePosition(20, 220);
        //                        draftimg.ScaleAbsolute(550f, 600f);

        //                        int PageCount = reader1.NumberOfPages;
        //                        for (int i = 1; i <= PageCount; i++)
        //                        {
        //                            var content = stamper.GetUnderContent(i);
        //                            if (docstatus == "D" || docstatus == "F")
        //                            {
        //                                content.AddImage(draftimg);
        //                            }
        //                            content.AddImage(image);
        //                            content.Rectangle(34.5, 28, 526, 60);


        //                            BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
        //                            content.BeginText();
        //                            content.SetColorFill(CMYKColor.BLACK);
        //                            content.SetFontAndSize(baseFont1, 9);
        //                            content.CreateTemplate(20, 10);
        //                            content.SetLineWidth(25);

        //                            var txt = Details.Tables[0].Rows[0]["declar"].ToString();
        //                            string[] stringSeparators = new string[] { "\r\n" };
        //                            string text = txt;
        //                            string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

        //                            var y = 65;
        //                            for (var j = 0; j < lines.Length; j++)
        //                            {
        //                                content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
        //                                y = y - 10;
        //                            }

        //                            txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
        //                            text = txt;
        //                            string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

        //                            content.SetFontAndSize(baseFont1, 8);
        //                            y = 771;
        //                            for (var j = 0; j < lines1.Length; j++)
        //                            {
        //                                content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
        //                                y = y - 10;
        //                            }
        //                            content.SetFontAndSize(baseFont1, 9);

        //                            content.EndText();

        //                            //content.Rectangle(450, 25, 120, 35);
        //                            string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
        //                            // Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
        //                            Phrase irn = new Phrase(String.Format(irnNo, i, PageCount), fontIRN);
        //                            Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
        //                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                            Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
        //                            //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
        //                            Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
        //                            Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
        //                            //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
        //                            //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
        //                            //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
        //                            //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
        //                            /*------------------Header ---------------------------*/
        //                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
        //                            if (i == 1)
        //                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, irn, 560, 737, 0);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

        //                            /*------------------Header end---------------------------*/

        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
        //                            //ColumnText.AddText(p1);
        //                            //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
        //                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
        //                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
        //                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
        //                        }
        //                    }
        //                }


        //            }
        //            bytes = ms.ToArray();
        //        }
        //    }

        //    return bytes;
        //}
        ////Get custom Invoice data in byte array to print
        //public byte[] GetCustomPdfData(string invNo, string invDt)
        //{
        //    StringReader reader = null;
        //    Document pdfDoc = null;
        //    PdfWriter writer = null;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            compId = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            brId = Session["BranchId"].ToString();
        //        }
        //        string ReportType = "custom";
        //        DataSet Details = _CustomInvoice_ISERVICE.GetCustomInvoiceDeatilsForPrint(compId, brId, invNo, invDt, ReportType);
        //        ViewBag.PageName = "CI";
        //        ViewBag.Title = "Custom Invoice";
        //        ViewBag.Details = Details;
        //        ViewBag.InvoiceTo = "";
        //        ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
        //        string htmlcontent = "";
        //        if (ReportType == "common")
        //        {
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoicePrint.cshtml"));
        //        }
        //        else
        //        {
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_CostumInvoicePrint.cshtml"));
        //        }
        //        using (MemoryStream stream = new System.IO.MemoryStream())
        //        {
        //            reader = new StringReader(htmlcontent);
        //            if (ReportType == "common")
        //            {
        //                pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        //            }
        //            else
        //            {
        //                pdfDoc = new Document(PageSize.A4, 20f, 20f, 103f, 90f);
        //            }
        //            writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
        //            pdfDoc.Close();
        //            Byte[] bytes = stream.ToArray();
        //            bytes = CustomHeaderFooterPagination(bytes, Details, ReportType);

        //            return bytes.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        throw ex;
        //        // return null;
        //    }
        //    finally
        //    {

        //    }
        //}
        ////Set custom print style 
        //private Byte[] CustomHeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType)
        //{

        //    var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
        //    var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font font = new Font(bf, 9, Font.NORMAL);
        //    Font fontIRN = new Font(bf, 5, Font.NORMAL);
        //    Font font1 = new Font(bf, 8, Font.NORMAL);
        //    Font fontb = new Font(bf, 9, Font.NORMAL);
        //    fontb.SetStyle("bold");
        //    Font fonttitle = new Font(bf, 13, Font.BOLD);
        //    fonttitle.SetStyle("underline");
        //    string logo = Server.MapPath(Details.Tables[0].Rows[0]["logo"].ToString());
        //    string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
        //    //string QR = "";
        //    string draftImage = Server.MapPath("~/Content/Images/draft.png");
        //    string irnNo = "IRN - " + Details.Tables[0].Rows[0]["gst_irn_no"].ToString();
        //    //if (irnNo == "IRN - ")
        //    //    irnNo = "IRN - 09878654335678908765467111111111111111111118907657890768675467890";
        //    using (var reader1 = new PdfReader(bytes))
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            using (var stamper = new PdfStamper(reader1, ms))
        //            {
        //                if (ReportType == "common")
        //                {
        //                    var draftimg = Image.GetInstance(draftImage);
        //                    draftimg.SetAbsolutePosition(20, 220);
        //                    draftimg.ScaleAbsolute(550f, 600f);

        //                    var qrCode = Image.GetInstance(QR);
        //                    qrCode.SetAbsolutePosition(460, 670);
        //                    qrCode.ScaleAbsolute(100f, 95f);

        //                    int PageCount = reader1.NumberOfPages;
        //                    for (int i = 1; i <= PageCount; i++)
        //                    {
        //                        var content = stamper.GetUnderContent(i);
        //                        if (docstatus == "D" || docstatus == "F")
        //                        {
        //                            content.AddImage(draftimg);
        //                        }
        //                        if (i == 1)
        //                            content.AddImage(qrCode);
        //                        Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
        //                    }
        //                }
        //                else
        //                {
        //                    var image = Image.GetInstance(logo);
        //                    image.SetAbsolutePosition(31, 794);
        //                    image.ScaleAbsolute(68f, 15f);

        //                    var draftimg = Image.GetInstance(draftImage);
        //                    draftimg.SetAbsolutePosition(20, 220);
        //                    draftimg.ScaleAbsolute(550f, 600f);

        //                    var qrCode = Image.GetInstance(QR);
        //                    qrCode.SetAbsolutePosition(475, 740);
        //                    qrCode.ScaleAbsolute(100f, 95f);

        //                    int PageCount = reader1.NumberOfPages;
        //                    for (int i = 1; i <= PageCount; i++)
        //                    {
        //                        var content = stamper.GetUnderContent(i);
        //                        if (docstatus == "D" || docstatus == "F")
        //                        {
        //                            content.AddImage(draftimg);
        //                        }
        //                        content.AddImage(image);
        //                        content.Rectangle(34.5, 28, 526, 60);
        //                        if (i == 1)
        //                            content.AddImage(qrCode);
        //                        //-----------------Adding Declaration--------------
        //                        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
        //                        content.BeginText();
        //                        content.SetColorFill(CMYKColor.BLACK);
        //                        content.SetFontAndSize(baseFont1, 9);
        //                        content.CreateTemplate(20, 10);
        //                        content.SetLineWidth(25);

        //                        var txt = Details.Tables[0].Rows[0]["declar"].ToString();
        //                        string[] stringSeparators = new string[] { "\r\n" };
        //                        string text = txt;
        //                        string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

        //                        var y = 65;
        //                        for (var j = 0; j < lines.Length; j++)
        //                        {
        //                            content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
        //                            y = y - 10;
        //                        }
        //                        txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
        //                        text = txt;
        //                        string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

        //                        content.SetFontAndSize(baseFont1, 8);
        //                        y = 771;
        //                        for (var j = 0; j < lines1.Length; j++)
        //                        {
        //                            content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
        //                            y = y - 10;
        //                        }
        //                        content.SetFontAndSize(baseFont1, 9);
        //                        //-----------------Adding Declaration--------------

        //                        //content.Rectangle(450, 25, 120, 35);
        //                        string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
        //                        //Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
        //                        Phrase irn = new Phrase(String.Format(irnNo, i, PageCount), fontIRN);
        //                        Phrase ptitle = new Phrase(String.Format("Export Invoice", i, PageCount), fonttitle);
        //                        Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
        //                        Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
        //                        //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
        //                        Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
        //                        Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
        //                        //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
        //                        //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
        //                        //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
        //                        /*------------------Header ---------------------------*/
        //                        // ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
        //                        if (i == 1)
        //                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, irn, 560, 737, 0);
        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

        //                        /*------------------Header end---------------------------*/

        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
        //                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
        //                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
        //                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
        //                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
        //                    }
        //                }

        //            }
        //            bytes = ms.ToArray();
        //        }
        //    }

        //    return bytes;
        //}
        ////Convert partial view to string for PDF Generation
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
        //Select Print Type
        public ActionResult GetSelectPrintTypePopup(string PrintOptions, string DocMenuId)
        {
            try
            {
                ViewBag.PrintCommand = "Y";
                ViewBag.PrintOptions = PrintOptions;
                ViewBag.DocMenuId = DocMenuId;
                return PartialView("~/Areas/Common/Views/Cmn_SelectPrintType.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        // Generate Envoice QR code to attach on Invoice Print
        private string GenerateQRCode(string qrcodeText)
        {
            if (string.IsNullOrEmpty(qrcodeText))
                qrcodeText = "N/A";
            Random rand = new Random();

            string fileName = "QR_" + rand.Next(111111, 999999).ToString();
            string path = Server.MapPath("~");
            string folderPath = path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\");
            string imagePath = folderPath + fileName + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = imagePath;
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }
        //Generate PDF file end
        //EInvoice Preview
        //Generate file to return 
        public FileResult GenerateEInvoicePreview(string invNo, string invDate, string invType, string dataType)
        {
            ViewBag.Title = "EInvoicePreview";
            return File(GetEInvoicePreviewData(invNo, invDate, invType, dataType), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        //Getting data from database and convert it to Byte array
        public byte[] GetEInvoicePreviewData(string invNo, string invDate, string invType, string dataType)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                DataSet dsEinvDetails;
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                if (dataType == "SSI")
                    dsEinvDetails = _gstApi.GetServiceSalesInvoiceData(compId, brId, invNo, invDate);
                else
                    dsEinvDetails = _gstApi.GetSalesInvoiceData(compId, brId, invNo, invDate, (dataType == "SRT" ? dataType : invType));

                ViewBag.EinvPreviewDetails = dsEinvDetails;
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GSTAPI/GstEinvoicePreview.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 70f, 90f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = EInvoicePreviewHFPagination(bytes, dsEinvDetails);
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
        //EInvoice preview is used to confirm E-Invoice data before send on emg portal
        private Byte[] EInvoicePreviewHFPagination(Byte[] bytes, DataSet Details)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font fontIRN = new Font(bf, 5, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 13, Font.BOLD);
            fonttitle.SetStyle("underline");
            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        int PageCount = reader1.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            //string previewImage = Server.MapPath("~/Content/Images/preview.png");
                            //var previewImg = Image.GetInstance(previewImage);
                            //previewImg.SetAbsolutePosition(100, 30);
                            //previewImg.ScaleAbsolute(600f, 650f);

                            ////~/Content/Images/preview.png
                            //var content = stamper.GetUnderContent(i);
                            //content.Rectangle(34.5, 28, 526, 60);
                            //BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                            //content.BeginText();
                            //content.SetColorFill(CMYKColor.BLACK);
                            //content.SetFontAndSize(baseFont1, 9);
                            //content.CreateTemplate(20, 10);
                            //content.SetLineWidth(25);

                            //content.AddImage(previewImg);

                            //string[] stringSeparators = new string[] { "\r\n" };
                            //content.SetFontAndSize(baseFont1, 8);
                            //content.SetFontAndSize(baseFont1, 9);
                            //content.EndText();

                            //Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 800, 15, 0);



                            string previewImage = Server.MapPath("~/Content/Images/preview.png");
                            var previewImg = Image.GetInstance(previewImage);
                            previewImg.SetAbsolutePosition(100, 30);
                            previewImg.ScaleAbsolute(600f, 650f);

                            //~/Content/Images/preview.png
                            var content = stamper.GetUnderContent(i);

                            content.AddImage(previewImg);
                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 800, 15, 0);
                        }
                    }
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }
        //asynchronious method to send data on EMG portal for reconsilation
        public async Task<string> ReconsileDataOnEmgPortal(string dataType, string fromDate, string toDate,string GSTR_DateOption)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();

                DataTable dt = _gstApi.GetSaleRegisterDetails(dataType, compId, brId, fromDate, toDate, GSTR_DateOption);
                if (dt.Rows.Count < 1)
                    return "No pending entries found";
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);
                string folderPath = path + ("..\\Attachment\\GSTAPI");
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderPath);
                }
                string dType = "GSTR1";
                if (dataType == "PR")
                    dType = "GSTR2";
                DataTable dtFileName = _gstApi.GetExcelFileNameForEMPReconsile(dType, compId, brId, fromDate);
                string name = dtFileName.Rows[0]["gstrFileName"].ToString();
                string finalPath = folderPath + "\\" + name + ".csv";
                // delete old file if already exist with same name
                if (System.IO.File.Exists(finalPath))
                {
                    System.IO.File.Delete(finalPath);
                }
                ExportToCsv(dt, finalPath);
                var client = new RestClient("https://app-prod.easemygst.com");
                var request = new RestRequest("/easemygst/emgst/extws/emgst/upload/csv", Method.Post);
                request.AddHeader("Content-Type", "multipart/form-data");
                //request.AlwaysMultipartFormData = true;
                request.AddFile("file", finalPath);
                request.AddParameter("mkey", "abc");
                request.AddParameter("serialNo", "abc");
                DateTime RequestTimeStamp = DateTime.Now;
                RestResponse response = await client.ExecuteAsync(request);
                DateTime ResponseTimeStamp = DateTime.Now;
                var content = response.Content;
                //content = {"entity":null,"status":200,"metadata":{},"annotations":null,"entityClass":null,"genericType":null,"lastModified":null,"allowedMethods":[],"mediaType":null,"stringHeaders":{},"entityTag":null,"links":[],"statusInfo":"OK","length":-1,"location":null,"language":null,"date":null,"cookies":{},"closed":false,"headers":{}}
                ///////////////// Request raised Log ///////////////////////////////
                _gstApi.SaveApiRequestLogs("Excel File : ", content, RequestTimeStamp, ResponseTimeStamp, "Posted", compId, brId, fromDate, toDate, "Sale Register Request Raised");
                JObject obj = JObject.Parse(content);
                //checking if status key contains or not
                if (response.IsSuccessStatusCode)
                {
                    // typecast response code as integer
                    int stsCode = (int)response.StatusCode;
                    if (stsCode == 204)
                        return "No data to send for reconsile.";
                    //GST Recon SUCCESS LOGIC 
                    return "GST Reconsilation Successful...!!";
                }
                else
                {
                    string msg = response.ErrorException.ToString();
                    //Generate Failed Logs
                    _gstApi.SaveApiRequestLogs("Excel Upload Response", content, RequestTimeStamp, ResponseTimeStamp, "Failed", compId, brId, fromDate, toDate, response.ErrorException.ToString());
                    return msg;
                }
            }
            catch (Exception ex)
            {
                // return exception occured with the error msg so that user can easily understand the error.
                return "Internal Server Error : " + ex.Message;
            }
        }
        //Code wrote by sanjay to convert datatable to csv
        public static void ExportToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write the header line
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    writer.Write(dataTable.Columns[i].ColumnName);
                    if (i < dataTable.Columns.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Write the data rows
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        if (i == 2)
                            writer.Write(row[i]);
                        else
                            writer.Write(row[i].ToString());
                        if (i < dataTable.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        
        public FileResult ReconsileDataCsvPreview(string dataType, string fromDate, string toDate, string GSTR_DateOption)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    brId = Session["BranchId"].ToString();
                DataTable dt = _gstApi.GetSaleRegisterDetails(dataType, compId, brId, fromDate, toDate, GSTR_DateOption);
                if (dt.Rows.Count < 1)
                    return null;
                
                string dType = "GSTR1";
                if (dataType == "PR")
                    dType = "GSTR2";
                DataTable dtFileName = _gstApi.GetExcelFileNameForEMPReconsile(dType, compId, brId, fromDate);
                string name = dtFileName.Rows[0]["gstrFileName"].ToString();
                //string finalPath = folderPath + "\\" + name + ".csv";
                // delete old file if already exist with same name
                
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel(name, dt);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
           
        }
    }
}