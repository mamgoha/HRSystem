using API.Controllers;
using API.Dtos.Employee;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Tests
{
    public class EmployeeControllerTests
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _mapperMock = new Mock<IMapper>();
            _employeeController = new EmployeeController(_employeeServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistEmployee()
        {
            var employees = CreateEmployeeList();
            var dtoExpected = MapModelToEmployeeListDto(employees);

            _employeeServiceMock.Setup(c => c.GetAll()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeReturnDto>>(
                It.IsAny<List<Employee>>())).Returns(dtoExpected);

            var result = await _employeeController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyEmployee()
        {
            var employees = new List<Employee>();
            var dtoExpected = MapModelToEmployeeListDto(employees);

            _employeeServiceMock.Setup(c => c.GetAll()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeReturnDto>>(It.IsAny<List<Employee>>())).Returns(dtoExpected);

            var result = await _employeeController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_OnlyOnce()
        {
            var employees = CreateEmployeeList();
            var dtoExpected = MapModelToEmployeeListDto(employees);

            _employeeServiceMock.Setup(c => c.GetAll()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeReturnDto>>(It.IsAny<List<Employee>>())).Returns(dtoExpected);

            await _employeeController.GetAll();

            _employeeServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenEmployeeExist()
        {
            var employee = CreateEmployee();
            var dtoExpected = MapModelToEmployeeReturnDto(employee);

            _employeeServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeReturnDto>(It.IsAny<Employee>())).Returns(dtoExpected);

            var result = await _employeeController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            _employeeServiceMock.Setup(c => c.GetById(2)).ReturnsAsync((Employee)null);

            var result = await _employeeController.GetById(2);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_OnlyOnce()
        {
            var employee = CreateEmployee();
            var dtoExpected = MapModelToEmployeeReturnDto(employee);

            _employeeServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeReturnDto>(It.IsAny<Employee>())).Returns(dtoExpected);

            await _employeeController.GetById(2);

            _employeeServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }
        
        [Fact]
        public async void Add_ShouldReturnOk_WhenEmployeeIsAdded()
        {
            var employee = CreateEmployee();
            var employeeAddDto = new EmployeeAddDto() 
            { 
                EmpCode = employee.EmpCode,
                EmpName = employee.EmpName,
                DateOfBirth = employee.DateOfBirth,
                Mobile = employee.Mobile,
                Address = employee.Address,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary
            };
            var employeeResultDto = MapModelToEmployeeReturnDto(employee);

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeAddDto>())).Returns(employee);
            _employeeServiceMock.Setup(c => c.Add(employee)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeReturnDto>(It.IsAny<Employee>())).Returns(employeeResultDto);

            var result = await _employeeController.Add(employeeAddDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var employeeAddDto = new EmployeeAddDto();
            _employeeController.ModelState.AddModelError("Employee Code", "The field employee code is required");

            var result = await _employeeController.Add(employeeAddDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenEmployeeResultIsNull()
        {
            var employee = CreateEmployee();
            var employeeAddDto = new EmployeeAddDto() 
            {
                EmpCode = employee.EmpCode,
                EmpName = employee.EmpName,
                DateOfBirth = employee.DateOfBirth,
                Mobile = employee.Mobile,
                Address = employee.Address,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary
            };

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeAddDto>())).Returns(employee);
            _employeeServiceMock.Setup(c => c.Add(employee)).ReturnsAsync((Employee)null);

            var result = await _employeeController.Add(employeeAddDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromService_OnlyOnce()
        {
            var employee = CreateEmployee();
            var employeeAddDto = new EmployeeAddDto()
            {
                EmpCode = employee.EmpCode,
                EmpName = employee.EmpName,
                DateOfBirth = employee.DateOfBirth,
                Mobile = employee.Mobile,
                Address = employee.Address,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary
            };

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeAddDto>())).Returns(employee);
            _employeeServiceMock.Setup(c => c.Add(employee)).ReturnsAsync(employee);

            await _employeeController.Add(employeeAddDto);

            _employeeServiceMock.Verify(mock => mock.Add(employee), Times.Once);
        }

        [Fact]
        public async void Update_ShouldReturnOk_WhenEmployeeIsUpdatedCorrectly()
        {
            var employee = CreateEmployee();
            var employeeEditDto = new EmployeeEditDto()
            {
                Id = employee.Id,
                EmpCode = "0001",
                EmpName = "Test employee",
                DateOfBirth = DateTime.Parse("01/01/1980"),
                Address = "Address",
                Mobile = "12345698",
                DepartmentId = 1,
                Salary = 7000
            };

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeEditDto>())).Returns(employee);
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(c => c.Update(employee)).ReturnsAsync(employee);

            var result = await _employeeController.Update(employeeEditDto.Id, employeeEditDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenEmployeeIdIsDifferentThenParameterId()
        {
            var employeeEditDto = new EmployeeEditDto() { Id = 1, EmpName = "Test" };

            var result = await _employeeController.Update(2, employeeEditDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var employeeEditDto = new EmployeeEditDto() { Id = 1 };
            _employeeController.ModelState.AddModelError("Employee Code", "The field code is required");

            var result = await _employeeController.Update(1, employeeEditDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromService_OnlyOnce()
        {
            var employee = CreateEmployee();
            var employeeEditDto = new EmployeeEditDto() { Id = employee.Id, EmpName = "Test" };

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeEditDto>())).Returns(employee);
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(c => c.Update(employee)).ReturnsAsync(employee);

            await _employeeController.Update(employeeEditDto.Id, employeeEditDto);

            _employeeServiceMock.Verify(mock => mock.Update(employee), Times.Once);
        }
        
        [Fact]
        public async void Remove_ShouldReturnOk_WhenEmployeeIsRemoved()
        {
            var employee = CreateEmployee();
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(c => c.Remove(employee)).ReturnsAsync(true);

            var result = await _employeeController.Remove(employee.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            var employee = CreateEmployee();
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync((Employee)null);

            var result = await _employeeController.Remove(employee.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnBadRequest_WhenResultIsFalse()
        {
            var employee = CreateEmployee();
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(c => c.Remove(employee)).ReturnsAsync(false);

            var result = await _employeeController.Remove(employee.Id);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromService_OnlyOnce()
        {
            var employee = CreateEmployee();
            _employeeServiceMock.Setup(c => c.GetById(employee.Id)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(c => c.Remove(employee)).ReturnsAsync(true);

            await _employeeController.Remove(employee.Id);

            _employeeServiceMock.Verify(mock => mock.Remove(employee), Times.Once);
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

        private EmployeeReturnDto MapModelToEmployeeReturnDto(Employee employee)
        {
            var employeeDto = new EmployeeReturnDto()
            {
                Id = employee.Id,
                EmpCode = employee.EmpCode,
                EmpName = employee.EmpName,
                Address = employee.Address,
                Age = employee.DateOfBirth.CalculateAge(),
                DepartmentId = employee.DepartmentId,
                Mobile = employee.Mobile,
                Salary = employee.Salary
            };
            return employeeDto;
        }

        private List<Employee> CreateEmployeeList()
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

        private List<EmployeeReturnDto> MapModelToEmployeeListDto(List<Employee> employees)
        {
            var listEmployees = new List<EmployeeReturnDto>();

            foreach (var item in employees)
            {
                var employee = new EmployeeReturnDto()
                {
                    Id = item.Id,
                    EmpCode = item.EmpCode,
                    EmpName = item.EmpName,
                    Address = item.Address,
                    Age = item.DateOfBirth.CalculateAge(),
                    DepartmentId = item.DepartmentId,
                    Mobile = item.Mobile,
                    Salary = item.Salary
                };
                listEmployees.Add(employee);
            }
            return listEmployees;
        }
    }
}
