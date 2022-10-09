using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmployeeService : IDisposable
    {
        Task<IReadOnlyList<Employee>> GetAll();
        Task<Employee> GetById(int id);
        Task<Employee> Add(Employee employee);
        Task<Employee> Update(Employee employee);
        Task<bool> Remove(Employee employee);
        Task<IReadOnlyList<Employee>> GetEmployeesByDepartment(int departmentId);
    }
}
