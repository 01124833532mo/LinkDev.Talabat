using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Apis.Controllers.Exeptions
{
    public class NotFoundExeption :ApplicationException
    {
        public NotFoundExeption() : base("Not Found")
        {
            
        }
    }
}
