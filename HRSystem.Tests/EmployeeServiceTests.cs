using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfEmployees_WhenEmployeesExist()
        {
            var employees = CreateEmployeeList();

            _employeeRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync(employees);

            var result = await _employeeService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Employee>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenEmployeesDoNotExist()
        {
            _employeeRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((IReadOnlyList<Employee>)null);

            var result = await _employeeService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _employeeRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((IReadOnlyList<Employee>)null);

            await _employeeService.GetAll();

            _employeeRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnEmployee_WhenEmployeeExist()
        {
            var employee = CreateEmployee();

            _employeeRepositoryMock.Setup(c =>
                c.GetById(employee.Id)).ReturnsAsync(employee);

            var result = await _employeeService.GetById(employee.Id);

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            _employeeRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Employee)null);

            var result = await _employeeService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _employeeRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Employee)null);

            await _employeeService.GetById(1);

            _employeeRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddEmployee_WhenEmployeeCodeDoesNotExist()
        {
            var employee = CreateEmployee();

            _employeeRepositoryMock.Setup(c =>
                c.IsExists(c => c.EmpCode == employee.EmpCode))
                .ReturnsAsync(new List<Employee>());
            _employeeRepositoryMock.Setup(c => c.Add(employee));

            var result = await _employeeService.Add(employee);

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddEmployee_WhenEmployeeCodeAlreadyExist()
        {
            var employee = CreateEmployee();
            var employeeList = new List<Employee>() { employee };

            _employeeRepositoryMock.Setup(c =>
                c.IsExists(c => c.EmpCode == employee.EmpCode)).ReturnsAsync(employeeList);

            var result = await _employeeService.Add(employee);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var employee = CreateEmployee();

            _employeeRepositoryMock.Setup(c =>
                    c.IsExists(c => c.EmpCode == employee.EmpCode))
                .ReturnsAsync(new List<Employee>());
            _employeeRepositoryMock.Setup(c => c.Add(employee));

            await _employeeService.Add(employee);

            _employeeRepositoryMock.Verify(mock => mock.Add(employee), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateEmployee_WhenEmployeeCodeDoesNotExist()
        {
            var employee = CreateEmployee();

            _employeeRepositoryMock.Setup(c =>
                c.IsExists(c => c.EmpCode == employee.EmpCode && c.Id != employee.Id))
                .ReturnsAsync(new List<Employee>());
            _employeeRepositoryMock.Setup(c => c.Update(employee));

            var result = await _employeeService.Update(employee);

            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateEmployee_WhenEmployeeDoesNotExist()
        {
            var employee = CreateEmployee();
            var employeeList = new List<Employee>()
            {
                new Employee()
                {
                    Id = 2,
                    EmpCode = "0002",
                    EmpName = "Employee 2",
                    DateOfBirth = DateTime.Parse("01/01/1983"),
                    Address = "Address",
                    Mobile = "8527410963",
                    DepartmentId = 2,
                    Salary = 9000
                }
            };

            _employeeRepositoryMock.Setup(c =>
                    c.IsExists(c => c.EmpCode == employee.EmpCode && c.Id != employee.Id))
                .ReturnsAsync(employeeList);

            var result = await _employeeService.Update(employee);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var employee = CreateEmployee();

            _employeeRepositoryMock.Setup(c =>
                    c.IsExists(c => c.EmpCode == employee.EmpCode && c.Id != employee.Id))
                .ReturnsAsync(new List<Employee>());

            await _employeeService.Update(employee);

            _employeeRepositoryMock.Verify(mock => mock.Update(employee), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnTrue_WhenEmployeeCanBeRemoved()
        {
            var employee = CreateEmployee();

            var result = await _employeeService.Remove(employee);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var employee = CreateEmployee();

            await _employeeService.Remove(employee);

            _employeeRepositoryMock.Verify(mock => mock.Remove(employee), Times.Once);
        }

        private Employee CreateEmployee()
        {
            return new Employee()
            {
                Id = 1,
                EmpCode = "0001",
                EmpName = "Employee 1",
                DateOfBirth = DateTime.Parse("01/01/1980"),
                Address = "Address",
                Mobile = "8527410963",
                DepartmentId = 1,
                Salary = 5000
            };
        }

        private IReadOnlyList<Employee> CreateEmployeeList()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    EmpCode = "0001",
                    EmpName = "Employee 1",
                    DateOfBirth = DateTime.Parse("01/01/1980"),
                    Address = "Address",
                    Mobile = "8527410963",
                    DepartmentId = 1,
                    Salary = 5000
                },
                new Employee()
                {
                    Id = 2,
                    EmpCode = "0002",
                    EmpName = "Employee 2",
                    DateOfBirth = DateTime.Parse("01/01/1983"),
                    Address = "Address",
                    Mobile = "8527410963",
                    DepartmentId = 2,
                    Salary = 9000
                },
                new Employee()
                {
                    Id = 3,
                    EmpCode = "0003",
                    EmpName = "Employee 3",
                    DateOfBirth = DateTime.Parse("01/01/1985"),
                    Address = "Address",
                    Mobile = "8527410963",
                    DepartmentId = 3,
                    Salary = 7000
                }
            };
        }
    }
}
