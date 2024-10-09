using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Employees;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Employees;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Contracts.Specifications.Employees;
using LinkDev.Talabat.Core.Domain.Entities.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Employees
{
    internal class EmployeeService(IUnitOfWork unitOfWork,IMapper mapper) : IEmployeeService
    {
      
        public async Task<IEnumerable<EmployeeToReturnDto>> GetEmployeesAsync()
        {
            var spec = new EmployeesWithDepartmentSpecifications();

            var employees = await unitOfWork.GetRepository<Employee, int>().GetAllWithSpecAsync(spec);

            var employeestoreturn = mapper.Map<IEnumerable<EmployeeToReturnDto>>(employees);

            return employeestoreturn;

        }

        public async Task<EmployeeToReturnDto> GetEmployeeAsync(int id)
        {
            var spec = new EmployeesWithDepartmentSpecifications(id);

            var emploee= await unitOfWork.GetRepository<Employee,int>().GetWithSpecAsync(spec);
            var employeetoreturn =mapper.Map<EmployeeToReturnDto>(emploee);

            return employeetoreturn;
        }

    }
}
