using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Modifed by Shubham Maurya on 16-12-2022 add state_code//
namespace EnRepMobileWeb.MODELS.BusinessLayer.AddressStructure
{

    public  class AddressStructureModel
    {
        public string DocumentMenuId { get; set; }
        public string hdnSavebtn { get; set; }
        public string hdn_Country_ID { get; set; }
        public string country_nm { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string Collapse { get; set; }
        public string state_code { get; set; }
        public string Title { get; set; }
        public int state_id { get; set; }

        public string Dstate_id { get; set; }
        public string state_name { get; set; }
        public string Dstate_name { get; set; }
        public List<country_list> country_List { get; set; }
        public List<state_list> state_Lists { get; set; }
        public string country_id { get; set; }
        public string Dcountry_id { get; set; }
        public string country_name { get; set; }
        public string Dcountry_name { get; set; }
        public string Name { get; set; }
        public string hdnAction { get; set; }
        public string DhdnAction { get; set; }
        public string ChdnAction { get; set; }
        public string district_id { get; set; }
        public string district_name { get; set; }
        //-------country model start -------
        public string City_name { get; set; }
        public string City_id { get; set; }
        public string pin { get; set; }
        public string Cdistrict_name { get; set; }
        public string Cdistrict_id { get; set; }
        public string Cstate_name { get; set; }
        public string Cstate_id { get; set; }
        public string Ccountry_name { get; set; }
        public string Ccountry_id { get; set; }
        public List<district_list> district_Lists { get; set; }

        //-------country model start -------

    }
    public class district_list
    {
        public int dist_id { get; set; }
        public string dist_name { get; set; }
    }
    public class state_list
    {
        public int dist_state_id { get; set; }
        public string dist_state_name { get; set; }
    }

    public class country_list
    {
        public int country_id { get; set; }
        public string country_name { get; set; }
    }
}
