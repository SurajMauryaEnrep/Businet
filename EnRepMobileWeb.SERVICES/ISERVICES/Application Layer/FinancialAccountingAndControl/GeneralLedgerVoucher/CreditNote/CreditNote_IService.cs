using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CreditNote;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CreditNote
{
    public interface CreditNote_IService
    {
        Dictionary<string, string> AutoGetEntityList(string CompID, string AccName, string BrchID, string Entitytype,string flag);
        DataTable GetCurrList(string CompID);
        DataSet GetAccCurrOnChange(string acc_id, string CompId, string br_id, string date);
        DataSet GetEntityIDDetail(string CompID, string BrchID, string EntityID,string VouDate);
        String InsertCreditNoteDetail(DataTable CreditNoteHeader, DataTable CreditNoteGLDetails
            , DataTable CrNoAttachments, DataTable CRCostCenterDetails, DataTable DebitNoteBillAdj,string conv_rate);
        DataSet GetCreditNoteListAll(string EntityType, string EntityId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataTable AutoGetEntityList1(string CompID, string AccName, string BrchID, string Entitytype, string flag);
        DataSet GetCreditNoteDetail(string VouNo, string Voudt,string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string CNDelete(CreditNote_Model _CreditNote_Model, string CompID, string br_id, string DocumentMenuId);
        string CreditNoteApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet CheckPaymentAgainstCreditNote(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet CreditNoteCancel(CreditNote_Model _CreditNote_Model, string CompID, string br_id, string mac_id);
    }
}
