using LinkDev.Talabat.Core.Application.Abstraction.Services.Employees;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services
{
    public interface IServiceManager
    {

        public IProductService ProductService { get;  }
        public IEmployeeService EmployeeService { get; }


    }
}
