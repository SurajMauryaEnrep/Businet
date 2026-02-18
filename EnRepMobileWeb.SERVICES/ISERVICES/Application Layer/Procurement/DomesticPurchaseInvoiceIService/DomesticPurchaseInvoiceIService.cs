using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DomesticPurchaseInvoiceIService
{
    public interface DomesticPurchaseInvoiceIService
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType);
        DataSet GetGoodReceiptNoteList(string Supp_id, string CompId, string BrID);
        DataSet GetGoodReceiptNoteDetail(string GRNNo, string GRNDate, string CompId, string BrID,string supp_id);
        string InsertPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            ,DataTable CRCostCenterDetails, DataTable DtblSubItemDetail,string Nurr, DataTable DtblTdsDetail
            , DataTable DtblOcTdsDetail,string TDS_Amount, DataTable DtblVarDetail, DataTable DtblVarTaxDetail);
        string Delete_PI_Detail(PI_ListModel _PI_ListModel, string InvType, string CompId, string BrID);
        string Approve_PI(string Inv_No, string Inv_Date, string Inv_Type, string MenuDocId, string Branch, string CompID
            , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr
            ,string Bp_Nurr,string Dn_Nurr, string DN_VarianceNarr);
        DataSet Edit_PIDetail(string CompId, string BrID,string VouType, string PINo, string PIDate, string Type, string UserID, string DocID);
        DataSet CheckPIDetail(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetVarienceDetails(string CompId, string BrchID, string GRNNo, string GRNDate, string ItmCode, string flag = "");
        DataSet GetPIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date);
        DataSet GetPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string PI_No, string PI_Date);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetPITaxListDAL(string CompId, string BrchID);
        DataSet PI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag);
        DataSet getEinvoiceno_ewbNo(string CompID, string BrchID, string grnno, string GRN_Date);
        DataTable CheckRoundOffAcc(string CompId, string BrID);
        DataSet GetPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type);
    }
    public interface DomesticPurchaseInvoiceListIService
    {
        DataSet GetStatusList(string MenuID);
        DataSet GetPI_DetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
    }
    }
