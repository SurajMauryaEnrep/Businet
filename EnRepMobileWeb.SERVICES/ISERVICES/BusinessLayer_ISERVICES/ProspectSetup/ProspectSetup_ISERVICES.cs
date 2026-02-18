using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.ProspectSetup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ProspectSetup
{
    public interface ProspectSetup_ISERVICES
    {
        DataSet GetProspectDetailsList(string pros_id, string CompID, string Br_ID);
        Dictionary<string, string> GetCityList(string GroupName);
        DataTable GetCurronProspectTypeDAL(string CompID, string prosType);
        string DeleteProsId(ProspectSetupMODEL prospectSetupMODEL,string command,string Comp_ID,string Br_ID);
        DataSet Getviewprospectdetail(string Custcode, string CompID, string Br_ID);
        string insertProspectDetails(DataTable ProspectDetail, DataTable Attachments);
        DataTable BranchList(string CompId);
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        DataTable GetCountryListDDL(string CompID, string ProspectType);
        DataTable GetstateOnCountryDDL(string ddlCountryID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
        DataSet GetStateCode(string stateId);
        /*----------------------Code End of Country,state,district,city--------------------------*/
    }
}
