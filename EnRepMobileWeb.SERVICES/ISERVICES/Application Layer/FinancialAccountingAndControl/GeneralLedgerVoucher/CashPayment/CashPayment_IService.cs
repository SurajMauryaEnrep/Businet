using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment
{
   public interface CashPayment_IService
    {
        Dictionary<string, string> AutoGetCashAccList(string CompID, string AccName, string BrchID);
        DataTable GetCurrList(string CompID);
        DataSet GetAccCurrOnChange(string acc_id, string CompId,string Br_ID, string Date);
        DataSet GetCashAccIDDetail(string CompID, string BrchID, string CashAccID);
        String InsertVoucherDetail(DataTable VoucherHeader, DataTable VoucherItemDetails, DataTable CPAttachments, DataTable VoucherBillAdjDetail,DataTable CRCostCenterDetails);
        DataSet GetVoucherListAll(string SrcType, string CashId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetVoucherDetail(string VouType,string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string CPDelete(CashPayment_Model _CashPayment_Model, string CompID, string br_id, string DocumentMenuId);
        string CashPaymentApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet CashPaymentCancel(CashPayment_Model _CashPayment_Model, string CompID, string br_id, string mac_id);
        DataSet CheckAdvancePayment(string CompId, string BrchID, string DocNo, string DocDate);

    }
}
