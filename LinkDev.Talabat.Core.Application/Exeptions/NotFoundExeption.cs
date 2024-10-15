using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Exeptions
{
    public class NotFoundExeption :ApplicationException
    {
        public NotFoundExeption(string name,object key) : base($"{name} with ({key} is not Found)")
        {
            
        }
    }
}
