using API.DTOs.Roles;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.Educations
{
    public class CreateEducationDto
    {
        public string Major { get; set; }
        public string Degree { get; set; }
        public float Gpa { get; set; }
        public Guid UniversityGuid { get; set; }

        // Operator konversi implisit dari CreateEducationDto ke Education
        public static implicit operator Education(CreateEducationDto createEducationDto)
        {
            return new Education
            {
                Major = createEducationDto.Major,
                Degree = createEducationDto.Degree,
                Gpa = createEducationDto.Gpa,
                UniversityGuid = createEducationDto.UniversityGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
