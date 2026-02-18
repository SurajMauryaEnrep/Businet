using EnRepMobileWeb.MODELS.BusinessLayer.TaxSlab;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxSlab
{
    public interface TaxSlab_ISERVICE
    {
        DataSet GetTaxSlabDetail(string CompID);
        string InsertTaxSlabDetail(string CompID, string taxper, string goods,string services, string transtype);
        string DeleteTaxSlab( string CompID, string tex,string texHsn, string transType);
        DataSet GetModulAndHsnDetails(string CompID);
        DataSet GetDataDropDownList(string CompID);
        DataSet getAttrName(string Comp_ID);
        string InsertHsnDetails(string CompID, string listTaxPer, string HSN_Number, string transtype);
    }
}
