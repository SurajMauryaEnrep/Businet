using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnRepMobileWeb.MODELS.Enumerator;

namespace EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup
{
    public class QCItemParameterSetup
    {
        public string CheckDependcy { get; set; }
        public string hdnSavebtn { get; set; }
        public string BtnEditName { get; set; }
        public string MessageIPS { get; set; }
        public string BtnNameIPS { get; set; }
        public string Commandfc { get; set; }
        public string Action { get; set; }
        public string ItemId { get; set; }
        public string Title { get; set; }
        public string param_Id { get; set; }
        public string uom_Id { get; set; }
        public string param_name { get; set; }
        public int SerialNumber { get; set; }
        public string para_type { get; set; }
        public string paratypeId { get; set; }
        public string upper_val { get; set; }
        public string lower_val { get; set; }
       // public string remarks { get; set; }


        public string comp_id { get; set; }
        public string item_id { get; set; }
        public string ListFilterData1 { get; set; }
        public string item_Name { get; set; }
        public string UOMName { get; set; }
        public string RefNo { get; set; }
        public string OEMNo { get; set; }
        public string SampleCode { get; set; }
        public string create_name { get; set; }
        public string create_dt { get; set; }
        public string mod_name { get; set; }
        public string mod_dt { get; set; }
        public string app_name { get; set; }
        public string app_dt { get; set; }
        public string mac_id { get; set; }
        public string status { get; set; }
        public DataOperation Operation { get; set; }
        public string remarks { get; set; }
        public List<GetItemList> getitemList { get; set; }
        public List<ItemList> getitemDropdownList { get; set; }
        public List<ItemNameList> ItemList { get; set; }

        public List<CurrentDetail> CurrentDetail { get; set; }
        public List<ParameterNameList> ParameterNameList { get; set; }
        public List<Uom> UomList { get; set; }
        public string hdnItemList { get; set; }
        public string item { get; set; }

    }

    public class ParameterNameList
    {
        public string param_Id { get; set; }
        public string param_name { get; set; }
    }
    public class Uom
    {
        public string uom_id { get; set; }
        public string uom_name { get; set; }


    }
    public class CurrentDetail
    {
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class BindItemNameList
    {
        public string item_id { get; set; }
        public string item_Name { get; set; }
        public string UOMName { get; set; }
        public string RefNo { get; set; }
        public string OEMNo { get; set; }
        public string SampleCode { get; set; }
    }
    public class ItemNameList
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
    public class ItemList 
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string UOM { get; set; }
    }
    public class GetItemList
    {

        public string param_Id { get; set; }
        public string param_name { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string para_type { get; set; }
        public string paratypeId { get; set; }
        public float upper_val { get; set; }
        public float lower_val { get; set; }
        public string remarks { get; set; }


        //public string item_id { get; set; }
        //public string item_name { get; set; }
        //public string item_oem_no { get; set; }
        //public string item_sam_cd { get; set; }
        //public string uom_id { get; set; }
        //public string uom_name { get; set; }
        //public string ref_no { get; set; }
        //public string HSNcode { get; set; }



    }
}
