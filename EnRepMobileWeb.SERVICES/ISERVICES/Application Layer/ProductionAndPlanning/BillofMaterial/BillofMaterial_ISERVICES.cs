using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.BillofMaterial
{
    public interface BillofMaterial_ISERVICES
    {
        (string, string) insertBOMDetail(DataTable bomproductdetail, DataTable bomitemdetails, DataTable AlternateItemDt, string A_Status, string A_Level, string A_Remarks, string menuid);//Insert for table name ppl$bom$detail and ppl$bom$item$detail
        DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);
        Dictionary<string, string> BindProductNameInDDLList(string CompID, string BrID, string ItmName);
        DataSet GetSOItemUOMDL(string Item_id, string CompId);// Binding for UON
        DataTable GetOperationNameList(int CompID);//Bind data in opetation name in ddl
        DataSet GeItemNameList(string CompID, string BrID, string ItmType, string wip, string Pack, string ItmName, string ProductId);//Bind data in item name in ddl
        DataSet GetItemCost(Int32 CompID, Int32 BrID, string ItmId);//Binding for item cost
        DataSet HidePrNameAfterSave(Int32 CompID, Int32 BrID);//Hide product name after save
        DataTable GetBOMList(Int32 CompID, Int32 BrID,string UserID, string wfstatus, string DocumentMenuId, string Act, string Status);//Hide product name after save
        DataSet BindBOMdbEdit(Int32 CompID, Int32 BrID, string product_id,Int32 rev_no, string UserID, string DocumentMenuID);//Binding for edit BOM
        DataSet GetBillofMaterialPrintDeatils(string CompID, string Br_ID, string product_id, int rev_no);
        DataTable GetShopFloorDetailsDAL(Int32 comp_id, Int32 br_id); // Added By NItesh 26-14-2023 for bind shopfoore Dropdown list
        DataSet getrepicateitem(Int32 comp_id, Int32 br_id,string SOItmName); // Added By NItesh 26-14-2023 for bind shopfoore Dropdown list
        DataSet getreplicateitemdata(string  comp_id, string br_id,string replicate_item); // Added By NItesh 26-14-2023 for bind shopfoore Dropdown list
        DataSet getUomConvRate(string comp_ID, string br_ID, string itemId, string uomId);
    }
}
