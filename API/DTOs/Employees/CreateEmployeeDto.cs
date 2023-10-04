using API.DTOs.Roles;
using API.Models;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Employees
{
    public class CreateEmployeeDto : GeneralEmployeeDto
    {
        // Operator konversi implisit dari CreateEmployeeDto ke Employee
        public static implicit operator Employee(CreateEmployeeDto createEmployeeDto)
        {
            return new Employee
            {
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                BirthDate = createEmployeeDto.BirthDate,
                Gender = createEmployeeDto.Gender,
                HiringDate = createEmployeeDto.HiringDate,
                Email = createEmployeeDto.Email,
                PhoneNumber = createEmployeeDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

    }
}
