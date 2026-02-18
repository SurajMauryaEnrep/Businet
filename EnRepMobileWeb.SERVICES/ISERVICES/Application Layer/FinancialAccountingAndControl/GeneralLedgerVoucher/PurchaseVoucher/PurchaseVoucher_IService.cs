using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.PurchaseVoucher;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.PurchaseVoucher
{
    public interface PurchaseVoucher_IService
    {
        Dictionary<string, string> AutoGetSuppAccList(string CompID, string AccName, string BrchID);
        String InsertPurchaseVoucherDetail(DataTable PurchaseVoucherHeader, DataTable PurchaseVoucherGLDetails, DataTable PVAttachments, DataTable CRCostCenterDetails);
        DataTable GetCurrList(string CompID);
        DataSet GetAccCurrOnChange(string acc_id, string CompId, string br_id, string date);
        DataSet GetSuppAccIDDetail(string CompID, string BrchID, string SuppAccID, string VouDate);
        DataSet GetPurchaseVoucherListAll(int curr,string SuppId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string VouType, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetPurchaseVoucherDetail(string VouNo, string Voudt,string VouType, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string PVDelete(PurchaseVoucher_Model _PurchaseVoucher_Model, string CompID, string br_id, string DocumentMenuId);
        string PurchaseVoucherApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet PurchaseVoucherCancel(PurchaseVoucher_Model _PurchaseVoucher_Model, string CompID, string br_id, string mac_id);
        DataSet GetBillDetail(string CompID, string BrchID, string AccId, string fromdt, string todt, string flag, string DocumentNumber, string Status);
        DataSet CheckPVDetail(string CompId, string BrchID, string DocNo, string DocDate);
    }
}
