using LinkDev.Talabat.Core.Domain.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications.Employees
{
    public class EmployeesWithDepartmentSpecifications : BaseSpecifications<Employee,int>
    {
        public EmployeesWithDepartmentSpecifications() : base()
        {
            Includes.Add(e => e.Department!);
        }


        public EmployeesWithDepartmentSpecifications(int id) : base(id)
        {
            Includes.Add(e => e.Department!);
        }
    }
}
