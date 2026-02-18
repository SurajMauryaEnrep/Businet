using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.SupplierPriceList
{
    public class SupplierPriceList_Model
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string DocumentStatus { get; set; }
        public string BtnName { get; set; }
        public string ListFilterData1 { get; set; }
        public string UserID { get; set; }
        public string SuppID { get; set; }
        public string suppname { get; set; }
        public string hdnSuppId { get; set; }
        public string Category { get; set; }
        public string HdnCategory { get; set; }
        public string Portfolio { get; set; }
        public string HdnPortfolio { get; set; }
        public string ValidUpto { get; set; }
        public string DeleteCommand { get; set; }
        public string ItemDetails { get; set; }
        public string RevItemDetails { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string Create_id { get; set; }
        public string CreatedBy { get; set; }
        public string Createdon { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status_name { get; set; }
        public string WF_Status1 { get; set; }
        public bool ActiveStatus { get; set; } = true;
        public string Search { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class SupplierPriceListModel
    {
        public string Title { get; set; }
        public string catalog { get; set; }
        public string portfolio { get; set; }
        public string ActStatus { get; set; }
        public string ValidUpto { get; set; }
        public string ListFilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string wfdocid { get; set; }
        public string wf_status { get; set; }
        public List<catalogName> CatalogList { get; set; }
        public List<PortfolioName> portfolioList { get; set; }
    }
    public class catalogName
    {
        public string catalog_id { get; set; }
        public string catalog_name { get; set; }
    }
    public class PortfolioName
    {
        public string Portfolio_id { get; set; }
        public string Portfolio_name { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message1 { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string ListFilterData1 { get; set; }
    }
}
