using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        new Task<IReadOnlyList<Employee>> GetAll();
        Task<IReadOnlyList<Employee>> GetEmployeesByDepartment(int departmentId);
    }
}