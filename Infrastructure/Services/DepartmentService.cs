using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IEmployeeService _employeeService;

        public DepartmentService(IDepartmentRepository departmentRepo, IEmployeeService employeeService)
        {
            _departmentRepo = departmentRepo;
            _employeeService = employeeService;
        }

        public async Task<Department> Add(Department department)
        {
            if (_departmentRepo.IsExists(d => d.DepartmentName == department.DepartmentName).Result.Any())
                return null;

            await _departmentRepo.Add(department);
            return department;
        }

        public void Dispose()
        {
            _departmentRepo?.Dispose();
        }

        public async Task<IReadOnlyList<Department>> GetAll()
        {
            return await _departmentRepo.GetAll();
        }

        public async Task<Department> GetById(int id)
        {
            return await _departmentRepo.GetById(id);
        }

        public async Task<bool> Remove(Department department)
        {
            var employees = await _employeeService.GetEmployeesByDepartment(department.Id);

            if (employees.Any())
                return false;

            await _departmentRepo.Remove(department);
            return true;
        }

        public async Task<Department> Update(Department department)
        {
            if (_departmentRepo.IsExists(d => d.DepartmentName == department.DepartmentName
                    && d.Id != department.Id).Result.Any())
                return null;

            await _departmentRepo.Update(department);
            return department;
        }
    }
}
