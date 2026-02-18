using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment
{
   public interface BankPayment_IService
    {
        Dictionary<string, string> AutoGetBankAccList(string CompID, string AccName, string BrchID);
        String InsertBankPaymentDetail(DataTable BankPaymentHeader, DataTable BankPaymentGLDetails, DataTable BankPaymentBillAdjDetail, DataTable BPAttachments, DataTable CRCostCenterDetails, string PDC, string InterBrch);
        DataTable GetCurrList(string CompID);
        DataTable GetGLVoucherDtForPDC(string CompID, string BrID);/*Add By Hina on 08-08-2024 for pdc*/
        DataTable GetBranchList(string CompID);//Added by Suraj on 13-08-2024 for getting Branch List.
        DataSet GetAccCurrOnChange(string acc_id, string CompId,string Br_ID,string Date);
        DataSet GetBankAccIDDetail(string CompID, string BrchID, string BankAccID);
        DataSet GetBankPaymentListAll(string SrcType, string BankId, string Fromdate, string Todate, string Status, string CompID
            , string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId, string Currency
            , string InsType, string RecoStatus, string skip = "0", string pageSize = "25", string searchValue = ""
            , string sortColumn = "SrNo", string sortColumnDir = "ASC", string Flag = "");
        DataSet GetBankPaymentDetail(string VouType, string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string BPDelete(BankPayment_Model _BankPayment_Model, string CompID, string br_id, string DocumentMenuId);
        string BankPaymentApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level
            , string wf_remarks, string comp_id, string br_id, string mac_id, string DocID,string int_br_nurr);
        DataSet BankPaymentCancel(BankPayment_Model _BankPayment_Model, string CompID, string br_id, string mac_id);
        DataSet GetBillDetail(string CompID, string BrchID, string AccId, string fromdt, string todt, string flag, string DocumentNumber, string Status,string Curr);
        DataSet CheckAdvancePayment(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet SearchAdjustedAmountDetail(string compId, string brId, string InVNo, string InvDate, string Accid, string VouNo, int AccTyp);
    }
}
