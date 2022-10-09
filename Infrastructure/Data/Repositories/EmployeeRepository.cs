using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HRContext context) : base(context)
        {
        }

        async Task<IReadOnlyList<Employee>> IEmployeeRepository.GetAll()
        {
            var employees = await _context.Employees.AsNoTracking().Include(e => e.Department)
                .OrderBy(e => e.EmpCode)
                .ToListAsync();
            return employees;
        }

        async Task<IReadOnlyList<Employee>> IEmployeeRepository.GetEmployeesByDepartment(int departmentId)
        {
            return await IsExists(e => e.DepartmentId == departmentId);
        }
    }
}
