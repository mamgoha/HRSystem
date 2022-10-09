using API.Dtos.Department;
using API.Dtos.Employee;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            return Ok(_mapper.Map<IReadOnlyList<EmployeeReturnDto>>(employees));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetById(id);

            if (employee == null) return NotFound(new ApiResponse(404, "Employee Not Found"));

            return Ok(_mapper.Map<EmployeeEditDto>(employee));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(EmployeeAddDto employeeDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            var employee = _mapper.Map<Employee>(employeeDto);
            var employeeResult = await _employeeService.Add(employee);

            if (employeeResult == null) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            return Ok(_mapper.Map<EmployeeReturnDto>(employeeResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, EmployeeEditDto employeeDto)
        {
            if (id != employeeDto.Id) return BadRequest(new ApiResponse(400, "Check your updated data"));

            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Not Valid Data"));

            await _employeeService.Update(_mapper.Map<Employee>(employeeDto));

            return Ok(employeeDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null) return NotFound();

            var result = await _employeeService.Remove(employee);

            if (!result) return BadRequest(new ApiResponse(404, "Data doesn't deleted, Internal server error"));

            return Ok();
        }
    }
}
