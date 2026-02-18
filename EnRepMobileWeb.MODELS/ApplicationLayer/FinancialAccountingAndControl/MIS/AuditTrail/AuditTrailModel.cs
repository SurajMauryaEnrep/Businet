using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AuditTrail
{
   public class AuditTrailModel
    {
        public string Title { get; set; }
        public string AccName { get; set; }
        public string curr { get; set; }
        public string FromVouDate { get; set; }
        public string From_VouDate { get; set; }
        public string ToVouDate { get; set; }
        public string To_VouDate { get; set; }
        public string ddlGroup { get; set; }
        public string ddlGroupName { get; set; }
        public string GLAccount { get; set; }
        public string GLAccountName { get; set; }
        public string VouAmountFrom { get; set; }
        public string VouAmountTo { get; set; }
        public string VouType { get; set; }
        public string VouTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorName { get; set; }
        public string CreatedOn { get; set; }
        public string CreateDate { get; set; }

        public string ApprovedBy { get; set; }
        public string ApproverName { get; set; }
        public string ApprovedOn { get; set; }


        public string ApproveDate { get; set; }
        public string Narration { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string SearchStatus { get; set; }
        public string CC_DetailList { get; set; }
        public string Allfilters { get; set; }
        public List<GlGroupName> GlGroupNameList { get; set; }
        public List<GLAccount> GLAccountList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<CreatorName> CreatorList { get; set; }
        public List<ApproverName> ApproverList { get; set; }
        public List<AuditTrailList> AuditTrail_List { get; set; }

        /*--------------Print Work----------------*/
        public string PrintData { get; set; }
        public string hdnPDFPrint { get; set; }
        public string hdnCSVPrint { get; set; }
        public string searchValue { get; set; }
    }
    public class GlGroupName
    {
        public string acc_grp_name { get; set; }

        public string acc_grp_id { get; set; }
    }

    public class GLAccount
    {
        public string glacc_id { get; set; }
        public string glacc_name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class CreatorName
    {
        public string creator_id { get; set; }
        public string creator_name { get; set; }
    }
    public class ApproverName
    {
        public string approver_id { get; set; }
        public string approver_name { get; set; }
    }
    public class AuditTrailList
    {
        public Int64 SrNo { get; set; }
        public string sono_total { get; set; }
        public string Vou_contain_row { get; set; }

        public string Vou_No { get; set; }
        public string Vou_Dt { get; set; }
        public string hdnVouDate { get; set; }
        public string Vou_Type { get; set; }
        public string Vou_TypeName { get; set; }
        public string Instrument_Type { get; set; }
        public string Instrument_TypeName { get; set; }
        public string Instrument_No { get; set; }
        public string StatusName { get; set; }
        public string StatusId { get; set; }
        public string Created_By { get; set; }
        public string Created_On { get; set; }
        public string Approved_By { get; set; }
        public string Approved_On { get; set; }
        public string Cancelled_By { get; set; }
        public string Cancelled_On { get; set; }
        public string Cancelled_Remarks { get; set; }
       
        public string CurrName { get; set; }
        public string CurrId { get; set; }
        public string Acc_Name { get; set; }
        public string Acc_Id { get; set; }
        public string AccTyp_Id { get; set; }
        public string Dr_Amt { get; set; }
        public string Cr_Amt { get; set; }
        public string Narr { get; set; }



    }
}
