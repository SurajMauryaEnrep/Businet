using System;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxStructure
{
    public interface TaxStructure_ISERVICE
    {
        DataTable GetTaxStructureDetail(string CompID);
        DataSet GetTaxStructureViewDetail(string TaxCode, string CompID);
        DataTable GetTaxPercDetail(string CompID);
        DataSet GetAllData(string CompID,string Flag, string TaxCode);
        DataTable GetTaxListDAL(string CompID);
        String InsertGstTaxStructureDetail(DataTable TaxStructureDt);
        string DeleteTaxDetail(string tax_perc, string CompID);


    }
}
