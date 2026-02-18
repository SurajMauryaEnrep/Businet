using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.ItemSearch
{
    public class ItemSearch_MODELS
    {
        public string SearchName { get; set; }
        public string Title { get; set; }
        public Boolean act_status { get; set; }
        public string actstatus { get; set; }
        public string ItemNameID { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public string VolumeInLitres { get; set; }
        public string WeightInKg { get; set; }
        public string remarks { get; set; }
        public string HSNCode { get; set; }
        public string GroupName { get; set; }
        public string attatchmentDefaultdetail { get; set; }
        public string attatchmentdetail { get; set; }
        public string TechnicalDescription { get; set; }
        public string TechnicalSpecification { get; set; }
        public string ReferenceNumber { get; set; }
        public string AliasName { get; set; }
        public string OEMNo { get; set; }
        public string UomName { get; set; }
        public string ItemName { get; set; }
        public string ddl_ItemName { get; set; }
        public string PrintNetweight { get; set; } = "Y";
        public string Printgrossweight { get; set; } = "Y";
        public string PrintVolumeinliter { get; set; } = "Y";
        public string PrintWeightinKG { get; set; } = "Y";
        public string PrintRemarks { get; set; } = "Y";
        public string PrintHsnNumber { get; set; } = "Y";
        public string PrintItemGroupName { get; set; } = "Y";
        public string PrintItemName { get; set; } = "Y";
        public string PrintItemImage { get; set; } = "Y";
        public string ShowProdDesc { get; set; } = "Y";
        public string ShowUOM { get; set; } = "Y";
        public string ShowItemAliasName { get; set; } = "Y";
        public string ShowOEMNumber { get; set; } = "Y";
        public string ShowProdTechDesc { get; set; } = "Y";
        public string ShowProdTechSpec { get; set; } = "Y";
        public string ShowCatalogueNumber { get; set; } = "Y";
        public string ShowRefNumber { get; set; } = "Y";
        public string ShowVehicleName { get; set; } = "Y";
        public string ShowModelNumber { get; set; } = "Y";
        public List<ItemName_List> ItemNameList { get; set; }
    }
    public class ItemName_List
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }

}
