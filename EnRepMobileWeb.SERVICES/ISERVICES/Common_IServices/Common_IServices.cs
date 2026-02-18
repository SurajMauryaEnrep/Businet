using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices
{
    public interface Common_IServices
    {
        Dictionary<string, string> SuppCityDAL(string GroupName);
        string GetPageNameByDocumentMenuId(string CompID, string DocumentMenuId ,string language);
        DataTable GetRole_List(string CompID, string UserID, string DocumentMenuId);
        DataSet GetApprovalLevel(string CompID, string BrID, string DocumentMenuId);
        DataSet GetCommonPageDetails(string CompID, string BrID, string UserID, string DocumentMenuId, string language);
        DataSet GetItemGstDetails(string CompID, string BrID, string ItemIDs,string gst_number);
        DataSet GetGSTApplicable(string CompID, string BrID, string param_id);
        DataSet GetBrList(string CompID);
        DataSet GetBaseCurrency(string CompID);
        DataSet GetStatusList(string MenuID);
        string InsertForwardDetails(string compid, string brid, string docid, string docno, string docdate, string status, string forwarededto, string forwardedby, string level, string remarks);
        DataSet GetWFLevel_Detail(string CompId, string BrID, string DocNo, string DocDate, string DocID, string DocStatus);
        DataSet GetItemDetailDL(string ItemID, string CompId);
        DataTable GetAddressdetail(string CustID, string CompId, string CustPros_type, string BranchID, string Add_type,string add_id);
        DataTable GetSuppAddressdetail(string SuppID, string CompId,string BranchID);
        DataSet GetItemCustomerInfo(string ItemID, string CustID, string CompId);
        DataSet GetItemSupplierInfo(string ItemID, string SuppID, string CompId);
        DataSet GetItmListDL(string CompID, string BrID, string ItmName,string PageName);
        DataSet GetItmListDL1(string CompID, string BrID, string ItmName, string PageName);
        DataSet GetAccountListDDL(string CompID, string BrID, string AccName, string DocMenuID, string gl_curr_id = "0");
        DataSet GetAccGrpDDL( string CompId, string Acc_ID);
        DataSet GetAccBalance(string CompId, string BrID, string Acc_ID, string VouDate);
        string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID,string UomId, string LotID
            , string BatchNo,string DocumentMenuId);
        DataSet Getfw_List(string CompID, string BrID, string UserID, string DocumentMenuId);
        DataSet GetfwHistory_List(string CompID, string BrID, string DocNo, string DocId);
        DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID);
        DataSet getItemstockWarehouseWise(string ItemId,string UomId, string CompId, string BranchId, string DocumentMenuId);
        DataSet getItemstockShopFloorWise(string ItemId,string UomId, string CompId, string BranchId);
        DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataTable GetItemSOHistory(string ItemID, string CustID, string FinStDt, string Date12, string CompId, string BranchID);
        DataTable GetCustomerSalesHistory(string CustID, string FinStDt, string Date12, string CompId, string BranchID);
        DataTable GetItemPOHistory(string ItemID, string SuppID, string FinStDt, string Date12, string DMenuId, string CompId, string BranchID);
        DataSet GetItemUOMDL(string Item_id, string CompId,string br_id, string ItemUomType);

        DataSet GetItemAvlStock(string CompId, string BrID, string Item_id);
        DataSet GetItemAvlStockShopfloor(string CompId, string BrID, string Item_id,string MaterialType,string SourceShopfloor);
        DataSet GetOtherChargeDAL(string CompId, string BrchID);
        DataSet Get_VoucherDetails(string compid, string brid, string vouno, string voudt, string flag, string narr);

        DataSet get_Costcenterdetails(string compid, string brid, string vouno, string voudt, string acc_id);
        DataSet GetTaxListDAL(string CompId, string BrchID);
        DataSet GetTaxPercentageDAL(string CompId, string BrchID, string TaxID);
        DataSet BindTaxSlablist(string Comp_ID,string DocumentMenuId);
        DataSet BindTaxTemplatelist(string Comp_ID,string DocumentMenuId, string Tmplt_type);
        DataSet BindTaxTemplateData(string Comp_ID, string tmplt_id,string TaxSlab, string Br_ID, string ItemId, string GSTNo);
        DataSet BindTermsTemplatelist(string Comp_ID, string DocumentMenuId);
        DataSet BindTermsTemplateData(string Comp_ID, string tmplt_id);
        DataTable GetCompLogo(string Comp_ID, string Br_id,string BrId_List=null);
        DataSet GetSubItemDetails(string CompID, string ItemId);
        DataSet GetSubItemShflAvlstockDetails(string comp_ID, string br_ID, string shfl_id, string item_id,string stkType);
        DataSet GetSubItemWIPstockDetails(string CompID, string br_ID, string shfl_id, string ItemId);
        DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id,string UomId, string flag);
        DataSet GetCstCntrData(string CompId, string BrchId, string CC_Id, string Flag);
        DataSet GetAccountDetail(string acc_id, string CompId, string BrchId, string Date);
        DataSet Cmn_GetTDSDetail(string CompId, string BrchId,string doc_id);
        DataSet Cmn_GetTDSTempltDetail(string CompId, string BrchId, string tmplt_id, string doc_id, string AmtForTds);
        void SendAlertEmail(string compId, string brId, string docId, string docNo, string status, string sentBy, string sentTo);
        void SendAlertEmail(string compId, string brId, string docId, string docNo, string status, string sentBy, string sentTo, string docPdfFilePath);
        DataSet Cmn_GetGLVoucherPrintDeatils(string CompID, string br_id, string JVNo, string JVDate, string Vou_type);
        DataSet Cmn_getAlternateItemDetalInfo(string compID, string br_ID, string product_id, string op_id, string item_type_id, string item_id);
        DataTable curFYdt(string compId, string brId);
        string curFYdtAndPreviousFYdt(string compId, string brId, string DocDate);
        DataSet Fin_curFYdt(string compId, string brId,string VouDt);
        DataTable Cmn_GetDeliverySchudule(string compId, string brId, string orderType, string orderNo, string itemId, string docId);
        DataSet Get_rcptpatmhist_details(string CompId, string BrchId, string AccId, string Flag);
        DataSet GetStockUomWise(string comp_ID,string Br_ID, string itemId, string uomId);
        DataTable Cmn_GetBrList(string comp_id, string userid);
        DataSet GetTaxTemplateByOC(string comp_ID, string oC_id);
        DataSet CheckUserRolePageAccess(string Comp_ID, string Br_ID, string UserID, string MaterPagetype);
        DataSet CmnGetOcTpEntityList(string comp_ID, string br_ID, string ocId, string currId,string DocId);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetAllGLDetails1(DataTable GLDetail,string BrId);/*Added by Suraj on 07-08-2024 to add new column acc_id*/
        DataSet Cmn_GetTdsDetails(string comp_ID, string br_ID, string SuppId, string GrossVal,string tax_type);
        DataSet Get_FYList(string Compid, string Brid);
        DataTable Cmn_GetBrList(string comp_id, string br_id, string userid, string flag);
        DataSet Cmn_GetGernalLedgerDetails(string comp_id, string br_id, string acc_id, string from_dt, string to_dt,string doc_id);
        DataSet Cmn_Get_GernalLedger_Details(string comp_id, string br_id, string acc_id, string from_dt, string to_dt, string doc_id, string curr_id);
        DataSet Cmn_GetSFBOMDetailsItemWise(string comp_ID, string br_ID, string fGItemId, string sFItemId);
        DataTable GetItemName(string CompID, string BranchId, string itemNameSearch);
        string SendAlertEmailExternal(string compId, string brId, string UserId,string docId, string docNo, string status, string mail_Id, string docPdfFilePath);
        string SendAlertEmailExternal1(string compId, string brId, string UserId, string docId, string docNo, string status, string mail_Id);
        string ViewEmailAlert(string compId, string brId,string UserId, string docId, string docNo, string status, string mail, string FilePath);
        void SendAlertlog(string compId, string brId, string Alert_type,string docNo, string docDate, string docId, string doc_status,string send_dt,char sent_status, string mail_ID,string mail_cont,string file_path);
        DataTable EmailAlertLogDetails(string compId, string brId, string docId, string docNo);
        DataTable GetSupplierEmail(string CompId, string BrID, string docid, string id,string type);//Added by Nidhi on 01-08-2025
        DataSet GetOCHSNDL(string oc_id, string CompId, string br_id, string SuppStateId);
        string CheckMailAttch(string compId, string brId, string docid, string docstatus);
        DataSet Cmn_Get_StockGlAccountList(string compId);
        DataSet Cmn_Get_ItemAliasList(string compId, string Search);
        DataSet Cmn_GetParameterValues(string comp_ID);
        DataTable GetItemStockWhLotBatchSerialWise(string compID, string branchID, string itemID);
        DataSet GetCustCommonDropdownDAL(string CompID, string SearchVal, string Stateid);
        DataSet GetVerifiedDataOfExcel(string compId, string BrchID, DataTable dts, string ItemName,string Flag);
        DataSet Cmn_getSchemeFocDetail(string compId, string brchID, string item_id, string cust_id, string order_qty, string order_value, string ord_dt);
    }
}
