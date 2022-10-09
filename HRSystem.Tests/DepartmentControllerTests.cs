using API.Controllers;
using API.Dtos.Department;
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
    public class DepartmentControllerTests
    {
        private readonly DepartmentController _departmentController;
        private readonly Mock<IDepartmentService> _departmentServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public DepartmentControllerTests()
        {
            _departmentServiceMock = new Mock<IDepartmentService>();
            _mapperMock = new Mock<IMapper>();
            _departmentController = new DepartmentController(_departmentServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistDepartment()
        {
            var departments = CreateDepartmentList();
            var dtoExpected = MapModelToDepartmentListDto(departments);

            _departmentServiceMock.Setup(c => c.GetAll()).ReturnsAsync(departments);
            _mapperMock.Setup(m => m.Map<IEnumerable<DepartmentReturnDto>>(
                It.IsAny<List<Department>>())).Returns(dtoExpected);

            var result = await _departmentController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyDepartment()
        {
            var departments = new List<Department>();
            var dtoExpected = MapModelToDepartmentListDto(departments);

            _departmentServiceMock.Setup(c => c.GetAll()).ReturnsAsync(departments);
            _mapperMock.Setup(m => m.Map<IEnumerable<DepartmentReturnDto>>(It.IsAny<List<Department>>())).Returns(dtoExpected);

            var result = await _departmentController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_OnlyOnce()
        {
            var departments = CreateDepartmentList();
            var dtoExpected = MapModelToDepartmentListDto(departments);

            _departmentServiceMock.Setup(c => c.GetAll()).ReturnsAsync(departments);
            _mapperMock.Setup(m => m.Map<IEnumerable<DepartmentReturnDto>>(It.IsAny<List<Department>>())).Returns(dtoExpected);

            await _departmentController.GetAll();

            _departmentServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnOk_WhenDepartmentExist()
        {
            var department = CreateDepartment();
            var dtoExpected = MapModelToDepartmentReturnDto(department);

            _departmentServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(department);
            _mapperMock.Setup(m => m.Map<DepartmentReturnDto>(It.IsAny<Department>())).Returns(dtoExpected);

            var result = await _departmentController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            _departmentServiceMock.Setup(c => c.GetById(2)).ReturnsAsync((Department)null);

            var result = await _departmentController.GetById(2);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_OnlyOnce()
        {
            var department = CreateDepartment();
            var dtoExpected = MapModelToDepartmentReturnDto(department);

            _departmentServiceMock.Setup(c => c.GetById(2)).ReturnsAsync(department);
            _mapperMock.Setup(m => m.Map<DepartmentReturnDto>(It.IsAny<Department>())).Returns(dtoExpected);

            await _departmentController.GetById(2);

            _departmentServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }

        [Fact]
        public async void Add_ShouldReturnOk_WhenDepartmentIsAdded()
        {
            var department = CreateDepartment();
            var departmentAddDto = new DepartmentAddDto() { DepartmentName = department.DepartmentName };
            var departmentResultDto = MapModelToDepartmentReturnDto(department);

            _mapperMock.Setup(m => m.Map<Department>(It.IsAny<DepartmentAddDto>())).Returns(department);
            _departmentServiceMock.Setup(c => c.Add(department)).ReturnsAsync(department);
            _mapperMock.Setup(m => m.Map<DepartmentReturnDto>(It.IsAny<Department>())).Returns(departmentResultDto);

            var result = await _departmentController.Add(departmentAddDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var departmentAddDto = new DepartmentAddDto();
            _departmentController.ModelState.AddModelError("Department Name", "The field department name is required");

            var result = await _departmentController.Add(departmentAddDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenDepartmentResultIsNull()
        {
            var department = CreateDepartment();
            var departmentAddDto = new DepartmentAddDto() { DepartmentName = department.DepartmentName };

            _mapperMock.Setup(m => m.Map<Department>(It.IsAny<DepartmentAddDto>())).Returns(department);
            _departmentServiceMock.Setup(c => c.Add(department)).ReturnsAsync((Department)null);

            var result = await _departmentController.Add(departmentAddDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromService_OnlyOnce()
        {
            var department = CreateDepartment();
            var departmentAddDto = new DepartmentAddDto() { DepartmentName = department.DepartmentName };

            _mapperMock.Setup(m => m.Map<Department>(It.IsAny<DepartmentAddDto>())).Returns(department);
            _departmentServiceMock.Setup(c => c.Add(department)).ReturnsAsync(department);

            await _departmentController.Add(departmentAddDto);

            _departmentServiceMock.Verify(mock => mock.Add(department), Times.Once);
        }

        [Fact]
        public async void Update_ShouldReturnOk_WhenDepartmentIsUpdatedCorrectly()
        {
            var department = CreateDepartment();
            var departmentEditDto = new DepartmentEditDto() { Id = department.Id, DepartmentName = "Testing" };

            _mapperMock.Setup(m => m.Map<Department>(It.IsAny<DepartmentEditDto>())).Returns(department);
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync(department);
            _departmentServiceMock.Setup(c => c.Update(department)).ReturnsAsync(department);

            var result = await _departmentController.Update(departmentEditDto.Id, departmentEditDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenDepartmentIdIsDifferentThenParameterId()
        {
            var departmentEditDto = new DepartmentEditDto() { Id = 1, DepartmentName = "Test" };

            var result = await _departmentController.Update(2, departmentEditDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var departmentEditDto = new DepartmentEditDto() { Id = 1 };
            _departmentController.ModelState.AddModelError("DepartmentName", "The field name is required");

            var result = await _departmentController.Update(1, departmentEditDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromService_OnlyOnce()
        {
            var department = CreateDepartment();
            var departmentEditDto = new DepartmentEditDto() { Id = department.Id, DepartmentName = "Test" };

            _mapperMock.Setup(m => m.Map<Department>(It.IsAny<DepartmentEditDto>())).Returns(department);
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync(department);
            _departmentServiceMock.Setup(c => c.Update(department)).ReturnsAsync(department);

            await _departmentController.Update(departmentEditDto.Id, departmentEditDto);

            _departmentServiceMock.Verify(mock => mock.Update(department), Times.Once);
        }

        [Fact]
        public async void Remove_ShouldReturnOk_WhenDepartmentIsRemoved()
        {
            var department = CreateDepartment();
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync(department);
            _departmentServiceMock.Setup(c => c.Remove(department)).ReturnsAsync(true);

            var result = await _departmentController.Remove(department.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            var department = CreateDepartment();
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync((Department)null);

            var result = await _departmentController.Remove(department.Id);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnBadRequest_WhenResultIsFalse()
        {
            var department = CreateDepartment();
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync(department);
            _departmentServiceMock.Setup(c => c.Remove(department)).ReturnsAsync(false);

            var result = await _departmentController.Remove(department.Id);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromService_OnlyOnce()
        {
            var department = CreateDepartment();
            _departmentServiceMock.Setup(c => c.GetById(department.Id)).ReturnsAsync(department);
            _departmentServiceMock.Setup(c => c.Remove(department)).ReturnsAsync(true);

            await _departmentController.Remove(department.Id);

            _departmentServiceMock.Verify(mock => mock.Remove(department), Times.Once);
        }

        private Department CreateDepartment()
        {
            return new Department()
            {
                Id = 2,
                DepartmentName = "Department 2"
            };
        }

        private DepartmentReturnDto MapModelToDepartmentReturnDto(Department department)
        {
            var departmentDto = new DepartmentReturnDto()
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName
            };
            return departmentDto;
        }

        private List<Department> CreateDepartmentList()
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

        private List<DepartmentReturnDto> MapModelToDepartmentListDto(List<Department> departments)
        {
            var listDepartments = new List<DepartmentReturnDto>();

            foreach (var item in departments)
            {
                var department = new DepartmentReturnDto()
                {
                    Id = item.Id,
                    DepartmentName = item.DepartmentName
                };
                listDepartments.Add(department);
            }
            return listDepartments;
        }
    }
}
