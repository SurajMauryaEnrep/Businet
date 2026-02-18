using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.MISOrderIntimation
{
    public class OrderIntimation_Model
    {
        public string Title { get; set; }
        public string cust_id { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string ord_no { get; set; }
        public string SalePerson { get; set; }
        public string ODFilter { get; set; }
        public string hdnCommand { get; set; }
        public string hdnIntimationList { get; set; }
        public string ShowItemName { get; set; } = "Y";
        public string ShowTechSpec { get; set; } = "N";
        public string ShowTechDesc { get; set; } = "N";
        public string ShowRefNumber { get; set; } = "N";
        public string ShowWeight { get; set; } = "N";
        public string ShowUom { get; set; } = "Y";
        public string ShowCustName { get; set; } = "N";
        public string ShowPrice { get; set; } = "N";
        public string ShowCustSpecItemDesc { get; set; } = "N";
        public string ShowOEMNo { get; set; } = "N";
        public string ShowBomAvl { get; set; } = "N";
        public string ShowHSN { get; set; } = "N";
        public string ShowPendingAmt { get; set; } = "N";
        public string ShowRemarks { get; set; } = "N";
        public string ShowOrderedQuantity { get; set; } = "N";
        public string ItemId { get; set; }
        public List<OrdNumberList> OrderNumbers { get; set; }
        public List<SalePersonList> SalePersonList { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
    }
    public class OrdNumberList
    {
        public string Order_number { get; set; }
        public string Order_date { get; set; }
    }
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
