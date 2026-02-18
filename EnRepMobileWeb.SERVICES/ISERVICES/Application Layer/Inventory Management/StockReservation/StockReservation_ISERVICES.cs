using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockReservation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockReservation
{
   public interface StockReservation_ISERVICES
    {
        DataSet BindItemName(string CompID, string BrID, string ItmName);
        //DataTable GetWhList(string CompId, string Tobranch);
        //DataTable GetCustNameList(string CompId, string br_id, string CustomerName);
        DataSet GetWHAndCustNameAndStkRsrvList(string CompId, string br_id, string CustomerName);
        //Dictionary<string, string> GetCustomerList(string CompID, string EntityName, string BranchID);
        DataSet GetDocumentNo(string CompID, string BrchID, string Entity_id, string Itm_ID, string wh_id,string Type);
        DataSet GetStock(string CompID, string BrchID, string item_id, string wh_id);
        DataSet GetBatchSerialDetail(string CompID, string BrchID, string item_id, string wh_id, string lot_id);
        DataSet GetBatchSerialAvalStock(string CompID, string BrchID, string item_id, string wh_id, string lot_id, string BatchNo);
        DataSet GetDocdetail(string CompID, string BrchID, string WhID, string item_id, string Docno);
        string InsertStockReserve(DataTable StockReserve,DataTable dtSubItem);
        DataTable GetStockReservationList(string CompID, string BrchID);
        DataSet GetReservedItemDetail(string CompID, string BrchID, string ItemID, string wh_id, string flag, string entity_id, string docno);
        DataSet StockRes_GetSubItemDetails(string compID, string brchID, string item_id, string wh_id, string doc_type, string cust_id
            , string doc_no, string doc_dt, string flag, string transType);
        DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string uomId, string flag);
    }
}
