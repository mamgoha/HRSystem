using API.Dtos.Department;
using API.Dtos.Employee;
using API.Extensions;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, DepartmentReturnDto>().ReverseMap();
            CreateMap<Department, DepartmentAddDto>().ReverseMap();
            CreateMap<Department, DepartmentEditDto>().ReverseMap();

            CreateMap<Employee, EmployeeReturnDto>()
                .ForMember(e => e.DepartmentName, d => d.MapFrom(s => s.Department.DepartmentName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Employee, EmployeeAddDto>().ReverseMap();
            CreateMap<Employee, EmployeeEditDto>().ReverseMap();
        }
    }
}
