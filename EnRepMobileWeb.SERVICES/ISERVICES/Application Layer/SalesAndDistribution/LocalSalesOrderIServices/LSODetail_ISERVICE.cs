using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesAndDistributionModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices
{
    public interface LSODetail_ISERVICE
    {
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID,string CustType,string DocId);
        DataTable GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID);
        DataSet GetSOItmListDL(string CompID, string BrID,string ItmName);
        DataTable GetCurrList();
        DataSet GetAllData(string CompID, string SPersonName, string BranchID,string UserID,string SO_no,string SO_dt);
        DataSet GetSOItemDetailDL(string ItemID, string CompId);
        DataSet GetSOItemUOMDL(string Item_id, string CompId);
        DataSet GetSOItemAvlStock( string CompId, string BrID, string Item_id);
        DataSet GetPriceListRate(string CompId, string BrID, string Item_id, string PPolicy, string PGroup, string Cust_id, string OrdDate);
        DataSet getremarks(string CompId, string Item_id, string Cust_id);
        DataSet GetCustAddrDetailDL(string Cust_id, string CompId);
        DataSet GetConvRateDetail(string Curr_id, string CompId);
        DataSet GetSOTaxListDAL(string CompId, string BrchID, string type);
        DataSet GetOtherChargeDAL(string CompId, string BrchID);
        DataSet GetSOTaxPercentageDAL(string CompId, string BrchID, string TaxID);
        string SO_InsertRollback(string CompID, string BrID, string SONo, string SODate);
        //string SO_InsertAttachmentDetails(string CompID, string BrID, string OrderType, string SONo, string SODate, string FileName);
        string InsertLSOApproveDetails(string SONo, string SODate, string Branch, string MenuID, string CompID, string ApproveID, string mac_id,string  status,string  clevel,string  remarks);
        DataSet GetSODetailDL(string CompId, string BrID, string SONo, string SODate,string User_ID,string DocumentMenuId);
        DataSet CheckLSOQty_ForceClosed(string CompId, string BrchID, string DocNo, string DocDate);
        string FinalInsertLSO_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCTaxDetail, DataTable DTOCDetail, DataTable DTDeliSchDetail, DataTable DTTermsDetail, DataTable DtblAttchDetail,DataTable dtSubItem);
        DataSet CheckPakingListLSO(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetQuotationNumberList(string Cust_id, string CompId, string BrID);
        DataSet GetQuotDetail(string QuotNo, string CompId, string BrID);
        DataSet GetQTDetail(string CompId, string BrID, string QTNo, string QTDate,string so_no,string DocumentMenuId);
        Dictionary<string, string> GetCountryList(string CountryName);
        DataSet GetLSOAttatchDetailEdit(string CompID, string BrchID, string LSO_No, string LSO_Date);
        DataSet GetSalesOrderDeatilsForPrint(string CompID, string BrchID, string LSO_No, string LSO_Date);
        DataSet GetSubItemDetailsBySQ(string CompID, string Br_id, string QTNo, string ItemId);

        DataSet SO_GetSubItemDetails(string CompID, string Br_id, string ItemId, string QtNo, string doc_no, string doc_dt, string Flag);
        DataSet GetIntimationDetail(string CompID, string br_id, string so_no, string so_date,string SoType);
        DataSet getReplicateWith(string comp_id, string br_id, string OrderType,string SarchValue);
        DataSet GetReplicateWithItemdata(string comp_id, string br_id, string so_no,string so_dt,string ReplicateType, string cust_id, string order_dt);
        DataTable GetItemsToExportExcel(string compId, string brId, string soNo, string soDate);
        DataSet GetPriceListDetails(string CompID, string Br_id, string cust_id, string item_id, string OrdDate);
    }
}
