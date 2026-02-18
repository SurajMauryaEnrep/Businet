using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISFinishedGoodsReceipt
{
    public class MISFinishedGoodsReceipt_Model
    {
        public List<ShopfloorListDropDown> ShopfloorList { get; set; }
        public int length { get; set; }
        public string DocumentMenuId { get; set; }
        public string ShowAs { get; set; }
        public string shopfloor_id { get; set; }
        public string Shopfloor { get; set; }
        public string MultiselectItemNameHdn { get; set; }
        public string SearchParaMeter { get; set; }
        public string ToDate { get; set; }
        public string Fromdate { get; set; }
        public string MultiselectStatusHdn { get; set; }
        public string Status { get; set; }
        public string GroupName { get; set; }
        public string Title { get; set; }
        public string GroupID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public List<ItemName> ItemNameList { get; set; }
        public List<ItemGroupName> ItemGroupNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MISFGRTableList> MISFGR { get; set; }
    }
    public class MISFGRTableList
    {
        public string SrNo1 { get; set; }
        public int SrNo { get; set; }
        public string item_name { get; set; }
        public string item_id { get; set; }
        public string uom_alias { get; set; }
        public string out_qty { get; set; }
        public string Qty_in_Sp { get; set; }
        public string shfl_name { get; set; }
        public string shfl_id { get; set; }
        public string rcpt_no { get; set; }
        public string rcpt_dt { get; set; }
        public string batch_no { get; set; }
        public string lot_no { get; set; }
        public string exp_dt { get; set; }
        public string cost_price { get; set; }
        public string total_value { get; set; }
        public string Doc_no { get; set; }
        public string Doc_dt { get; set; }
        public string input_item_name { get; set; }
        public string input_item_id { get; set; }
        public string input_uom_name { get; set; }
        public string input_uom_id { get; set; }
        public string Batchable { get; set; }
        public string cons_qty { get; set; }
        public string SNO_rcptno { get; set; }
        public string Count_rcpt_no { get; set; }
        public string SNO_INPUT { get; set; }
        public string count_input { get; set; }
        public string SNO_OUTPUT { get; set; }
        public string rowspanOutputItem { get; set; }
        public string rowspanInputItem { get; set; }
        public string rowspanrcptno { get; set; }
        public string count_output { get; set; }
        public string AbcNull { get; set; }

    }
    public class ShopfloorListDropDown
    {
        public string shop_id { get; set; }
        public string shop_name { get; set; }
    }
    public class ItemName
    {
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
    }
    public class ItemGroupName
    {
        public string Group_Id { get; set; }
        public string Group_Name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

}
