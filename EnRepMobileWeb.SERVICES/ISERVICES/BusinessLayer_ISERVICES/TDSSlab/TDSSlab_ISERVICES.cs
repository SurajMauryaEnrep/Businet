using EnRepMobileWeb.MODELS.BusinessLayer.TDSSlab;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TDSSlab
{
    public interface TDSSlab_ISERVICES
    {
        DataSet GetTDSList(string comp_id);
        string TDS_SlabInserUpdate(TDSSlab_Model _Model,string TransType);
        string TDS_SlabDelete(string comp_id, string slab_id, string TransType);
    }
}
