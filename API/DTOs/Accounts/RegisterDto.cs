using API.DTOs.Educations;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Accounts
{
    public class RegisterDto : GeneralEmployeeDto
    {
        public string Major { get; set; }
        public string Degree { get; set; }
        public float Gpa { get; set; }
        public string UniversityCode { get; set; }
        public string UniversityName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        // Operator konversi implisit dari CreateEmployeeDto ke Employee
        public static implicit operator Employee(RegisterDto registerEmployeeDto)
        {
            return new Employee
            {
                FirstName = registerEmployeeDto.FirstName,
                LastName = registerEmployeeDto.LastName,
                BirthDate = registerEmployeeDto.BirthDate,
                Gender = registerEmployeeDto.Gender,
                HiringDate = registerEmployeeDto.HiringDate,
                Email = registerEmployeeDto.Email,
                PhoneNumber = registerEmployeeDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        // Operator konversi implisit dari CreateEmployeeDto ke Employee
        public static implicit operator Education(RegisterDto registerEmployeeDto)
        {
            return new Education
            {
                Major = registerEmployeeDto.Major,
                Degree = registerEmployeeDto.Degree,
                Gpa = registerEmployeeDto.Gpa,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        // Operator implisit yang memungkinkan konversi dari CreateUniversityDto ke University.
        public static implicit operator University(RegisterDto registerEmployeeDto)
        {
            // Menginisialisasi entitas University dengan data dari CreateUniversityDto.
            return new University
            {
                Code = registerEmployeeDto.UniversityCode,  // Menggunakan kode dari DTO.
                Name = registerEmployeeDto.UniversityName,  // Menggunakan nama dari DTO.
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Account(RegisterDto registerEmployeeDto)
        {
            return new Account
            {
                Password = registerEmployeeDto.Password,
                ExpiredDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
