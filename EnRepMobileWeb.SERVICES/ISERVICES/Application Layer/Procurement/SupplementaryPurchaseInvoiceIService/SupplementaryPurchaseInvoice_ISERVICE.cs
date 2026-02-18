using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplementaryPurchaseInvoiceIService
{
    public interface SupplementaryPurchaseInvoice_ISERVICE
    {
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetDPIDetailDAL(string CompId, string BrID, string Inv_no, string Inv_dt, string UserID, string DocID);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType);
        Dictionary<string, string> GetPurchaseInvoicesList1(string CompID, string BranchID, string Suppid,  string InvoiceType);
        DataSet GetPurchaseInvoicesList(string CompID, string BranchID, string Suppid, string InvoiceType);
        DataSet GetPurchaseInvoicesDetailsList(string CompID, string BranchID, string InvNo, string InvoiceType);
        DataSet GetPurchaseInvoicesItemDetailsList(string CompID, string BranchID, string InvNo, string InvoiceType);
        DataSet DPI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt);
        string InsertDPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblSubItem, DataTable DTTaxDetail, DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblTdsDetail, DataTable DtblOcTdsDetail, DataTable DtblVouDetail, DataTable DtblAttchDetail, DataTable CRCostCenterDetails, string Nurr, string TDS_Amount);
        string DeleteDetails(string CompID, string BrchID, string Inv_no, string Inv_dt);
        string ApproveDPIDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr, string Bp_Nurr, string Dn_Nurr, string Cn_Nurr);
        DataSet GetDirectPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet CheckSuppPIDetail(string CompId, string BrchID, string DocNo, string DocDate);

        DataSet GetVarienceDetails(string CompId, string BrchID, string GRNNo, string GRNDate, string ItmCode);
        DataSet PI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag);
        string Approve_SuppPI(string Inv_No, string Inv_Date, string Inv_Type, string MenuDocId, string Branch, string CompID
            , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr
            , string Bp_Nurr, string Dn_Nurr);
        DataSet CheckPIDetail(string CompId, string BrchID, string DocNo, string DocDate);
    }

    public interface SupplementaryPurchaseInvoiceList_ISERVICE
    {
        DataSet GetStatusList(string MenuID);
        DataSet GetSPI_DetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
    }
}
