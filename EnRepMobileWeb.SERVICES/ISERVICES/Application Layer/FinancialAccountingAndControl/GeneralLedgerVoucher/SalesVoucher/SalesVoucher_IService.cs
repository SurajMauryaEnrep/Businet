using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher
{
    public interface SalesVoucher_IService
    {
        Dictionary<string, string> AutoGetCustAccList(string CompID, string AccName, string BrchID);
        String InsertSalesVoucherDetail(DataTable SalesVoucherHeader, DataTable SalesVoucherGLDetails, DataTable SVAttachments,DataTable CRCostCenterDetails,string sls_per);
        DataTable GetCurrList(string CompID);
        DataSet GetAccCurrOnChange(string acc_id, string CompId, string br_id, string date);
        DataSet GetCustAccIDDetail(string CompID, string BrchID, string CustAccID, string VouDate);
        DataSet GetSalesVoucherListAll(int curr,string CustId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetSalesVoucherDetail(string VouNo, string Voudt, string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string SVDelete(SalesVoucher_Model _SalesVoucher_Model, string CompID, string br_id, string DocumentMenuId);
        string SalesVoucherApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet SalesVoucherCancel(SalesVoucher_Model _SalesVoucher_Model, string CompID, string br_id, string mac_id);
        DataSet CheckSVDetail(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetGLVoucherPrintDeatils(string CompID, string br_id, string SVNo, string SVDate, string Vou_type);
        DataTable getSalesPersonList(string CompID, string br_id, string userid);
    }
}
