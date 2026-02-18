using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockMovement
{
    public interface StockMovement_ISERVICE
    {
        DataSet BindAllDropdownLists(string CompID);
        DataSet GetBatch_Lists(string CompID, string ItemId);
        DataTable GetMovementList(string CompID, string BrID, string MoveType, string ItemId, string BatchNo, string Serial_no, string Fromdt, string Todt);
    }
}
