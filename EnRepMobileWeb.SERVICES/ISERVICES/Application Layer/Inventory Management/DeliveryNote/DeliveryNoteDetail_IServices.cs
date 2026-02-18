using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;
namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management
{
    public interface DeliveryNoteDetail_IServices
    {
        DataTable GetSupplierListALl(string CompID, string SuppName, string BrchID);

      
        DataSet getDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo, string SourDocumentDate, string Item_id);

        String insertDeliveryNoteDetails(DataTable DeliveryNoteHeader, DataTable DeliveryNoteItemDetails, DataTable Attachments,DataTable dtSubItem);
        DataSet checkDependency(string CompID, string BrchID, string dn_no, string dn_dt);

        DataSet GetDeliveryNoteDetailByNo(string CompID, string dn_no, string BrchID, string UserID, string DocumentMenuId);

        DataSet DeliveryNoteDelete(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS ,string comp_id, string br_id,string DeliveryNoteNo);
        DataSet GetDeliveryNoteDeatilsForPrint(string CompID, string BrchID, string dn_no, string dn_dt);

        String DeliveryNoteApprove(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string comp_id, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId);

        Dictionary<string, string> AutoGetSupplierListALl(string CompID, string SuppName, string BrchID);

        DataSet getDeliveryNoteSourceDocumentNo(string CompID, string BrchID, string SupplierId, string DocumentNumber,string Item_id, string Dn_type);


        String DeliveryNoteCancel(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS,string comp_id, string userid, string br_id, string mac_id);

        string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix);
        DataSet GetSubItemDetailsFromPO(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag,string Srcdoc_no);
        DataSet DN_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag,string Srcdoc_no);

    }
}
