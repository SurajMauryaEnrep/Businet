using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.MODELS.BusinessLayer.WarehouseSetup
{
    public class WarehouseSetupModel
    {
        public string SerchData { get; set; }
        public string hdnSavebtn { get; set; }
        public string Duplicate { get; set; }
        public string Title { get; set; }
        public string ddlcity { get; set; }
        public string TransType { get; set; }
        public int wh_id { get; set; }
        public string create_id { get; set; }
        public string app_status { get; set; }
        public string DeleteCommand { get; set; }
        public string creat_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseType { get; set; }
        public string Address { get; set; }
        public string CityAndPIN { get; set; }
        public int CityAndPinID { get; set; }
        public string WarehouseTypefilter { get; set; }
        public List<CityAndPIN> _CityAndPIN_list { get; set; }
        public string DistrictAndZone { get; set; }
        public string ware_state { get; set; }
        public List<wh_namelist> wh_Namelists { get; set; }
        public string StateAndProvince { get; set; }
        public string Country { get; set; }
        public string ware_dist { get; set; }
        public string ware_Country { get; set; }
        public string branchId { get; set; }
        public Boolean RejWh { get; set; }
        public Boolean RewWh { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string wh_id_model { get; set; }
        public string details { get; set; }
        public string WHSearch { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public DataTable WhList { get; set; }
        public DataTable CustomerBranchList { get; set; }
    }
    public class wh_namelist
    {
        public int WareH_id { get; set; }
        public string wareH_name { get; set; }
    }
    public class CityAndPIN
    {
        public string CityAndPin_id { get; set; }
        public string CityAndPin_val { get; set; }
    }

   
}
