using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionOrder
{
    public interface ProductionOrder_ISERVICES
    {
        string insertJCDetail(DataTable bomproductdetail, DataTable bomitemdetails, DataTable PrdOrdrAttachments, DataTable dtSubItem, string A_Status,string A_Level,string A_Remarks, DataTable ProductionSch);//Insert
        DataSet BindProductNameInDDL(string CompID, string BrID,string ItmName);//Bind product name
        DataSet GetSOItemUOMDL(string Item_id, string CompId,int br_id);// Bind UON
        DataSet BindRevnoEdit(int CompId, int br_id, string Item_id);
        // DataSet BindRevisionNoInDDL(Int32 comp_id, Int32 br_id,string product_id);
        // DataSet GetOperationNameList(int CompID, int br_id, string product_id, int rev_no);//Bind data in opetation name in ddl
        DataSet GetAllData(int CompID, int br_id, string UserID, string wfstatus, string DocumentMenuId,
            string txtFromdate, string txtToDate);

        DataTable GetShopFloorDetailsDAL(Int32 comp_id, Int32 br_id);//Bind shop floor in ddl
        DataSet GetWorkStationDAL(Int32 comp_id, Int32 br_id,int shfl_id);//Bind work station in ddl
        DataSet Bind_Plus_AddAttribute(int comp_id, int br_id, string advice_no, string advice_dt, int op_id, string Shflid, string Itemid, string ProdQty, string Product_id);
        DataSet GetJCList(Int32 CompID, Int32 BrID, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetJCSearch(string CompID, string BrID,string ddl_shfl_id, string ddl_Product_id, string ddl_op_id, string ddl_shift_id, string ddl_workst_id, string txtFromdate, string txtToDate, string Status);
        DataSet BinddbClick(Int32 CompID, Int32 BrID, string jc_no,string jc_dt, string UserID, string DocumentMenuID);
        DataSet BindOperationNameInListPage(int CompId);
        DataSet BindOPNameBaseOnRevNo(int CompId, int br_id, string Item_id, int rev_no, string ProductionOrderNumber, string Jc_Date);
        DataSet CheckPCagainstPrOrdr(string CompID, string BrID, string jc_no, string jc_dt,string prdctOrdtype);
       
        DataSet GetAdviceDetails(string CompId, string BrID, string AdviceNo, string AdviceDate);
        DataSet Get_ItemDetailsList(int comp_id, int br_id, string productid, string advice_no, string advice_dt, int op_id);
        DataSet GetConfirmationDetail(string CompID, string BrchID, string ItemID, string JCNumber, string JCDate);
        DataSet JC_GetSubItemDetails(string CompID, string br_id,string Item_id,string jc_no,string jc_dt,string Flag);
        DataSet GetProductionOrderPrintDeatils(string CompID, string br_id,string jc_no,string jc_dt);
        DataSet GetAlternateItemDetails(string compID, string br_id, string product_Id, string op_Id, string item_Id,string shfl_id, string alt_item_id);
        DataSet GetSubItemWhAvlSHOPstockDetails(string comp_ID, string br_ID, string item_id, string UomId,
            string flag1, string Doc_no, string Doc_dt);
    }
}
