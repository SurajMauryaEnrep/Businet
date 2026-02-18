using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.ItemAttributeSetup
{
    public class ItemAttributeSetupModel
    {
        public string attr_val_id { get; set; }
        public string attrName_id { get; set; }
        public string attrValName { get; set; }
        public string attrName { get; set; }
        public string attr_id { get; set; }
        public string collapse { get; set; }
        public string Message { get; set; }
        public string actionCommand { get; set; }
        public string Title { get; set; }
        public string attributeID { get; set; }
        public string hdnAction { get; set; }
        public string hdnAction1attrval { get; set; }
        public string attributeName { get; set; }
        public string L_attribute_Name { get; set; }
        public string attributeValue { get; set; }
        public string attrVal_Id { get; set; }
        public List<Attributelist> attributes { get; set; }
    }
    public class Attributelist
    {
        public string attri_id { get; set; }
        public string attri_name { get; set; }
    }
}
