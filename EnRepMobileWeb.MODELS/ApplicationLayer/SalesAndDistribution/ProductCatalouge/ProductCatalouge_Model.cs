using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge
{
     public class ProductCatalouge_Model
    {
        // For List Page
        public string propectflag { get; set; }
        public string CustPros_type { get; set; }
        public string ProspectFromProduct { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string Catl_FromDate { get; set; }
        public string Catl_ToDate { get; set; }
        public string Catl_status { get; set; }
        public List<StatusList> _statusList { get; set; }
        public class StatusList
        {
            public string status_id { get; set; }
            public string status_name { get; set; }
        }       //List Page End
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
       public string StatusName { get; set; }
        public string Status { get; set; }

        // For Header Detail
        public string CompID { get; set; }
        public string BrchID { get; set; }
        public string TransType { get; set; }
        public string Itemdetails { get; set; }
        public string CTLNo { get; set; }
        public string CTLDate { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public List<CustNameList> _CustNameList { get; set; }
        public string Remarks { get; set; }
       
        //For Item Table Detail
        public string item_id { get; set; }
        public string item_Name { get; set; }
        public int uom_id { get; set; }
        public string uom_Name { get; set; }
        public List<ItemNameList> _ItemNameList { get; set; }
        public string ddlgroup_name { get; set; }

        //public string Item_group_name { get; set; }
        public string item_grp_id { get; set; }
        public List<GroupNameList> GroupList { get; set; }
        public string PortfName { get; set; }
        public string PortfID { get; set; }
        public List<Portfolio> PortfolioList { get; set; }
        public string VehicleName { get; set; }
        public List<Vehicle> VehicleList { get; set; }
        public string VehOEM_No { get; set; }
        public string ReferenceNo { get; set; }
        public string TechSpecific { get; set; }
        public string Delete { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string CatalogSearch { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string WF_status1 { get; set; }

        /*-------------For PopUp Print add by Hina Sharma on 24-07-2025-------------------- */
        public string DocumentMenuId { get; set; }
        public string PrintItemImage { get; set; } = "Y";
        public string ShowProdDesc { get; set; } = "N";
        public string ShowUOM { get; set; } = "N";
        public string ShowItemAliasName { get; set; } = "N";
        public string ShowOEMNumber { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowProdTechSpec { get; set; } = "N";
        public string ShowCatalougeNumber { get; set; } = "N";
        public string ShowRefNumber { get; set; } = "N";
        public string ShowVehicleName { get; set; } = "N";
        public string ShowModelNumber { get; set; } = "N";
        /*-------------For PopUp Print End by Hina Sharma on 24-07-2025-------------------- */
        /*-------------For PopUp Label Print add by Hina Sharma on 29-08-2025-------------------- */
        public string LblProdDesc { get; set; }
        public string LblUOM { get; set; }
        public string LblItemAlias { get; set; }
        public string LblOEMNumber { get; set; }
        public string LblProdTechDesc { get; set; }
        public string LblProdTechSpec { get; set; }
        public string LblCatalougeNum { get; set; }
        public string LblRefNumber { get; set; }
        public string LblVehicleName { get; set; }
        public string LblModelNumber { get; set; }
        


        public List<CustomerName> CustomerNameList { get; set; }
        public class CustomerName
        {
            public string cust_id { get; set; }
            public string cust_name { get; set; }
        }
        public class CustNameList
        {
            public string Cust_name { get; set; }

            public string Cust_id { get; set; }
        }
        public class ItemNameList
        {
            public string item_id { get; set; }
            public string item_name { get; set; }
        }
        public class GroupNameList
        {
            public string item_grp_id { get; set; }
            public string ItemGroupChildNood { get; set; }
        }
        public class Portfolio
        {
            public string setup_id { get; set; }
            public string setup_val { get; set; }
        }
        public class Vehicle
        {
            public string setup_id { get; set; }
            public string setup_val { get; set; }
        }
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
    }
}
