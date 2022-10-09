using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Department
{
    public class DepartmentAddDto
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string DepartmentName { get; set; }
    }
}
