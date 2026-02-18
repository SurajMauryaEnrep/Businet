using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.MISOrderDetail
{
    public class MISOrderDetail_Model
    {
        public string Title { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ShowAs { get; set; }
        public string SuppId { get; set; }
        public string ItemId { get; set; }
        public string CurrId { get; set; }
        public string SourceType { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string SearchStatus { get; set; }
        public string OrderDateOrderWise { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<SupplierModel> SuppliersList { get; set; }
        public List<StatusModel> StatusList { get; set; }
        public List<CurrencyList> Currencylist { get; set; }
        public List<SuppCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<SuppPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public string hdbr_id { get; set; }
    }
    public class SuppCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class SuppPortFolioList
    {
        public string Port_id { get; set; }
        public string Port_val { get; set; }

    }
    public class ItemsModel
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class SupplierModel
    {
        public string SuppId { get; set; }
        public string SuppName { get; set; }
    }
    public class StatusModel
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
    }
    public class CurrencyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }
}
