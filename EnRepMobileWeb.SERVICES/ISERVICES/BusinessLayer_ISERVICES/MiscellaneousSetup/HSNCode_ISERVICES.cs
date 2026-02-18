using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup
{
   public  interface HSNCode_ISERVICES
    {
        string SaveHSNDetails(string TransType ,string setup_id,string dbk_code, string setup_val, string Remarks, string hsn_des, string mac_id, string comp_id);
        string DeleteHSNDetail(string hsn_number, string comp_id);
        DataSet GetTaxDetailAgainstHSN(string hsn_number, string comp_id);
        DataSet Get_HsnDetails(string comp_id);
    }
}
