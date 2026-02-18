using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.MaterialTracking
{
    public class MaterialTracking_Model
    {
		public string Title { get; set; }
		public string MtrlTrackSearch { get; set; }
		public string JO_ItemName { get; set; }
		public string Supp_Name { get; set; }
		public string Supp_ID { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public string JobOrdNo { get; set; }
		public List<JobOrderNoList> JobOrdList { get; set; }
		//public string JobOrdDate { get; set; }
		//public string JobOrdDt { get; set; }
		public string OpOut_ItemName { get; set; }
		public string OpOut_ItemId { get; set; }
		//public string Status { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		//public List<Status> StatusList { get; set; }
		public List<MaterialTrackListDetail> MaterialTrackList { get; set; }
		
	}
	public class SupplierName
	{
		public string supp_id { get; set; }
		public string supp_name { get; set; }
	}
	public class JobOrderNoList
	{
		public string JobOrdnoId { get; set; }
		public string JobOrdnoVal { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class MaterialTrackListDetail
	{
		public string OrderNo { get; set; }
		public string OrderDate { get; set; }
		public string OrderDt { get; set; }
		public string SupplierName { get; set; }
		public string SupplierId { get; set; }
		public string OpOutProductName { get; set; }
		public string OpOutProductID { get; set; }
		public string OpOutUOM { get; set; }
		public string OpOutUOMId { get; set; }
		public string OrderQuantity { get; set; }
		public string MaterialName { get; set; }
		public string MaterialID { get; set; }
		public string MaterialUOM { get; set; }
		public string MaterialUOMId { get; set; }
		public string IssueQuantity { get; set; }
		public string ConsumeQuantity { get; set; }
		public string ReturnQuantity { get; set; }
		public string BalanceQuantity { get; set; }
		
		
	}
}
