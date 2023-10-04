using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class EmployeeDto : GeneralEmployeeDto
    {
        public Guid Guid { get; set; }
        public string Nik { get; set; }

        // Operator konversi eksplisit dari Employee ke EmployeeDto
        public static explicit operator EmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Guid = employee.Guid,
                Nik = employee.Nik,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber
            };
        }

        // Operator konversi implisit dari EmployeeDto ke Employee
        public static implicit operator Employee(EmployeeDto employeeDto)
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
