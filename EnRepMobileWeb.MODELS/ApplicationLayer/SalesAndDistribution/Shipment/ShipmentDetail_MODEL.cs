using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Shipment
{
    public class ShipmentDetail_MODEL
    {

  
        public string CancelledRemarks { get; set; }
        public string cust_trnsport_id { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Cust_term_del_pay { get; set; }
        public string custom_local_port { get; set; }
        public string CMN_Command { get; set; }
        public string ShipMent_type { get; set; }
        //public string title { get; set; }
        public string DocumentStatusCode { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocId { get; set; }
        public string WF_Status1 { get; set; }
        public string AppStatus { get; set; }
        public string ShipmentDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string MenuDocumentId { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string Customer_type { get; set; }
        public string ship_type { get; set; }
        public string print_type { get; set; }
        public string DeleteCommand { get; set; }
        public string EditCommand { get; set; }
        public string ship_no { get; set; }
        public string ship_dt { get; set; }
        public string CustomerID { get; set; }
        public string cust_id { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public string FilterPackNumber { get; set; }
        public string filterCustomerName { get; set; }
        public string SO_CustName { get; set; }
        public string bill_address { get; set; }
        public string ship_address { get; set; }
        public string ship_add_id { get; set; }
        public string bill_add_id { get; set; }
        public string trpt_name { get; set; }
        public string veh_number { get; set; }
        public string driver_name  { get; set; }
        public string mob_no { get; set; }
        public string tot_tonnage { get; set; }

        public string gr_no { get; set; }
        public string hdnGrNumber { get; set; }
        public string gr_dt { get; set; }      
        public string cntry_dest { get; set; }
        public string cntry_origin { get; set; }
        public string local_port { get; set; }
        public string loading_port { get; set; }
        public string Hdnloading_port { get; set; }
        public string discharge_port { get; set; }
        public string destination_port { get; set; }
        public string carrier_name { get; set; }
        public string carrier_no { get; set; }
        public string container_no { get; set; }
        public string so_remarks { get; set; }
        public string pack_num { get; set; }
        public string pack_dte { get; set; }
        public string curr_des { get; set; }
        public string curr_id { get; set; }
        public string ExporterAddress { get; set; }
        public List<PackListNumber> PackListNumberList { get; set; }
        public string  PACK_Number { get; set; }
        public string create_dt { get; set; }
        //public string create_id { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string ShipmentItemdetails { get; set; }
        public string Transpoterdetails { get; set; }
        public Boolean CancelFlag { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string CompId  { get; set; }
        public string BrchID  { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string TotalGrossWgt { get; set; }
        public string TotalNetWgt { get; set; }
        public string TotalCBM { get; set; }
        public string attatchmentdetail { get; set; }

        public string VouType { get; set; }
        public string VouNo { get; set; }
        public string VouDt { get; set; }
        public string Narr { get; set; }
        public string Voudetails { get; set; }
        public string Guid { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
       public string LineSealNumber { get; set; }
       public string SelfSealNumber { get; set; }
       public string ContainerNetWeight { get; set; }
       public string ContainerGrossWeight { get; set; }
       public string EWBNNumber { get; set; }
       public string PvtMark { get; set; }
       public List<TransListModel> TransList { get; set; }
        //--------------------Other Detail---------------------
        public string pre_carr_by { get; set; }
        public string trade_term { get; set; }
        public List<trade_termList> TradeTermsList { get; set; }// Added by Suraj on 20-10-2023 for trade term add in SI
        public string pi_rcpt_carr { get; set; }
        public string ves_fli_no { get; set; }
        //public string loading_port { get; set; }
        //public string discharge_port { get; set; }
        public string fin_disti { get; set; }
        //public string container_no { get; set; }
        public string other_ref { get; set; }
        public string term_del_pay { get; set; }
        public string des_good { get; set; }
        public string prof_detail { get; set; }
        public string declar { get; set; }
        public string BuyerIfOtherThenConsignee { get; set; }
        public string CountryOfOriginOfGoods { get; set; }
        public string CountryOfFinalDestination { get; set; }
        public string ExportersReference { get; set; }
        public string ConsigneeAddress { get; set; }
        public string BuyersOrderNumberAndDate { get; set; }
        public string IRNNumber { get; set; }
        public string ConsigneeName { get; set; }
        public string UserID { get; set; }
        //--------------------Other Detail End---------------------

        //-----------------------For Print Options

        public string PrtOpt_catlog_number { get; set; }
        public string PrtOpt_item_code { get; set; }
        public string PrtOpt_item_desc { get; set; }
        public string HdnPrintOptons { get; set; }
        public string custom_inv_no { get; set; }
        public string No_Of_Packages { get; set; }
        public string hdnNumberOfPacks { get; set; }
        public string DuplicateCustmInvNo { get; set; }
        public string CustomInvDate { get; set; }
        public List<PortOfLoadingListModel> PortOfLoadingList { get; set; }
        public List<pi_rcpt_carrListModel> pi_rcpt_carrList { get; set; }
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";
        public string CustomerAliasName { get; set; } = "N"; 
        public string ShowItemRemarks { get; set; } = "N"; 
        public string ItemAliasName { get; set; } = "N";
        public string ShowPackSize { get; set; } = "N";
    }
    public class PrintOptionsList
    {
        public bool PrtOpt_catlog_number { get; set; }
        public bool PrtOpt_item_code { get; set; }
        public bool PrtOpt_item_desc { get; set; }
    }
    public class trade_termList
    {
        public string TrdTrms_id { get; set; }
        public string TrdTrms_val { get; set; }
    }
    public class ShipMentModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlModel
    {
        public string Btn { get; set; }
        public string Cmd { get; set; }
        public string SO_SalePerson { get; set; }
        public string Spmt_no { get; set; }
        public string Spmt_dt { get; set; }
        public string Trns_Typ { get; set; }
        public string WF_sts1 { get; set; }
        public string Apsts { get; set; }
        public string DocId { get; set; }
        public string Sp_typ { get; set; }
        public string DocumentMenuId { get; set; }
    }
    public class GL_Detail
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public float Value { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string TransType { get; set; }

    }

    public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }
    public class PackListNumber
    {
        public string packing_no { get; set; }
        public string packing_dt { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class PortOfLoadingListModel
    {
        public string POL_id { get; set; }
        public string POL_Name { get; set; }
        //public string pin_number { get; set; }
        public string State_Name { get; set; }
    }   
    public class pi_rcpt_carrListModel
    {
        public string Pi_id { get; set; }
        public string Pi_Name { get; set; }
    }
}
