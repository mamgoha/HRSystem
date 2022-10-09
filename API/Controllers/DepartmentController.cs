using API.Dtos.Department;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class DepartmentController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAll();
            return Ok(_mapper.Map<IReadOnlyList<DepartmentReturnDto>>(departments));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetById(id);

            if (department == null) return NotFound(new ApiResponse(404, "Department Not Found"));

            return Ok(_mapper.Map<DepartmentReturnDto>(department));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(DepartmentAddDto departmentDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            var department = _mapper.Map<Department>(departmentDto);
            var departmentResult = await _departmentService.Add(department);

            if (departmentResult == null) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            return Ok(_mapper.Map<DepartmentReturnDto>(departmentResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, DepartmentEditDto departmentDto)
        {
            if (id != departmentDto.Id) return BadRequest(new ApiResponse(400, "Check your updated data"));

            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            await _departmentService.Update(_mapper.Map<Department>(departmentDto));

            return Ok(departmentDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null) return NotFound(new ApiResponse(404, "Department doesn't exists"));

            var result = await _departmentService.Remove(department);

            if (!result) return BadRequest(new ApiResponse(404, "Data doesn't deleted, Internal server error"));

            return Ok();
        }

    }
}
