using System;
using System.Collections.Generic;
using System.Data;
using EnRepMobileWeb.MODELS.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup
{
    public class TransporterSetupModel
    {
        public CommonAddress_Detail _CommonAddress_Detail { get; set; }

        public string Gst_Cat { get; set; }

        public string PageLevelDisable { get; set; }
        public string GstApplicable { get; set; }
        public string DocumentMenuId { get; set; }
        public string SaveAndApproveBtnDisatble { get; set; }
        public string Title { get; set; }
        public List<TransCountry> countryList { get; set; }
        public List<DistrictModel> DistrictList { get; set; }
        public string DeleteCommand { get; set; }
        public List<StatusModel> StatusList { get; set; }
        public List<CityList> cityLists { get; set; }
        public List<TransState> StateList { get; set; }
        public string CompId { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string ListFilterData1 { get; set; }
        public string TransId { get; set; }
        public string TransType { get; set; }
        public string TransportType { get; set; }
        public string TransName { get; set; }
        public string TransMode { get; set; }
        public string DocumentStatus { get; set; }
        public string TransAdd { get; set; }
        public string TransCntry { get; set; }
        public string TransState { get; set; }
        public string TransCity { get; set; }
        public string TransDist { get; set; }
        public string TransPin { get; set; }
        public string transGstFirstPart { get; set; }
        public string transGstNo { get; set; }
        public string transGstMidPart { get; set; }
        public string transGstLastPart { get; set; }
        public string transPanNo { get; set; }
        public string createModId { get; set; }
        public string TransStatus { get; set; }
        public string MacId { get; set; }
        public bool OnHold { get; set; }
        public string TransOnHold { get; set; }
        public string Remarks { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public string TCreate_id { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string attatchmentDefaultdetail { get; set; }
        public string attatchmentdetail { get; set; }
        public string TStatusName { get; set; }


        public DataTable AttachMentDetailItmStp { get; set; }
    }
    public class TransportAttachment
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class TransCountry
    {
        public string country_id { get; set; }
        public string country_name { get; set; }

    }
    public class TransState
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class CityList
    {
        public string City_Id { get; set; }
        public string City_Name { get; set; }
    }
    public class DistrictModel
    {
        public string district_id { get; set; }
        public string district_name { get; set; }
    }
    public class StatusModel
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
