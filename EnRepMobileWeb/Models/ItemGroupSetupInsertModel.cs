using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemGroupSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemGroupSetup;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EnRepMobileWeb.Models
{
    public class ItemGroupSetupInsertModel
    {


        private ItemGroupModel ObjBl;
        private ItemGroup_ISERVICES ObjBs;
        public string GetData(ItemMenuSearchModel ObjItemMenuSearchModel)
        {
            return "";
        }
        public ItemGroupSetupInsertModel()
        {
            ObjBl = new ItemGroupModel();
            //ObjBs = new ItemGroup_ISERVICES();

        }

        public DataTable ParentItemGroup { get; set; }
        
       
      
    }
}