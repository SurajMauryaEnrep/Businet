using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorSetup
{
    //class ShopfloorSetup_ISERVICES
    //{
    //}

    public interface ShopfloorSetup_ISERVICES
    {
        DataTable GetShopFloorDetailsDAL(Int32 comp_id,Int32 br_id);
        DataSet GetShopFloorDetailsBranchWiseDAL(Int32 comp_id, Int32 br_id, Int32 shfl_id);
        //string insertShopfloorDetail(int comp_id, int op_id,int shfl_id, string op_name, string op_type, string op_remarks, int create_id, string action);
        string insertShopfloorDetail(DataTable ShopFloorDetail,DataTable ProductionCapacity, DataTable Attachments, string mac_id);
        DataSet GetSOItmListDL(string CompID, string BrID, string ItmName);// Binding for Item Name
        DataSet GetSOItemUOMDL(string Item_id, string CompId);// Binding for UON
    }
}
