using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Employee
{
    public class EmployeeAddDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 4)]
        public string EmpCode { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string EmpName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public decimal Salary { get; set; }
    }
}
