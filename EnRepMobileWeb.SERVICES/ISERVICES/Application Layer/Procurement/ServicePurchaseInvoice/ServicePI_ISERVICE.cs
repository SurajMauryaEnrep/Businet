using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServicePurchaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServicePurchaseInvoice
{
   public interface ServicePI_ISERVICE
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetServiceVerifcationList(string Supp_id, string CompId, string BrID);
        DataSet GetServiceVerifcationDetail(string VerNo, string VerDate, string CompId, string BrID);
        DataSet CheckSPIDetail(string CompId, string BrchID, string DocNo, string DocDate);
        string InsertSPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            ,DataTable CRCostCenterDetails, DataTable DtblTdsDetail, DataTable DtblOcTdsDetail, string Tds_amt, string Bp_Narr, string Dn_Narr);
        DataSet EditSPIDetail(string CompId, string BrID, string PINo, string PIDate, string Type, string UserID, string DocID);       
        DataSet GetSPIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date);      
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetSPIList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetServicePurchaseInvoiceDetail(string CompId, string BrID,string Voutype, string InvNo, string InvDate, string UserID, string DocID);
        string ServicePIDelete(ServicePIModel _ServicePIModel, string CompID, string br_id, string DocumentMenuId);
        string ApproveSPI(string Inv_No, string Inv_Date, string MenuDocId, string Branch
            , string CompID, string UserID, string mac_id, string wf_status, string wf_level
            , string wf_remarks,string VoucherNarr,string Bp_Nurr,string Dn_Nurr);
        DataSet GetServicePurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet CheckDuplicateBillNo(string CompId, string BrID, string supp_id, string Bill_no,string doc_id,string bill_dt);
    }
}
