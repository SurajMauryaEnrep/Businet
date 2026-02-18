using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra
{
   public interface ContraVoucher_IService
    {
        Dictionary<string, string> AutoGetBankAccList(string CompID, string AccName, string BrchID);
        DataTable GetCurrList(string CompID);
        DataSet GetAccCLBal(string acc_id, string CompId, string Br_ID, string Date);  
        DataSet GetContraVoucherListAll(string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetContraVoucherDetail(string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string ContraVoucherDelete(ContraVoucherModel _ContraVoucherModel, string CompID, string br_id, string DocumentMenuId);
        string ContraVoucherApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet ContraVoucherCancel(ContraVoucherModel _ContraVoucherModel, string CompID, string br_id, string mac_id);
        String InsertContraDetail(DataTable ContraHeader, DataTable ContraGLDetails,     DataTable ContraAttachments);


    }
}
