using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentRequisition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentRequisition
{
    public interface PaymentRequisition_ISERVICES
    {
        DataSet GetAllData(string CompId, string br_id, string reqArea, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId,string flag);
        string InsertUpdatePR(DataTable PRHeader, DataTable Attachments);
        string DeleteData(PaymentRequisitionModel DeleteModel, string CompID, string BrchID, string DocumentMenuId);
        string CancelDocument(PaymentRequisitionModel CancelModel, string CompID, string BrchID, string mac_id);
        DataSet GetDetailData(string CompID, string BrchID, string pr_no, string pr_dt, string UserID, string DocumentMenuId);
        string DocumentApprove(PaymentRequisitionModel ApproveModel, string CompID, string br_id, string PR_Date, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID);

      
    }
}
