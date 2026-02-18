using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS
{
   public class Enumerator
    {
        public enum DataOperation : int
        {
            Create = 0,
            Update = 1,
            Correct = 2,
            Delete = 3,
            Read = 4
        }
    }
}
