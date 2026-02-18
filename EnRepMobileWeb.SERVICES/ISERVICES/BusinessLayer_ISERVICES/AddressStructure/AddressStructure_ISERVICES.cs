using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Modifed by Shubham Maurya on 16-12-2022 add state_code//
namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AddressStructure
{
    public interface AddressStructure_ISERVICES
    {
       
        DataSet GetStateCodeOnchange(string CompID);
        DataSet getcountrylist(string countryName);
        Dictionary<string, string> countrylist(string GroupName);
        Dictionary<string, string> statelist(string GroupName);
        Dictionary<string, string> districtlist(string GroupName);
        DataSet getAddressStructureLists();
        DataSet getAddressStructuredetails(string state_id);
        string deleteaddressstructure(string _ID,string flag);
        string insertstate(string state, string state_code, string country, string state_id, string TransType);
        string insertdistrict(string district_id,string district_name,string state_id,string country,string TransType);
        string insertcityandpin(string city_id,string city, string district_id, string state_id, string country, string TransType);
    }
}
