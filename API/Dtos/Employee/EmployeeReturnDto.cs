namespace API.Dtos.Employee
{
    public class EmployeeReturnDto
    {
        public int Id { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal Salary { get; set; }
    }
}
