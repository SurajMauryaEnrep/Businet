using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EnRepMobileWeb.MODELS.BusinessLayer.ItemDetail
{
    public class ItemListModel
    {
        //public string ILSearch { get; set; }
        //public string IL_SSearch { get; set; }
        public Int64 SrNo { get; set; }
        public string Item_cat_no { get; set; }
        public string Item_leg_cd { get; set; }
        public string Image { get; set; }
        public string ItemName { get; set; }
        public string ItemID { get; set; }
        public string Group { get; set; }
        public string UOM { get; set; }
        public string OEMNo { get; set; }
        public string SampleCode { get; set; }
        public string Active { get; set; }
        public string Status { get; set; }
        public string ItemCost { get; set; }
        public string ItemPrice { get; set; }
        public string Createdon { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string AmmendedBy { get; set; }
    }
}
