using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ShopfloorStockTransfer
{
    public class ShopfloorStockTransfer_Model
    {
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string shopfloorSource { get; set; }
        public string shopfloorSource_id { get; set; }
        public string Destnation { get; set; }
        public string Destnation_id { get; set; }
        public string trf_no { get; set; }
        public string trf_dt { get; set; }
        public string TransferType { get; set; }
        public string MaterialType { get; set; }
        public string remarks { get; set; }
        public string create_by { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string create_id { get; set; }
        public string create_on { get; set; }
        public string app_by { get; set; }
        public string app_on { get; set; }
        public string mod_by { get; set; }
        public string mod_on { get; set; }
        public string status { get; set; }
        public string StatusCode { get; set; }
        public string mac_id { get; set; }
        public string DeleteCommand { get; set; }
        public string TransType { get; set; }
        public string Title { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string ItemDetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ListStatus { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string BtnName { get; set; }
        public string SSTSearch { get; set; }
        public string SHPF_No { get; set; }
        public string SHPF_Dt { get; set; }
        public string WF_Status1 { get; set; }
        public string WF_Status { get; set; }
        public string DocumentStatus { get; set; }
        public string Cmd_batch { get; set; }
        //public string Status { get; set; }
        public List<LStatus> StatusList { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string SHPF_No { get; set; }
        public string SHPF_Dt { get; set; }
        public string DMS { get; set; }

    }
    public class OrderNumber
    {
        public string porder_no { get; set; }
        public string porder_dt { get; set; }
    }
    public class LStatus
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
