using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management
{
    public class DeliveryNoteDetail_MODELS
    {
        public List<ItemName> ItemNameList { get; set; }
        public string ddl_ItemName { get; set; }
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string VehicleNumber { get; set; }
        public string TransporterName { get; set; }
        public string FreightAmount { get; set; }
        public string GRNumber { get; set; }
        public DateTime? GRDate { get; set; }
        public string Title { get; set; }
        public string SubitemFlag { get; set; }
        public string duplicateBillNo { get; set; } = "N";
        public string DocumentNo { get; set; }
        public string SupplierID { get; set; }
        public string dn_id { get; set; }
        public string MenuDocumentId { get; set; }
        public string dn_type { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string Status { get; set; }
        public string WFBarStatus { get; set; }
        public string ListFilterData1 { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string doc_status { get; set; }
        public string dn_status { get; set; }

        public string dn_no { get; set; }

        //public DateTime? dn_dt { get; set; }
        public string dn_dt { get; set; }
        public DateTime? bill_date { get; set; }
       
        public string bill_no { get; set; }

        public string veh_no { get; set; }

        public string veh_load { get; set; }

        public string dn_rem { get; set; }

        public string supp_id { get; set; }
        public string SupplierName { get; set; }
 
        public DateTime? src_doc_date { get; set; }
        public string attatchmentdetail { get; set; }

        public string src_doc_no { get; set; }

        public string DocumentId { get; set; }

       public List<SupplierName> SupplierNameList { get; set; }

        public List<DocumentNumber> DocumentNumberList { get; set; }

        public List<ItemDetails> ItemDetailsList { get; set; }

        public string DnItemdetails { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }

        public string br_id { get; set; }
       public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string imp_file_no { get; set; }
        public string cntry_origin { get; set; }
        public string mr_dt { get; set; }
        public string  mr_no { get; set; }
        public string CreateBy { get; set; }

        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }


        public Boolean CancelFlag { get; set; }

        public string pending_qty { get; set; }

        public string DeleteCommand { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string WF_status1 { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string SubItemDetailsDt { get; set; }//For Sub-item
        public string PO_Type { get; set; }//For Purchase order type 'I' or  'D'
    }

    public class ItemName
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class DeliveryNoteModelAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class URlModelData
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string dn_no { get; set; }
        public string dn_dt { get; set; }
        public string TransType { get; set; }
    }

    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }

    public class DocumentNumber
    {
        public string po_no { get; set; }
        public string po_dt { get; set; }
        public string DocumentType { get; set; }

    }

    public class ItemDetails
    {
        public string item_id { get; set; }
        public string ord_qty { get; set; }
        public string base_uom_id { get; set; }
        public string ord_qty_base { get; set; }
        public string SourceDocumentNo { get; set; }
        public string SourceDocumentDate { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string OrderQty { get; set; }
        public string BilledQty { get; set; }
        public string RecievedQty { get; set; }
        public Boolean QCRequired { get; set; }
        public string AcceptedQty { get; set; }
        public string RejectedQty { get; set; }
        public string ReworkableQty { get; set; }
        public Boolean SampleRecieved { get; set; }
        public string it_remarks { get; set; }
    }
}
