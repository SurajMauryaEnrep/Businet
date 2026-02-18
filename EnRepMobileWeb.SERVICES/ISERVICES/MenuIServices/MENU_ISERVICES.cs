using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES
{
    public interface MENU_ISERVICES
    {
         DataSet GetAllMenuDAL(string CompID, string Language, string UserID);
        DataSet GetAllTopNavBrchList(string CompID, string User_id, string brId);
        int SaveUpdateMyFavouriteMenu(string action,string compId, string userId, string docId);
        DataSet GetMyFavMenuDetails(string compId, string userId, string docId);
        DataSet GetMyFavListItems(string compId,string language, string userId);
       
    }
}
