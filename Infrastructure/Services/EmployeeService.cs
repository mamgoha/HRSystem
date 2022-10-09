using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeService(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<Employee> Add(Employee employee)
        {
            if (_employeeRepo.IsExists(e => e.EmpCode == employee.EmpCode).Result.Any())
                return null;

            await _employeeRepo.Add(employee);
            return employee;
        }

        public void Dispose()
        {
            _employeeRepo?.Dispose();
        }

        public async Task<IReadOnlyList<Employee>> GetAll()
        {
            return await _employeeRepo.GetAll();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _employeeRepo.GetById(id);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeesByDepartment(int departmentId)
        {
            return await _employeeRepo.GetEmployeesByDepartment(departmentId);
        }

        public async Task<bool> Remove(Employee employee)
        {
            await _employeeRepo.Remove(employee);
            return true;
        }

        public async Task<Employee> Update(Employee employee)
        {
            if (_employeeRepo.IsExists(e => e.EmpCode == employee.EmpCode
                    && e.Id != employee.Id).Result.Any())
                return null;

            await _employeeRepo.Update(employee);
            return employee;
        }
    }
}
