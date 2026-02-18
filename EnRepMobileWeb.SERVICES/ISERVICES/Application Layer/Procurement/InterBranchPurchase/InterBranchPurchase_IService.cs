using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.InterBranchPurchase
{
    public interface InterBranchPurchase_IService
    {
        DataSet GetAllData(string CompID, string CustName, string BranchID, string User_ID, string CustId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet GetBranchList(string CompId, string BrID);
        DataSet GetInterBranchSaleInvoiceDetail(string CompId, string BrID, string invno, string invdt, string BrchIDCurr,string supp_id);
        
        Dictionary<string, string> GetBillNoList(string CompId, string BrID, string Interbranch);
        DataSet GetIBPDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataTable CheckRoundOffAcc(string CompId, string BrID);
        string Insert_IBPDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable SubitemINVQty, DataTable ItemBatchDetails,
            DataTable ItemSerialDetails, DataTable DTTaxDetail, DataTable DtblIOCDetail, DataTable DtblOCTaxDetail, DataTable DtblOCTdsDetail,
            DataTable DtblVouDetail, DataTable DtblAttchDetail, DataTable CRCostCenterDetails, string Narr);

        string ApproveDPIDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
    , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr
    , string Bp_Nurr, string Dn_Nurr);

        string DeleteIBPDetail(string CompID, string BrchID, string Inv_no, string Inv_dt);
        DataSet IBP_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt);
        DataSet GetIBPInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
    }
}

