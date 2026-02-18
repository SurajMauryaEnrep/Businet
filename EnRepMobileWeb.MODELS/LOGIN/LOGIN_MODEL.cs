using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnRepMobileWeb.MODELS.LOGIN
{
    public class LOGIN_MODEL
    {
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string UserPassword { get; set; }
        public bool RememberMe { get; set; }
        public string CompanyName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }

        public string Language { get; set; }

       public string UserMessage { get; set; }
        public string PassMessage { get; set; }
        public string UserBorder { get; set; }
        public string PassBorder { get; set; }
        public string ClassForUser { get; set; }
        public string ClassForPass { get; set; }


        public List<ClassCompanyName> CompanyNameList { get; set; }

        public List<ClassBranchName> BranchNameList { get; set; }

        public List<ClassLanguage> LanguageList { get; set; }


    }
    public class ClassCompanyName
    {

        public string Comp_id { get; set; }
        public string Comp_name { get; set; }
    }
    public class ClassBranchName
    {

        public string Br_id { get; set; }
        public string Br_name { get; set; }
    }
    public class ClassLanguage
    {
        public string lang_id { get; set; }
        public string lang_name { get; set; }
         
            

    }
}
