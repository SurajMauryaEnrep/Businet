using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//All Session Removed By Shubham Maurya On 20-12-2022//
namespace EnRepMobileWeb.MODELS.BusinessLayer.MiscellaneousSetup
{
    public class MISDetail_Model
    {
        //public string setup_type_id { get; set; }
        public string hdnsetuptypeid { get; set; }
        public string CustomerZone_id { get; set; }
        public string Customergroup_id { get; set; }
        public string ShowCustomergroup { get; set; }
        public string ShowCustomerZone { get; set; }
        public string Customergroup { get; set; }
        public string CustomerZone { get; set; }
        public string ShowAsset { get; set; }
        public string AssetCategory { get; set; }
        public string AssetCategory_id { get; set; }
        public string ShowGLReport { get; set; }
        public string GLReport_Id { get; set; }
        public string GLReportingGroup { get; set; }
        public string ShowEmployee { get; set; }
        public string Emp_EmailId { get; set; }
        public string Emp_ContactNo { get; set; }
        public string EmployeeNameID { get; set; }
        public string EmployeeName { get; set; }
        public string Rejection_id { get; set; }
        public string RejectionReason { get; set; }
        public string Wstg_id { get; set; }
      
        public string WastageReason { get; set; }
        public string hdnSavebtn { get; set; }
        public string checkdependcy { get; set; }
        public string BtnNameMIS { get; set; }
        public string setup_flag { get; set; }
        public string setup_val { get; set; }
        public string setup_id { get; set; }
        public string TransTypeMIS { get; set; }
        public string setup_type_id { get; set; }
        public string MessageMIS { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string BINNumber { get; set; }
        public string BIN_id { get; set; }//PortfolioName
        public string PortfolioName { get; set; }
        public string Portfolio_id { get; set; }
        public string CustPortfolioName { get; set; }
        public string CustPortfolio_id { get; set; }
        public string SuppPortfolioName { get; set; }
        public string SuppPortfolio_id { get; set; }
        public string CustCategoryName { get; set; }
        public string CustCategory_id { get; set; }
        public string SuppCategoryName { get; set; }
        public string SuppCategory_id { get; set; }
        public string SalesRegionName { get; set; }
        public string SalesRegion_id { get; set; }
        public string RequirmentAreaName { get; set; }
        public string GroupName { get; set; }
        public string GroupName_id { get; set; }
        public string VehicleName { get; set; }
        public string VehicleName_id { get; set; }
        public Boolean ShopFloor { get; set; }
        public string RequirmentArea_id { get; set; }      
        public string BtnName { get; set; }
        public string ShowBin { get; set; }
        public string ShowItemPort { get; set; }
        public string ShowCustPort { get; set; }
        public string ShowSuppPort { get; set; }
        public string ShowCustCat { get; set; }
        public string ShowSuppCat { get; set; }
        public string ShowSalesReg { get; set; }
        public string ShowReqArea { get; set; }
        public string ShowGroupName { get; set; }
        public string ShowSEName { get; set; }
        public string ShowRejectionReason { get; set; }
        public string ShowWastage { get; set; }
        public string ShowPortSetup { get; set; }
        public string ShowVehicleName { get; set; }
       
       
        public string Br_ID { get; set; }
        public string SeId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string SEContactNo { get; set; }
        public string SEEmailId { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public string CustomerBranchDetails { get; set; }
        public List<BrList> brLists { get; set; }
        public List<Countrylist> Country_list { get; set; }
        public List<SalesRegionlist> SalesRegionlist { get; set; }
        public string Country { get; set; }
        public string SalesRegion { get; set; }
        public string ddlstate { get; set; }
        public string countryid { get; set; }
        public string portid { get; set; }
        public string PortDescription { get; set; }
        public string PinCode { get; set; }
        public string hdnprot_id { get; set; }
        public string state { get; set; }
        public string Porttype { get; set; }
        public List<statelist> state_list { get; set; }
        public string DocumentMenuId { get; set; }
    }
    public class BrList
    {
        public string br_id { get; set; }
        public string br_name { get; set; }
    }
    public class Countrylist
    {
        public string country_id { get; set; }
        public string country_name { get; set; }
    }
    public class SalesRegionlist
    {
        public string sr_id { get; set; }
        public string sr_name { get; set; }
    }
    public class statelist
    {
        public string state_id { get; set; }
        public string state_name { get; set; }
    }
}
