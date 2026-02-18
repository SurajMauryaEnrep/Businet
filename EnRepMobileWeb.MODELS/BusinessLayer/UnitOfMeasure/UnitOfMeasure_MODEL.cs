using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.UnitOfMeasure
{
    public class UnitOfMeasure_MODEL
    {
        public string hdnSavebtn { get; set; }
        public string uom_name { get; set; }
        public string uom_alias { get; set; }
        public string Title { get; set; }
        public string uom_id { get; set; }
        public string BtnName { get; set; }
        public string mac_id { get; set; }
        public string Message { get; set; }
        public string TransTypeUOM { get; set; }
        public string BtnNameUOM { get; set; }
        public string ItemName { get; set; }
        public string ConvertedUnit { get; set; }
        public string Uom { get; set; }
        public string UomID { get; set; }
        public string ConversionRate { get; set; }
        public bool ShowStock { get; set; }
        public string ShowUom { get; set; }
        public string ShowCD { get; set; }
        public string DeleteCommand { get; set; }
        public List<ConvertedUnitList> _ConvertedUnitList { get; set; }
        public List<ItemNameList> _ItemNameList { get; set; }
    }
    public class ItemNameList
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class ConvertedUnitList
    {
        public string UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
