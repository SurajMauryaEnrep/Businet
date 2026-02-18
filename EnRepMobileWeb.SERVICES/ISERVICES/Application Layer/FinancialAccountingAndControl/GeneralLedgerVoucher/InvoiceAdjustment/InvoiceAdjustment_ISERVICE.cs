using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment
{
     public interface InvoiceAdjustment_ISERVICE
    {
        DataTable GetEntity(string CompID, string BrchID, string entity_type);
        DataSet GetAdv_Inv_Details(string comp_id, string br_id, string entity_id, string entity_type);
        String InsertInvoiceAdjustmentDetail(DataTable InvoiceAdjustmentHeader, DataTable InvoiceAdjustmentAdvDetails, DataTable InvoiceAdjustmentBillsDetail);
        DataSet GetInvoiceAdjListAll(string EntityType, string EntityId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetInvoiceAdjustmentDetail(string VouNo, string Voudt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        string InvAdjDelete(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string CompID, string br_id, string DocumentMenuId);
        string InvoiceAdjustmentApprove(string VouNo, string VouDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet InvoiceAdjustmentCancel(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string CompID, string br_id, string mac_id);
    }

}
