using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.GatePassInward
{
    public class GatePassInwardDetail_Model
    {
        public string bill_add_id { get; set; }
        public string Address { get; set; }
        public string A_Level { get; set; }
        public string A_Status { get; set; }
        public string A_Remarks { get; set; }
        public string Qty_pari_Command { get; set; }
        public string SaveSrcDocDeatil { get; set; }
        public string HDNItemDeatilDataSave { get; set; }
        public string Message { get; set; }
        public string attatchmentdetail { get; set; }
        public string SrcDocNo { get; set; }
        public string entity_type { get; set; }
        public string DocumentID { get; set; }
        public string EntityTypeID { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string EntityID { get; set; }
        public string Source_Type { get; set; }
        public string SourceType { get; set; }
        public string Title { get; set; }
        public string DeleteCommand { get; set; }
        public string Massage { get; set; } 
        public string TransType { get; set; }
        public string GatePassDate { get; set; }
        public string GatePassNumber { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string Created_by { get; set; }
        public string Created_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string StatusName { get; set; }
        public string Status_Code { get; set; }
        public string WF_Status1 { get; set; }
        public string doc_status { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public Boolean CancelFlag { get; set; }
        public string remarks { get; set; }
        public string DocumentDate { get; set; }
        public string ReceivedBy { get; set; }
        public string ListFilterData1 { get; set; }
        public List<EntityNameList1> EntityNameList { get; set; }
        public List<SrcDocNoList> docNoLists { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string StatusCode { get; set; }
        public string Guid { get; set; }
    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class EntityNameList1
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
    public class UrlModel
    {
        public string Msg { get; set; }
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string GP_no { get; set; }
        public string GP_dt { get; set; }
        public string DMS { get; set; }

    }
    public class GatePassattchment
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }

}
