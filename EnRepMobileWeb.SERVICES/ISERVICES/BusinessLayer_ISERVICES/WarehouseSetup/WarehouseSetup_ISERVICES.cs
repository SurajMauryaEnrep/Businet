using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.WarehouseSetup
{
    public interface WarehouseSetup_ISERVICES 
    {

        //DataTable Get_CityAndPIN_list();
        DataSet GetBrList(string CompID);
        DataSet GetWarehouseDetails(string wh_type, string wh_name,string comp_id);
        string insertWarehouseDetails(DataTable WarehouseDetail,DataTable WarehouseBranch);
        string Delete_warehousedetails(string wh_id, string comp_id);
        //DataSet InsertBrDetailDAL(string Comp_ID, string Taxcode, string BrID, string Flag, string TransType);

    }

}
