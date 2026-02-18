using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DirectPurchaseInvoice
{
    public interface DirectPurchaseInvoice_ISERVICE
    {
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetDPIDetailDAL(string CompId, string BrID, string Inv_no, string Inv_dt, string UserID, string DocID);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType);
        DataSet DPI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt);
        string InsertDPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail,DataTable DtblSubItem,DataTable BatchItemTableData,DataTable SerialItemTableData, DataTable DTTaxDetail
    , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblTdsDetail, DataTable DtblOcTdsDetail, DataTable DtblVouDetail, DataTable DtblAttchDetail
    , DataTable CRCostCenterDetails, string Nurr, string TDS_Amount);
        string DeleteDPIDetails(string CompID, string BrchID, string Inv_no, string Inv_dt);
        string ApproveDPIDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
    , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr
    , string Bp_Nurr, string Dn_Nurr);
        DataSet GetDirectPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet CheckDPIDetail(string CompId, string BrchID, string DocNo, string DocDate);
    }
}
