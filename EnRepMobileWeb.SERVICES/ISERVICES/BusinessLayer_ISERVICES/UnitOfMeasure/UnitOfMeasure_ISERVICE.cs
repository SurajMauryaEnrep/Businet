using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.UnitOfMeasure
{
    public interface UnitOfMeasure_ISERVICE
    {
        DataSet GetUOMTable(string comp_id,string br_id);
        string SaveUOMData(string TransType, string comp_id, string user_id, string mac_id,string uom_id, string uom_name, string uom_alias,string item_id,string Conv_uom_id,string conv_rate,string ShowStock);
        string DeleteUOM(string TransType, string uom_id, string comp_id,string Conv_item_id,string conv_uom_id);
        DataTable GetItemNameLists(string CompId);
        DataSet GetAllDataDropDown(string CompId,string br_id);
        DataTable GetUomNameLists(string CompId,string br_id, string searchValue=null);
    }
}
