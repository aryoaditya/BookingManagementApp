using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class EmployeeDetailDto : GeneralEmployeeDto
    {
        public Guid Guid { get; set; }
        public string FullName { get; set; }
        public string Nik { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
        public float Gpa { get; set; }
        public string University { get; set; }
    }
}
