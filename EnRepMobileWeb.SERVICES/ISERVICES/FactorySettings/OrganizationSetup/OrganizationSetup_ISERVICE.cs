using EnRepMobileWeb.MODELS.Factory_Settings.Organization_Setup;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings_ISERVICE.OrganizationSetup_ISERVICE
{
    public interface OrganizationSetup_ISERVICE
    {
        DataSet BindLAng();
        DataSet BindHeadOffice();
        DataTable GetCurrencyList();
        DataTable SuppCityDAL(string GroupName);
        DataTable GetdOCNAME_ENG(string comp_id);
        Dictionary<string, string> OSCityList(string GroupName);
        DataSet GetsuppDSCntrDAL(string SuppCity);
        DataSet GetDataCheckDepency(string Entityprefix,string flag, string comp_id, string RoleHoName, string Branchchk);
        DataSet getst_dt_end_dt(string Headoffice_id);
        string InsertOS_Data(DataTable OCDetail, DataTable ODQuantity,string LandlineNumber,DataTable ORGAddressDetail,DataTable LicenceDetail);
        //string DeleteOSComDetail( int comid);
        DataSet GetviewCOM(int com_ID);
        JObject GetAllHoBranchGrp(OrganizationSetupModel _OrganizationSetupModel);
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        DataTable GetCountryListDDL();
        DataTable GetstateOnCountryDDL(string ddlCountryID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
        DataSet GetStateCode(string stateId);
        /*----------------------Code End of Country,state,district,city--------------------------*/
    }
}
