using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Exeptions
{
    public class BadRequestExeption : ApplicationException
    {
        public BadRequestExeption(string Message) :base(Message)
        {
            
        }

    }
}
