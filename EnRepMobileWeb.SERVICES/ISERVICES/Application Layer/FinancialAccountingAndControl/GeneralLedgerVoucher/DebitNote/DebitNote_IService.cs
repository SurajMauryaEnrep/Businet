using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote
{
    public interface DebitNote_IService
    {
        DataTable GetCurrList(string CompID);
        DataSet GetAccCurrOnChange(string acc_id, string CompId, string br_id, string date);
        Dictionary<string, string> AutoGetEntityList(string CompID, string AccName, string BrchID, string Entitytype,string flag);
        DataSet GetEntityIDDetail(string CompID, string BrchID, string EntityID,string VouDate);
        String InsertDebitNoteDetail(DataTable DebitNoteHeader, DataTable DebitNoteGLDetails, DataTable DbNoAttachments, DataTable CRCostCenterDetails, DataTable DebitNoteBillAdj,string conv_rate,string sls_per);
        DataSet GetDebitNoteListAll(string EntityType, string EntityId,string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataTable AutoGetEntityList1(string CompID, string AccName, string BrchID, string Entitytype, string flag);
        DataSet GetDebitNoteDetail(string VouNo, string Voudt,string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string DNDelete(DebitNote_Model _DebitNote_Model, string CompID, string br_id, string DocumentMenuId);
        string DebitNoteApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet CheckPaymentAgainstDebitNote(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet DebitNoteCancel(DebitNote_Model _DebitNote_Model, string CompID, string br_id, string mac_id);
        DataTable getSalesPersonList(string CompID, string br_id, string userid);
    }
}
