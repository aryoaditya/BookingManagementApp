using API.Models;

namespace API.DTOs.Employees
{
    public class UpdateEmployeeDto : GeneralEmployeeDto
    {
        public Guid Guid { get; set; }
        public string Nik { get; set; }

        // Operator konversi implisit dari EmployeeDto ke Employee
        public static implicit operator Employee(UpdateEmployeeDto employeeDto)
        {
            return new Employee
            {
                Guid = employeeDto.Guid,
                Nik = employeeDto.Nik,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                BirthDate = employeeDto.BirthDate,
                Gender = employeeDto.Gender,
                HiringDate = employeeDto.HiringDate,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                ModifiedDate = DateTime.Now
            };
        }
    }
}