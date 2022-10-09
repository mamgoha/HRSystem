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
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
        private readonly Mock<IEmployeeService> _employeeService;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartmentRepository>();
            _employeeService = new Mock<IEmployeeService>();
            _departmentService = new DepartmentService(_departmentRepositoryMock.Object, _employeeService.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfDepartments_WhenDepartmentsExist()
        {
            var departments = CreateDepartmentList();

            _departmentRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync(departments);

            var result = await _departmentService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Department>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenDepartmentsDoNotExist()
        {
            _departmentRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((IReadOnlyList<Department>)null);

            var result = await _departmentService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _departmentRepositoryMock.Setup(c =>
                c.GetAll()).ReturnsAsync((IReadOnlyList<Department>)null);

            await _departmentService.GetAll();

            _departmentRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnDepartment_WhenDepartmentExist()
        {
            var department = CreateDepartment();

            _departmentRepositoryMock.Setup(c =>
                c.GetById(department.Id)).ReturnsAsync(department);

            var result = await _departmentService.GetById(department.Id);

            Assert.NotNull(result);
            Assert.IsType<Department>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            _departmentRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Department)null);

            var result = await _departmentService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _departmentRepositoryMock.Setup(c =>
                c.GetById(1)).ReturnsAsync((Department)null);

            await _departmentService.GetById(1);

            _departmentRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        [Fact]
        public async void Add_ShouldAddDepartment_WhenDepartmentNameDoesNotExist()
        {
            var department = CreateDepartment();

            _departmentRepositoryMock.Setup(c =>
                c.IsExists(c => c.DepartmentName == department.DepartmentName))
                .ReturnsAsync(new List<Department>());
            _departmentRepositoryMock.Setup(c => c.Add(department));

            var result = await _departmentService.Add(department);

            Assert.NotNull(result);
            Assert.IsType<Department>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddDepartment_WhenDepartmentNameAlreadyExist()
        {
            var department = CreateDepartment();
            var departmentList = new List<Department>() { department };

            _departmentRepositoryMock.Setup(c =>
                c.IsExists(c => c.DepartmentName == department.DepartmentName)).ReturnsAsync(departmentList);

            var result = await _departmentService.Add(department);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var department = CreateDepartment();

            _departmentRepositoryMock.Setup(c =>
                    c.IsExists(c => c.DepartmentName == department.DepartmentName))
                .ReturnsAsync(new List<Department>());
            _departmentRepositoryMock.Setup(c => c.Add(department));

            await _departmentService.Add(department);

            _departmentRepositoryMock.Verify(mock => mock.Add(department), Times.Once);
        }

        [Fact]
        public async void Update_ShouldUpdateDepartment_WhenDepartmentNameDoesNotExist()
        {
            var department = CreateDepartment();

            _departmentRepositoryMock.Setup(c =>
                c.IsExists(c => c.DepartmentName == department.DepartmentName && c.Id != department.Id))
                .ReturnsAsync(new List<Department>());
            _departmentRepositoryMock.Setup(c => c.Update(department));

            var result = await _departmentService.Update(department);

            Assert.NotNull(result);
            Assert.IsType<Department>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateDepartment_WhenDepartmentDoesNotExist()
        {
            var department = CreateDepartment();
            var departmentList = new List<Department>()
            {
                new Department()
                {
                    Id = 2,
                    DepartmentName = "Department 2"
                }
            };

            _departmentRepositoryMock.Setup(c =>
                    c.IsExists(c => c.DepartmentName == department.DepartmentName && c.Id != department.Id))
                .ReturnsAsync(departmentList);

            var result = await _departmentService.Update(department);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var department = CreateDepartment();

            _departmentRepositoryMock.Setup(c =>
                    c.IsExists(c => c.DepartmentName == department.DepartmentName && c.Id != department.Id))
                .ReturnsAsync(new List<Department>());

            await _departmentService.Update(department);

            _departmentRepositoryMock.Verify(mock => mock.Update(department), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldRemoveDepartment_WhenDepartmentDoNotHaveRelatedEmployees()
        {
            var department = CreateDepartment();

            _employeeService.Setup(b =>
                b.GetEmployeesByDepartment(department.Id)).ReturnsAsync(new List<Employee>());

            var result = await _departmentService.Remove(department);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldNotRemoveDepartment_WhenDepartmentHasRelatedEmployees()
        {
            var department = CreateDepartment();

            var employees = new List<Employee>()
            {
                new Employee()
                {
                    Id = 1,
                    EmpCode = "0001",
                    EmpName = "Employee 1",
                    DateOfBirth = DateTime.Parse("07-07-1987"),
                    Address = "Address 1",
                    Mobile = "0123456789",
                    Salary = 5000,
                    DepartmentId = department.Id,
                }
            };

            _employeeService.Setup(b => b.GetEmployeesByDepartment(department.Id)).ReturnsAsync(employees);

            var result = await _departmentService.Remove(department);

            Assert.False(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var department = CreateDepartment();

            _employeeService.Setup(b =>
                b.GetEmployeesByDepartment(department.Id)).ReturnsAsync(new List<Employee>());

            await _departmentService.Remove(department);

            _departmentRepositoryMock.Verify(mock => mock.Remove(department), Times.Once);
        }

        private Department CreateDepartment()
        {
            return new Department()
            {
                Id = 1,
                DepartmentName = "Department 1"
            };
        }

        private IReadOnlyList<Department> CreateDepartmentList()
        {
            return new List<Department>()
            {
                new Department()
                {
                    Id = 1,
                    DepartmentName = "Department 1"
                },
                new Department()
                {
                    Id = 2,
                    DepartmentName = "Department 2"
                },
                new Department()
                {
                    Id = 3,
                    DepartmentName = "Department 3"
                }
            };
        }
    }
}
