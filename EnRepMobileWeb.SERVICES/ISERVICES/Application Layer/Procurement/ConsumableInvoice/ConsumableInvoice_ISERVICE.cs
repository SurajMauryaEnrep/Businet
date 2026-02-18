using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ConsumableInvoice;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ConsumableInvoice
{
    public interface ConsumableInvoice_ISERVICE
    {
        DataTable Getsuppplier(string CompID, string GroupName, string type, string BrchID);
        DataSet CheckCIDetail(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetAllData(string CompID, string GroupName, string type, string BrchID, string suppID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        string InsertCI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DTOCDetail, DataTable DTAttachmentDetail, DataTable DtblVouDetail,DataTable DtblOCTaxDetail, DataTable CRCostCenterDetails, string Nurr,DataTable DtblTdsDetail, string tds_amt, DataTable DtblOcTdsDetail);
        DataSet GetCIDetailList(string CompId, string BrchID, string suppID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet Edit_CIDetail(string CompId, string BrID,string VouType, string inv_no, string inv_dt, string UserID, string DocID);
        string Delete_CI_Detail(ConsumableInvoiceDetails_Model _ConsumableInvoiceDetails, string CompId, string BrID);
        string Approve_CI(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
            , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks
            , string VoucherNarr, string curr_id, string conv_rate,string Bp_Nurr,string Dn_Narr);
        DataSet GetSourceDocList(string Comp_ID, string BrchID, string SuppID);
        DataSet getdataPOitemtabledata(string CompID, string  BrchID, string srdocNo, string srcdoc_dt, string suppid);
        DataSet GetDataBillNoBillDate(string CompID, string  BrchID, string srdocNo, string srcdoc_dt, string suppid);
        DataSet GetConsumbleInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
    }
}
