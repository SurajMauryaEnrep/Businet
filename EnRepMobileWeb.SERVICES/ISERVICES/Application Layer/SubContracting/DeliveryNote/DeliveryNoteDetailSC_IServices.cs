using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.DeliveryNote;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.DeliveryNote
{
   public interface DeliveryNoteDetailSC_IServices
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetJobORDDocNOList(string Supp_id,string Comp_ID, string Br_ID);
        DataSet GetMDDocNOList(string Comp_ID, string Br_ID,string JONO);
        //DataSet GetReturnItemDDLList(string Comp_ID, string Br_ID, string JONO);
        DataSet getDetailBySourceDocumentMDNo(string CompID, string BrchID, string SourDocumentNo, string SourDocumentDate, string DNNo);
        string InsertDN_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblItemReturnDetail, DataTable DispatchQtyItemDetails, DataTable DTAttachmentDetail, DataTable dtSubItem, DataTable dtByPrdctScrapSubItem);
        DataSet GetDNSCListandSrchDetail(string CompId, string BrchID, DNListModel _DNListModel, string UserID, string wfstatus, string DocumentMenuId);

        DataSet GetDNDetailEditUpdate(string CompId, string BrchID, string DNSC_NO, string DNSC_Date, string UserID, string DocID);
        string DN_DeleteDetail(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string CompId, string BrID);
        string DNApproveDetails(string CompID, string BrchID, string DN_No, string DN_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet DNCancel(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string CompID, string br_id, string mac_id);
        DataSet GetReturnItemDDLList(string CompID, string BrID, string ItmName, string PageName, string JONO);
        DataSet BindByProdctScrapItm_AgainstDircetJO(string CompID, string BrID, string ItmName, string PageName/*, string JONO*/);
        DataSet ChkGRNSCDagainstDNSC(string CompID, string BrID, string DNNo, string DNDate);
        DataSet CheckQCAgainstDNSC(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetSubItemDetailsFromMD(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag, string JobOrdNo, string MDNo, string MDDate);
        DataSet DNSC_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string MDNo, string doc_no, string doc_dt, string Flag);
        DataSet GetDeliveryNoteSCDeatilsForPrint(string CompID, string BrchID, string dn_no, string dn_dt);


    }
}

