using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassOutward;
using EnRepMobileWeb.UTILITIES;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.GatePassOutward
{
    public class GatePassOutwardList_Services : GatePassOutwardList_IServices
    {
       
        public DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();

               
                    SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                      objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                     };
                    DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GatePass_Supp_CustList", prmContentGetDetails);
                    return PARQusData;
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            //return null;
        }
        public DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
             string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$gpass$out$list$bind", prmContentGetDetails);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable SearchDataFilter(string Entity_type, string Entity_id , string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@Entity_type",DbType.String, Entity_type),
                objProvider.CreateInitializedParameter("@Entity_id",DbType.String, Entity_id),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Gpass$detail_Filter", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
