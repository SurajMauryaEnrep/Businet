using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionAdvice
{
    public class ProductionAdvice_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string WF_Status { get; set; }
        public string WF_Status1 { get; set; }
        public string Message { get; set; }
        public string DocumentStatus { get; set; }
        public string PA_Date { get; set; }
        public string PA_Number { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string ADVSearch { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string advice_no { get; set; }
        public string advice_dt { get; set; }
        public string deletecommand { get; set; }
        public string title { get; set; }
        //public string transtype { get; set; }
        public string createby { get; set; }
        public string createdon { get; set; }
        public string approvedby { get; set; }
        public string approvedon { get; set; }
        public string ammendedby { get; set; }
        public string ammendedon { get; set; }
        public string status { get; set; }
        public string statuscode { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string ddl_revisionno { get; set; }
        public string hdnddl_revisionno { get; set; }
        public List<revisionnumber> revisionnolist { get; set; }
        public string advice_qty { get; set; }
        public string sub_item { get; set; }
        public string batch_no { get; set; }
        public string ddl_financial_year { get; set; }
        public string hdnddl_finyear { get; set; }
        public List<financial_year> ddl_financial_yearList { get; set; }
        public string ddl_period { get; set; }
        public string hdnddlperiod { get; set; }
        public List<period> ddl_periodList { get; set; }
        public string txtfromdate { get; set; }
        public string txttodate { get; set; }
        public string completiondate { get; set; }
        public string hdnfromdate { get; set; }
        public string hdntodate { get; set; }
        public string ddl_src_type { get; set; }
        public string ddl_ProductName { get; set; }
        public Boolean cancelflag { get; set; }
        public string ddlsrctype { get; set; }
        public List<sourcetype> ddl_src_typeList { get; set; }
        public List<status> statuslist { get; set; }
        public string hdnmaterialdetail { get; set; }
        public string hdnopdetail { get; set; }
        public string hdnopitemdetail { get; set; }
        public string remarks { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string attatchmentdetail { get; set; }
        public string src_doc_no { get; set; }
        public string src_doc_dt { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public string SubItemDetailsDt { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public List<ProductName> ProductNameList { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string PAC_No { get; set; }
        public string PAC_dt { get; set; }
        public string DMS { get; set; }

    }
    public class Pro_Model
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class ProductName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class SearchItem
    {
        public string SearchName { get; set; }
    }
    public class sourcetype
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class period
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class revisionnumber
    {
        public string rev_no { get; set; }
        public string rev_text { get; set; }
    }
    public class financial_year
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}