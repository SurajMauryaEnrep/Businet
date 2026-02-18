using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings.ResetData
{
    public interface ResetData_IService
    {
        DataSet BindHeadOffice();
        DataSet BindBranchList(string comp_id);
        string FactoryReset_data(string comp_id, string br_id, string flag);
    }
}
