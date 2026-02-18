using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation
{
   public interface DomesticSalesQuotation_ISERVICES
    {
        DataSet Edit_SQDetail(string SQNo, string CompId, string BrID, string DocumentMenuID, string UserID,string flag, string rev_no);
        DataTable GetTaxTypeList(string CompID, string BranchID);
        string InsertSQ_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DTOCTaxDetail, DataTable DTTermsDetail, DataTable dtSubItem, DataTable DtblAttchDetail);
        string QTApproveDetails(string CompID, string BrchID, string QTNo, string QTDate, string UserID, string MenuID, string mac_id,  string A_Status, string A_Level, string A_Remarks);
        DataSet CheckSODetail(string CompId, string BrchID, string DocNo, string DocDate);
        //string QTdetailDelete(string CompID, string BrID, string QTNo, string QTDate);
        string QTdetailDelete(DomesticSalesQuotationModel _DomesticSalesQuotationModel, string CompId, string BrID);
        DataTable GetCustAddressdetail(string CustID, string CompId, string CustPros_type, string BranchID);
        Dictionary<string, string> GetCustomerList(string CompID, string CustomerName, string BranchID, string CustProstype, string Cust_type);
        DataSet GetAllData(string CompID, string CustomerName, string BranchID,string UserID, string CustProstype, string Cust_type, string SPersonName,string SQ_no,string SQ_dt);
        DataSet GetCustAddrDetailDL(string Cust_id, string CompId, string BranchID, string CustPros_type);
        DataSet GetItemCustomerInfo(string ItemID, string CustID, string CompId);
        DataSet GetPriceListRate(string CompId, string BrID, string Item_id, string PPolicy, string PGroup, string Cust_id);
        DataSet GetQTAttatchDetailEdit(string CompID, string BrchID, string QT_No);
        DataSet GetSalesQuotationDeatils(string CompID, string BrchID, string OrderNo, string OrderDate);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID);
        DataSet SQ_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetSlsQtGstDtlForPrint(string CompID, string BrchID, string SQtNo, string SQtDt, string Qt_type);
    }
    
}
