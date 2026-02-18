using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemAttributeSetup
{
    public interface ItemAttributeSetup_ISERVICES
    {
        DataSet getAttrName(string CompID);
        string DeleteAttr(string TransType,string attr_id,string CompID);

        string insertAttributeName(string CompID,string attr_id, string attrName, string TransType);
        string insertAttributeVal(string CompID, string attr_id, string attr_val_Id, string attr_val_Name, string TransType);
    }
}
