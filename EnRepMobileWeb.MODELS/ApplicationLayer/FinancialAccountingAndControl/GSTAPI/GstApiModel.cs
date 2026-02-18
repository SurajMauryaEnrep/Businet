using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GSTAPI
{
    public class GstApiModel
    {
        public string Title { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchStatus { get; set; }
        public string DataType { get; set; }
        public string DocStatus { get; set; }
        public string IsSearched { get; set; }
        public string MonthYear { get; set; }
        public string Gst_Cat { get; set; }
        public List<FinMonthYearModel> FinMonthYear { get; set; }
    }
    public class FinMonthYearModel
    {
        public string FinMonthYearName { get; set; }
        public string MnthYear { get; set; }
    }
}
