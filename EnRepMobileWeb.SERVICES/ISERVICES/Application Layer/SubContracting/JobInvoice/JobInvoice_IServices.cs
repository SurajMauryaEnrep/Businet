using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobInvoice
{
    public interface JobInvoice_IServices
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId);
        DataSet GetGoodReceiptNoteList(string Supp_id, string CompId, string BrID,string DocumentNumber);
        DataSet GetJOandGoodReceiptNoteSCDetails(string GRNNo, string GRNDate, string CompId, string BrID);
        DataSet GetAllGLDetails(DataTable GLDetail);
        string InsertJI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails, DataTable DtblTdsDetail, DataTable DtblOcTdsDetail, string Tds_amt);
        DataSet GetJoInvListandSrchDetail(string CompId, string BrchID, JI_ListModel _JI_ListModel, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetJobInvDetailEditUpdate(string CompId, string BrchID, string JISC_NO, string JISC_Date, string UserID, string DocID,string VouType);
        string JInv_DeleteDetail(JobInvoiceModel _JobInvoiceModel, string CompId, string BrID);
        string JIApproveDetails(string CompID, string BrchID, string JI_No, string JI_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks, string VoucherNarr,string BP_Nurration, string DN_Nurration);
        DataSet JobInvCancel(JobInvoiceModel _JobInvoiceModel, string CompID, string br_id, string mac_id,string Nurr);
        DataSet GetJobInvoiceDeatilsForPrint(string CompID, string BrchID, string invNo, string invDt);
        DataSet CheckJIDetail(string CompId, string BrchID, string DocNo, string DocDate);

    }

}
